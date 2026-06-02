ARCHITECTURE OVERVIEW
Two separate concerns, two separate mechanisms:

SignalR = user is inside the app, anywhere on any page. Bell updates instantly.
FCM = user is outside the app, backgrounded or closed. OS push notification delivered.

Both fire on every notification trigger. They are not alternatives. They work together.

NOTIFICATION TRIGGERS
There are exactly two:

User A sends a message to User B
Organizer marks a member as paid for a specific round


BACKEND IMPLEMENTATION PLAN
Step 1: Notification domain
Create a Notification entity with these fields:
Id
UserId (recipient)
Type (enum: NewMessage, PaymentMarkedAsPaid)
Title
Body
IsRead
CreatedAt
ReferenceId (GroupId or MessageId, for navigation on click)
Step 2: SaveNotificationCommand
Create a command that saves a notification record to the database. Both triggers call this command before doing anything else. The bell icon count comes from unread notifications in this table.
Step 3: Update SendMessageCommandHandler
After saving the message, the handler must do four things in this order:

Save message to DB
Save a Notification record for the recipient via SaveNotificationCommand
Send SignalR event to user-{receiverId} group with event name ReceiveNotification and the notification payload
Send SignalR event to user-{receiverId} group with event name ReceiveMessage and the message payload
Send FCM push to recipient's stored token

Step 4: Update MarkAsPaidCommandHandler
After marking payment as paid, the handler must do four things:

Save the payment update to DB
Save a Notification record for the member via SaveNotificationCommand
Send SignalR event to user-{memberId} group with event name ReceiveNotification and payload:

json{
  "type": "PaymentMarkedAsPaid",
  "title": "Payment Confirmed",
  "body": "Your payment for Round {roundNumber} in {groupName} has been marked as paid.",
  "referenceId": "{groupId}"
}

Send FCM push to member's stored token

Step 5: ChatHub fix
Add OnConnectedAsync and OnDisconnectedAsync as already outlined in the fix plan. Every user auto-joins user-{userId} group on connect. No manual JoinGroupChat needed for notifications to work.
Step 6: Notifications API endpoints
Add these three endpoints:
GET  /api/notifications          — returns all notifications for the current user, unread first
PUT  /api/notifications/read-all — marks all as read
PUT  /api/notifications/{id}/read — marks one as read

FRONTEND IMPLEMENTATION PLAN
Step 1: Global SignalR connection in NotificationProvider
This is the most important frontend change. SignalR must connect at the app level, not inside the chat component. The chat component has its own connection for messages. The global connection is only for notifications.
In NotificationProvider:
typescriptuseEffect(() => {
  if (!token) return;

  const connection = new HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_SIGNALR_HUB_URL, {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();

  connection.on('ReceiveNotification', (notification) => {
    addNotification(notification);      // adds to bell icon count
    showToast(notification.body);       // optional in-app toast
  });

  connection.start().catch(console.error);

  return () => { connection.stop(); };
}, [token]);
This runs once when the user logs in. It stays alive on every page. It only stops on logout.
Step 2: Bell icon component
The bell icon reads from your notification state:
typescriptconst { notifications } = useNotifications();
const unreadCount = notifications.filter(n => !n.isRead).length;
On mount, fetch existing notifications from GET /api/notifications to populate the initial count. After that, ReceiveNotification from SignalR keeps it updated in real time.
Step 3: Chat component keeps its own SignalR connection
useChat stays as is for ReceiveMessage. It handles real time message display inside the chat. This is separate from the global notification connection. Both connections run simultaneously when the user is inside the chat.
Step 4: Fix getToken() with explicit Service Worker
As already outlined. This fixes the refresh bug.
Step 5: FCM foreground handler
In NotificationContext.tsx, the onMessage foreground handler should also call addNotification so the bell updates even when FCM delivers while the app is open:
typescriptonMessage(messaging, (payload) => {
  addNotification({
    type: payload.data?.type,
    title: payload.notification?.title,
    body: payload.notification?.body,
    isRead: false,
    referenceId: payload.data?.referenceId,
  });
});

EXECUTION ORDER
Do backend first, then frontend.
Backend:

Create Notification entity and migration
Create SaveNotificationCommand and handler
Add OnConnectedAsync to ChatHub
Wrap SignalR call in try/catch in SendMessageCommandHandler
Add ReceiveNotification SignalR event in SendMessageCommandHandler after saving notification
Update MarkAsPaidCommandHandler to save notification and fire SignalR + FCM
Add the three notification API endpoints
Fix InfrastructureServiceExtensions.cs to read FIREBASE_ADMINSDK_JSON

Frontend:

Fix .gitignore and rotate VAPID key
Fix getToken() with explicit service worker registration
Update SW CDN version to match package.json
Add global SignalR connection in NotificationProvider
Add ReceiveNotification listener that updates bell icon
Fetch initial notifications from GET /api/notifications on login
Add onMessage foreground FCM handler that also calls addNotification
Set all variables in Vercel dashboard


EXPECTED BEHAVIOR AFTER ALL FIXES

User A sends message to User B
User B is on the dashboard. Bell icon updates instantly via global SignalR.
User B is outside the app. OS push notification arrives via FCM.
User B opens the app. Bell shows the unread count from the DB.
User B opens chat. Messages appear. Replies in real time.
User A is anywhere in the app. Bell updates instantly.
Organizer marks User B as paid. User B gets bell update instantly with professional payment text.
Refresh does not break anything. Service Worker is explicit. Global SignalR reconnects automatically. FCM token re-registers correctly
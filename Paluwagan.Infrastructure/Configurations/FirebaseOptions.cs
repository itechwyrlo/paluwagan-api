namespace Paluwagan.Infrastructure.Configurations
{
    public sealed class FirebaseOptions
    {
        public const string Section = "Firebase";

        public string ServiceAccountPath { get; init; } = string.Empty;
    }
}

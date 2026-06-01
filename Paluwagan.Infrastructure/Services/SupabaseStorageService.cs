using Microsoft.Extensions.Options;
using Paluwagan.Domain.Services;
using Paluwagan.Infrastructure.Configurations;

namespace Paluwagan.Infrastructure.Services
{
    public sealed class SupabaseStorageService(IHttpClientFactory httpClientFactory, IOptions<SupabaseOptions> opts) : IStorageService
    {
        private readonly SupabaseOptions _opts = opts.Value;

        public async Task<string> UploadAsync(string fileName, byte[] fileBytes, string contentType, CancellationToken ct = default)
        {
            var client = httpClientFactory.CreateClient("Supabase");
            var path = $"{Guid.NewGuid()}_{fileName}";
            var requestUri = $"{_opts.Url}/storage/v1/object/{_opts.BucketName}/{path}";

            using var content = new ByteArrayContent(fileBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = content
            };
            request.Headers.Add("Authorization", $"Bearer {_opts.ServiceRoleKey}");
            request.Headers.Add("x-upsert", "true");

            var response = await client.SendAsync(request, ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                throw new HttpRequestException($"Supabase upload failed ({response.StatusCode}): {body}");
            }

            return $"{_opts.Url}/storage/v1/object/public/{_opts.BucketName}/{path}";
        }
    }
}

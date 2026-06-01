namespace Paluwagan.Domain.Services
{
    public interface IStorageService
    {
        Task<string> UploadAsync(string fileName, byte[] fileBytes, string contentType, CancellationToken ct = default);
    }
}

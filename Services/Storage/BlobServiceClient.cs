using Azure.Storage.Blobs;


namespace ASP_spr321.Services.Storage

{
    public class BlobServiceClient : IStorageService
    {
        private const String connectionString = "https://serg.blob.core.windows.net/home";

        public string GetRealPath(string name)
        {
            return connectionString + name;
        }

        public string SaveFile(IFormFile formFile)
        {
            String savedName;
            String fullName;
            var ext = Path.GetExtension(formFile.FileName);
            do
            {
                savedName = Guid.NewGuid() + ext;
                fullName = connectionString + savedName;
            } while (File.Exists(fullName));

            formFile.CopyTo(new FileStream(fullName, FileMode.CreateNew));
            return savedName;
        }
    }
}

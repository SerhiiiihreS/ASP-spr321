
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASP_spr321.Services.Storage
{
    public class FileStorageService : IStorageService
    {
        private  const String storagePath = "C:\\Storage(ASP 311)\\";
        public string GetRealPath(string name)
        {
          return  storagePath + name; 
        }

        public string SaveFile(IFormFile formFile)
        {
            var ext=Path.GetExtension(formFile.FileName);
            String savedName;
            String fullName;
            do
            {
                savedName = Guid.NewGuid() + ext;
                fullName = storagePath + savedName;
            }
            while (File.Exists(fullName));

            formFile.CopyTo(new FileStream(fullName, FileMode.CreateNew));

            return savedName;
        }
    }
}

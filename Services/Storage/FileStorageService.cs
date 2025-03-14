
using static System.Net.Mime.MediaTypeNames;

namespace ASP_spr321.Services.Storage;
public class FileStorageService : IStorageService
{
    private const String storagePath = "C:\\StorageASP311\\";

    public string GetRealPath(string name)
    {
        return storagePath + name;
    }

    public string SaveFile(IFormFile formFile)
    {
        // 1. З імені файлу визначити розширення
        // 2. згенерувати нове ім'я зберігши розширення, переконатись в його унікальності
        // 3. скопіювати formFile до сховища під новим іменем
        String savedName;
        String fullName;
            var ext = Path.GetExtension(formFile.FileName);
        do
        {
            savedName = Guid.NewGuid() + ext;
            fullName = storagePath + savedName;
        } while (File.Exists(fullName));

        formFile.CopyTo(new FileStream(fullName, FileMode.CreateNew));
        return savedName;
    }
}

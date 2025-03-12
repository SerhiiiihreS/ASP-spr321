namespace ASP_spr321.Services.Storage
{
    public interface IStorageService
    {
        String SaveFile(IFormFile formFile);
        String GetRealPath(String name);
    }
}

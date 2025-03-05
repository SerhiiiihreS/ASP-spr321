namespace ASP_spr321.Services.Kdf
{
    public interface IKdfService
    {
        String DerivedKey(String password, String salt);
    }
}

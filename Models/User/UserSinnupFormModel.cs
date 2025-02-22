namespace ASP_spr321.Models.User
{
    public class UserSinnupFormModel
    {
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!; 
        public string UserPhone {  get; set; } = null!;
        public string UserPassword { get; set; }=null!;
        public string UserRepeat { get; set; } = null!;
        public string UserLogin { get; set;} = null!;
    }
}

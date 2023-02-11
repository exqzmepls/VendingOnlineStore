namespace VendingOnlineStore.Models.Manage;

public class ProfileViewModel
{
    public ProfileViewModel(string login, string city)
    {
        Login = login;
        City = city;
    }

    public string Login { get; }
    public string City { get; }
}
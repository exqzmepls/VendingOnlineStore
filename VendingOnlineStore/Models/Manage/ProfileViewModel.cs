namespace VendingOnlineStore.Models.Manage;

public class ProfileViewModel
{
    public ProfileViewModel(string login, int cityId)
    {
        Login = login;
        CityId = cityId;
    }

    public string Login { get; }
    public int CityId { get; }
}
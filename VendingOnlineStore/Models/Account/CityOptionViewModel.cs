namespace VendingOnlineStore.Models.Account;

public class CityOptionViewModel
{
    public CityOptionViewModel(int value, string text)
    {
        Value = value;
        Text = text;
    }

    public int Value { get; }
    public string Text { get; }
}
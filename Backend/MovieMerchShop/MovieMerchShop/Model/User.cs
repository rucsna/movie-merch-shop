namespace MovieMerchShop.Model;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public DateTime BirthDate { get; set; }
    private string _password;
    private decimal _balance;
    private readonly ICollection<Order> _orders;
    private readonly ICollection<MerchItem> _wishList;

    public User(string userName, DateTime birthDate, string password)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        BirthDate = birthDate;
        _password = password;
        _balance = 0;
        _orders = new List<Order>();
        _wishList = new List<MerchItem>();
    }

    
}
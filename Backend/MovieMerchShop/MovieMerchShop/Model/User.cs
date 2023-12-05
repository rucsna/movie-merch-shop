namespace MovieMerchShop.Model;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public DateTime BirthDate { get; set; }
    private string _password;
    private string _address;
    private decimal _balance;
    private ICollection<MerchItem> _cart;
    private readonly ICollection<Order> _orders;
    private readonly ICollection<MerchItem> _wishList;

    public User()
    {
    }
    public User(string userName, DateTime birthDate, string password, string address)
    {
        UserId = Guid.NewGuid();
        UserName = userName;
        BirthDate = birthDate;
        _password = password;
        _address = address;
        _balance = 0;
        _cart = new List<MerchItem>();
        _orders = new List<Order>();
        _wishList = new List<MerchItem>();
    }

    public void SetPassword(string newPassword)
    {
        _password = newPassword;
    }

    public void SetAddress(string newAddress)
    {
        _address = newAddress;
    }

    public string GetAddress()
    {
        return _address;
    }
    public void SetBalance(decimal balance)
    {
        _balance = balance;
    }

    public decimal GetBalance()
    {
        return _balance;
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    public IEnumerable<Order> GetOrders()
    {
        return _orders;
    }

    public void AddMerchItem(MerchItem merchItem)
    {
        _wishList.Add(merchItem);
    }

    public IEnumerable<MerchItem> GetMerchItems()
    {
        return _wishList;
    }
}
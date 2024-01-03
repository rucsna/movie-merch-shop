using Microsoft.AspNetCore.Identity;

namespace MovieMerchShop.Model;

public class ApplicationUser : IdentityUser
{ 
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    private ICollection<MerchItem> _cart;
    private readonly ICollection<Order> _orders;
    private readonly ICollection<MerchItem> _wishList;

    public ApplicationUser()
    {
    }
    public ApplicationUser(DateTime birthDate, string password, string address)
    {
        BirthDate = birthDate;
        Address = address;
        Balance = 0;
        IsActive = true;
        _cart = new List<MerchItem>();
        _orders = new List<Order>();
        _wishList = new List<MerchItem>();
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
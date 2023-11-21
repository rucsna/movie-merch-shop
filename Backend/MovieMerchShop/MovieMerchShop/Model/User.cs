namespace MovieMerchShop.Model;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    //public ICollection<Order> Orders { get; set; }
    //public ICollection<MerchItem> WishList { get; set; }
    public decimal Balance { get; set; }
}
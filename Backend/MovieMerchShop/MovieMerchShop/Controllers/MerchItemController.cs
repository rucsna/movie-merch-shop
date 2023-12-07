using Microsoft.AspNetCore.Mvc;
using MovieMerchShop.Model;
using MovieMerchShop.Service;

namespace MovieMerchShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MerchItemController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MerchItemController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult GetAllItems()
    {
        List<MerchItem> items = _dbContext.MerchItems.ToList();
        return Ok(items);
    }
    
    [HttpPost]
    public IActionResult AddNewItem(MerchItem newItem)
    {
        try
        {
            _dbContext.MerchItems.Add(newItem);
            _dbContext.SaveChanges();
            return Content(newItem.Id.ToString());
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    // get item by id
    // get all by movieId
    // get all by userId (/ order?)
}
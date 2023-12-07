using System.ComponentModel.DataAnnotations;
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
    
    [HttpGet("/{id}")]
    public IActionResult GetItemById(Guid id)
    {
        try
        {
            var user = _dbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpGet("/ItemsByMovie/{movieId}")]
    public IActionResult GetItemsByMovie([Required]Guid movieId)
    {
        try
        {
            var items = _dbContext.MerchItems.Select(item => item.MovieId == movieId).ToList();
            return Ok(items);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
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

    
    // get all by movieId
    // get all by userId (/ order?)
}
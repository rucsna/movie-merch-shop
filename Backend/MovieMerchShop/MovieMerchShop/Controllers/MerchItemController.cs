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
    
    [HttpGet("{id}")]
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

    [HttpGet("ItemsByMovie/{movieId}")]
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
    
    [HttpGet("ItemsByUser/{userId}")]
    public IActionResult GetItemsByUser([Required]Guid userId)
    {
        try
        {
            var items = _dbContext.MerchItems.Select(item => item.MovieId == userId).ToList();
            return Ok(items);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost("Mug")]
    public IActionResult AddNewMug(Mug newMug)
    {
        try
        {
            _dbContext.MerchItems.Add(newMug);
            _dbContext.SaveChanges();
            return Content(newMug.Id.ToString());
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("Shirt")]
    public IActionResult AddNewShirt(Shirt newShirt)
    {
        try
        {
            _dbContext.MerchItems.Add(newShirt);
            _dbContext.SaveChanges();
            return Content(newShirt.Id.ToString());
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost("Poster")]
    public IActionResult AddNewPoster(Poster newPoster)
    {
        try
        {
            _dbContext.MerchItems.Add(newPoster);
            _dbContext.SaveChanges();
            return Content(newPoster.Id.ToString());
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
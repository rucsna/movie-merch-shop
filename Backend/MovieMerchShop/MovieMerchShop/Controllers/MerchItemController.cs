using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MovieMerchShop.Data;
using MovieMerchShop.Model;
using MovieMerchShop.Service;
using MovieMerchShop.Service.Repository;

namespace MovieMerchShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MerchItemController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IMerchItemRepository _itemRepository;

    public MerchItemController(AppDbContext dbContext, IMerchItemRepository itemRepository)
    {
        _dbContext = dbContext;
        _itemRepository = itemRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        try
        {
            var items = await _itemRepository.GetAllItemsAsync();
            return Ok(items);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Error getting items");
        }
    }
    
    // [HttpGet("{id}")]
    // public IActionResult GetItemById(Guid id)
    // {
    //     try
    //     {
    //         var user = _dbContext.Users.Find(id);
    //
    //         if (user == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         return Ok();
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e);
    //     }
    // }

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
    
    [HttpGet("ItemsByMovie/{movieId}/{itemType}")]
    public IActionResult GetItemsByMovie([Required] Guid movieId, [Required] ItemType itemType)
    {
        try
        {
            switch (itemType)
            {
                case ItemType.Mug:
                    var mugList = _dbContext.MerchItems
                        .Where(item => item.MovieId == movieId && item.Type == ItemType.Mug)
                        .OfType<Mug>()
                        .ToList();
                    return Ok(mugList);

                case ItemType.Shirt:
                    var shirtList = _dbContext.MerchItems
                        .Where(item => item.MovieId == movieId && item.Type == ItemType.Shirt)
                        .OfType<Shirt>()
                        .ToList();
                    return Ok(shirtList);

                case ItemType.Poster:
                    var posterList = _dbContext.MerchItems
                        .Where(item => item.MovieId == movieId && item.Type == ItemType.Poster)
                        .OfType<Poster>()
                        .ToList();
                    return Ok(posterList);

                default:
                    return BadRequest("Invalid item type");
            }
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
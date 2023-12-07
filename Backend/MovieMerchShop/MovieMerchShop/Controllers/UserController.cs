using Microsoft.AspNetCore.Mvc;
using MovieMerchShop.Model;
using MovieMerchShop.Service;

namespace MovieMerchShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpPost]
    public ActionResult<User> AddUser(User newUser)
    {
        if (newUser == null)
        {
            BadRequest();
        }
        
        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(AddUser), new { id = newUser.UserId }, newUser);
    }

    [HttpGet("/{userId}")]
    public ActionResult<User> GetUserById(Guid userId)
    {
        var user = _dbContext.Users.Find(userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("/{userId}")]
    public ActionResult<User> DeleteUser(Guid userId)
    {
        var userToDelete = _dbContext.Users.FirstOrDefault(user => user.UserId == userId);

        if (userToDelete == null)
        {
            return NotFound();
        }

        _dbContext.Users.Remove(userToDelete);
        _dbContext.SaveChanges();

        return Ok($"User with id {userId} has been deleted.");
    }
}
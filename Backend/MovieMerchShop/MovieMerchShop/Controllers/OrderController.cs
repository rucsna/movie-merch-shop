using Microsoft.AspNetCore.Mvc;
using MovieMerchShop.Model;
using MovieMerchShop.Service;
using System.Linq;
using MovieMerchShop.Data;

namespace MovieMerchShop.Controllers;
[ApiController]
[Route("api/[controller]")]

public class OrderController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public OrderController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("GetAllOrders")]
    public ActionResult<IEnumerable<Order>> GetAllOrders()
    {
        List<Order> orders = _context.Orders.ToList();
        return Ok(orders);
    }
    
    [HttpGet("GetOrderById/{id}")]
    public ActionResult<Order> GetOrderById(Guid id)
    {
        var order = _context.Orders.Find(id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }
    
    [HttpGet("GetOrderByUserId/{userid}")]
    public ActionResult<Order> GetOrderByUserId(Guid userid)
    {
        var orders = _context.Orders.Where(order => order.UserId == userid).ToList();

        if (orders == null)
        {
            return NotFound(); 
        }

        return Ok(orders);
    }

    [HttpPost("AddOrder")]
    public ActionResult<Order> AddOrder(Order newOrder)
    {
        if (newOrder == null)
        {
            return BadRequest(); 
        }

        _context.Orders.Add(newOrder);
        _context.SaveChanges();
        return Content(newOrder.Id.ToString());
        // return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
    }
    
    [HttpDelete("DeleteOrderById/{orderid}")]
    public ActionResult DeleteOrderById(Guid orderid)
    {
        var orderToDelete = _context.Orders.FirstOrDefault(order => order.Id == orderid);
    
        if (orderToDelete == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(orderToDelete);
        _context.SaveChanges();

        return Ok($"Order with ID {orderid} has been deleted.");
    }
}

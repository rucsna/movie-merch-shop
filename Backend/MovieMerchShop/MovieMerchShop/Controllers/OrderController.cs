using Microsoft.AspNetCore.Mvc;
using MovieMerchShop.Model;
using MovieMerchShop.Service;
using System.Linq;
using MovieMerchShop.Contracts;
using MovieMerchShop.Data;
using MovieMerchShop.Service.Repository;

namespace MovieMerchShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMerchItemRepository _itemRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderController(AppDbContext context, IMerchItemRepository itemRepository, IOrderRepository orderRepository)
    {
        _context = context;
        _itemRepository = itemRepository;
        _orderRepository = orderRepository;
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
    public async Task<ActionResult<Order>> AddOrderAsync([FromBody] OrderRequest request)
    {
        if (request.OrderedItemIds.Count == 0)
        {
            return NotFound("The cart is empty");
        }

        var orderedItems = new List<MerchItem>();
        
        foreach (var itemId in request.OrderedItemIds)
        {
            var itemToAddOrder = await _itemRepository.GetItemByIdAsync(itemId);
            if (itemToAddOrder == null)
            {
                return NotFound("There is no such item in the database");
            }
            orderedItems.Add(itemToAddOrder);
            
            if (itemToAddOrder.Quantity < 1)
            {
                return BadRequest($"Not enough item in the store");
            }

            itemToAddOrder.Quantity -= 1;
            
            await _itemRepository.UpdateItemAsync(itemToAddOrder);
            //return Ok($"Item with id {itemToAddOrder.Id} updated quantity is {itemToAddOrder.Quantity}");
        }
        
        var newOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Items = orderedItems,
                    OrderSum = request.OrderSum,
                    OrderTime = DateTime.Now
                };

        await _orderRepository.CreateNewOrderAsync(newOrder);
        return Ok(newOrder);
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
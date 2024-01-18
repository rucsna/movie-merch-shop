using Microsoft.AspNetCore.Mvc;
using MovieMerchShop.Model;
using Microsoft.AspNetCore.Authorization;
using MovieMerchShop.Contracts;
using MovieMerchShop.Service.Repository;

namespace MovieMerchShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMerchItemRepository _itemRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IMerchItemRepository itemRepository, IOrderRepository orderRepository, IUserRepository userRepository, ILogger<OrderController> logger)
    {
        _itemRepository = itemRepository;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting orders");
            return BadRequest("Error getting orders");
        }
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetOrdersByUserId/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(string userId)
    {
        try
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            if (!orders.Any())
            {
                return NotFound("This user doesn't have any orders");
            }
            return Ok(orders);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting orders by user id");
            return BadRequest("Error getting orders");
        }
    }

    [Authorize(Roles = "User")]
    [HttpPost("AddOrder")]
    public async Task<IActionResult> AddOrderAsync([FromBody] OrderRequest request)
    {
        var userInDb = await _userRepository.GetUserByEmailAsync(request.UserEmail);
        if (userInDb == null)
        {
            _logger.LogError("No user found in the database with email address {userEmail}", request.UserEmail);
            return NotFound("No user found in the database");
        }
        
        if (request.OrderedItemIds.Count == 0)
        {
            _logger.LogError("No items in the shopping cart");
            return NoContent();
        }

        var orderedItems = new List<MerchItem>();

        foreach (var itemId in request.OrderedItemIds)
        {
            var itemToAddOrder = await _itemRepository.GetItemByIdAsync(itemId);
            if (itemToAddOrder == null)
            {
                _logger.LogError("No item found in the database with id {itemId}", itemId);
                return NotFound("No item found in the database");
            }
            
            _logger.LogInformation("Item found in the database");
            orderedItems.Add(itemToAddOrder);

            if (itemToAddOrder.Quantity < 1)
            {
                _logger.LogError("Item quantity in the database is 0");
                return BadRequest($"Not enough item in the store");
            }

            itemToAddOrder.Quantity -= 1;
            _logger.LogInformation("Item quantity successfully decreased. Quantity: {quantity}", itemToAddOrder.Quantity);

            await _itemRepository.UpdateItemAsync(itemToAddOrder);
        }

        var newOrder = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userInDb.Id,
            Items = orderedItems,
            OrderSum = request.OrderSum,
            OrderTime = DateTime.Now
        };
        decimal minusSum = request.OrderSum * (-1);
        await _orderRepository.CreateNewOrderAsync(newOrder);
        await _userRepository.UpdateBalanceAsync(request.UserEmail, minusSum);
        return Ok(newOrder);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteOrderById/{orderId:guid}")]
    public async Task<IActionResult> DeleteOrderById(Guid orderId)
    {
        try
        {
            var orderToDelete = await _orderRepository.GetOrderByIdAsync(orderId);
            if (orderToDelete == null)
            {
                _logger.LogError("No order found in database with id {orderId}", orderId);
                return NotFound("No order found in database");
            }

            await _orderRepository.DeleteOrderAsync(orderToDelete);
            return Ok($"Order with id {orderToDelete.Id} successfully deleted");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error with deleting order");
            return BadRequest("Error with deleting order");
        }
    }
}
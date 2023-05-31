using Integration_Test.Models;
using Integration_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Integration_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult GetAllOrder()
        {
            try
            {
                List<OrderEntity> orderList = _orderService.GetAllOrder();

                return Ok(orderList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllOrder Error: {ex}");
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// 取得訂單
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult GetOrder(int orderID)
        {
            try
            {
                OrderEntity? order = _orderService.GetOrder(orderID);

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrder Error: {ex}");
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult CreateOrder()
        {
            try
            {
                bool isSuccess = _orderService.CreateOrder();

                if (!isSuccess)
                    throw new Exception("新增訂單異常");

                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrder Error: {ex}");
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// 編輯訂單
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult EditOrder(int orderID, int price)
        {
            try
            {
                bool isSuccess = _orderService.EditOrder(orderID, price);

                if (!isSuccess)
                    throw new Exception("編輯訂單異常");

                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"EditOrder Error: {ex}");
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult DeleteOrder(int orderID)
        {
            try
            {
                bool isSuccess = _orderService.DeleteOrder(orderID);

                if (!isSuccess)
                    throw new Exception("建單異常");

                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteOrder Error: {ex}");
                return BadRequest("Error");
            }
        }
    }
}
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
        /// ���o�Ҧ��q��
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
        /// ���o�q��
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
        /// �s�W�q��
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult CreateOrder()
        {
            try
            {
                bool isSuccess = _orderService.CreateOrder();

                if (!isSuccess)
                    throw new Exception("�s�W�q�沧�`");

                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrder Error: {ex}");
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// �s��q��
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult EditOrder(int orderID, int price)
        {
            try
            {
                bool isSuccess = _orderService.EditOrder(orderID, price);

                if (!isSuccess)
                    throw new Exception("�s��q�沧�`");

                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError($"EditOrder Error: {ex}");
                return BadRequest("Error");
            }
        }

        /// <summary>
        /// �R���q��
        /// </summary>
        [HttpGet("[Action]")]
        public IActionResult DeleteOrder(int orderID)
        {
            try
            {
                bool isSuccess = _orderService.DeleteOrder(orderID);

                if (!isSuccess)
                    throw new Exception("�س沧�`");

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
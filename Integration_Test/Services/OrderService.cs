using Dapper;
using Integration_Test.Models;
using System.Data;
using System.Security.Cryptography;

namespace Integration_Test.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IDbConnection _conn;

        public OrderService(
            ILogger<OrderService> logger,
            IDbConnection conn)
        {
            this._logger = logger;
            this._conn = conn;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        public bool CreateOrder()
        {
            try
            {
                var order = new OrderEntity
                {
                    Price = RandomNumberGenerator.GetInt32(1, 1000000),
                    CreateDate = DateTime.Now
                };

                return this._conn.Execute("INSERT INTO [dbo].[Order] (Price, CreateDate) values (@Price, @CreateDate)", order) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateOrder Error: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 編輯訂單
        /// </summary>
        public bool EditOrder(int orderID, int price)
        {
            try
            {
                return this._conn.Execute("Update [dbo].[Order] Set Price = @price WHERE OrderID = @orderID", new { orderID, price }) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"EditOrder Error: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        public bool DeleteOrder(int orderID)
        {
            try
            {
                return this._conn.Execute("DELETE [dbo].[Order] WHERE OrderID = @orderID", new { orderID }) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteOrder Error: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        public List<OrderEntity> GetAllOrder()
        {
            try
            {
                return this._conn.Query<OrderEntity>(@"SELECT OrderID, Price, CreateDate FROM [dbo].[Order]").ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllOrder Error: {ex}");
                throw;
            }
        }
    }
}
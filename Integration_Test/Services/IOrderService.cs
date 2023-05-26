using Integration_Test.Models;

namespace Integration_Test.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// 新增訂單
        /// </summary>
        bool CreateOrder();

        /// <summary>
        /// 編輯訂單
        /// </summary>
        bool EditOrder(int orderID, int price);

        /// <summary>
        /// 刪除訂單
        /// </summary>
        bool DeleteOrder(int orderID);

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        List<OrderEntity> GetAllOrder();
    }
}
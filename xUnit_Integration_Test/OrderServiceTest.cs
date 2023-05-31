using Integration_Test.Models;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using xUnit_Integration_Test.Extension;
using xUnit_Integration_Test.Setup;

namespace xUnit_Integration_Test
{
    [TestCaseOrderer("xUnit_Integration_Test.Extension.PriorityOrderer", "xUnit_Integration_Test")]
    public class OrderServiceTest : IClassFixture<IntegrationTestFactory<Program>>
    {
        private readonly IntegrationTestFactory<Program> _integrationTestFactory;
        private readonly HttpClient _httpClient;

        public OrderServiceTest(IntegrationTestFactory<Program> integrationTestFactory)
        {
            _integrationTestFactory = integrationTestFactory;
            _httpClient = _integrationTestFactory.CreateClient();
        }


        [Fact, TestPriority(0)]
        public async Task GetAllOrderTest_取得所有訂單_正流程()
        {
            _integrationTestFactory.ExecSqlCommand("INSERT INTO [Test].[dbo].[Order] (Price, CreateDate) VALUES (100, '2023-05-31');")
                                   .ExecSqlCommand("INSERT INTO [Test].[dbo].[Order] (Price, CreateDate) VALUES (200, '2023-05-31');")
                                   .ExecSqlCommand("INSERT INTO [Test].[dbo].[Order] (Price, CreateDate) VALUES (300, '2023-05-31');");

            // Arrange
            var res = await _httpClient.GetAsync("api/Order/GetAllOrder");
            string resDataStr = await res.Content.ReadAsStringAsync();
            var resData = System.Text.Json.JsonSerializer.Deserialize<List<OrderEntity>>(resDataStr);

            // Assert
            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(resData.Count == 3);
            Assert.IsType<List<OrderEntity>>(resData);
        }

        [Fact, TestPriority(1)]
        public async Task GetOrderTest_取得訂單_正流程()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/GetOrder?orderID=1");

            // Assert
            string resDataStr = await res.Content.ReadAsStringAsync();
            OrderEntity? resData = System.Text.Json.JsonSerializer.Deserialize<OrderEntity?>(resDataStr);

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.NotNull(resData);
            Assert.True(resData.Price == 100);
        }

        [Fact, TestPriority(2)]
        public async Task GetOrderTest_取得訂單_反流程_查不存在訂單()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/GetOrder?orderID=1000");

            // Assert
            string resDataStr = await res.Content.ReadAsStringAsync();

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.NoContent);
            Assert.True(string.IsNullOrEmpty(resDataStr));
        }

        [Fact, TestPriority(3)]
        public async Task CreateOrderTest_新增訂單_正流程()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/CreateOrder");

            // Assert
            string resDataStr = await res.Content.ReadAsStringAsync();

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(resDataStr == "Success");
        }

        [Fact, TestPriority(4)]
        public async Task EditOrderTest_編輯訂單_正流程()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/EditOrder?orderID=1&price=999");
            var checkOrderRes = await _httpClient.GetFromJsonAsync<OrderEntity?>("api/Order/GetOrder?orderID=1");

            // Assert
            string resDataStr = await res.Content.ReadAsStringAsync();

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(resDataStr == "Success");

            Assert.NotNull(checkOrderRes);
            Assert.True(checkOrderRes.Price == 999);
        }

        [Fact, TestPriority(5)]
        public async Task EditOrderTest_編輯訂單_反流程_修改不存在訂單()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/EditOrder?orderID=999&price=1");

            // Assert
            string resDataStr = await res.Content.ReadAsStringAsync();

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.True(resDataStr == "Error");
        }

        [Fact, TestPriority(6)]
        public async Task DeleteOrderTest_刪除訂單_正流程()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/DeleteOrder?orderID=2");
            var checkOrderRes = await _httpClient.GetStringAsync("api/Order/GetOrder?orderID=2");

            // Assert
            var status = res.StatusCode;
            string resDataStr = await res.Content.ReadAsStringAsync();

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(resDataStr == "Success");

            Assert.True(string.IsNullOrEmpty(checkOrderRes));
        }

        [Fact, TestPriority(7)]
        public async Task DeleteOrderTest_刪除訂單_反流程_刪除不存在訂單()
        {
            // Arrange
            var res = await _httpClient.GetAsync("api/Order/DeleteOrder?orderID=2");

            // Assert
            var status = res.StatusCode;
            string resDataStr = await res.Content.ReadAsStringAsync();

            Assert.NotNull(res);
            Assert.True(res.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.True(resDataStr == "Error");
        }
    }
}
using Integration_Test.Models;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using xUnit_Integration_Test.Setup;

namespace xUnit_Integration_Test
{
    public class OrderServiceTest : IClassFixture<IntegrationTestFactory<Program>>
    {
        private readonly IntegrationTestFactory<Program> _integrationTestFactory;

        public OrderServiceTest(IntegrationTestFactory<Program> integrationTestFactory)
        {
            _integrationTestFactory = integrationTestFactory;


        }

        [Fact]
        public async Task Test1()
        {
            var client = _integrationTestFactory.CreateClient();
            await _integrationTestFactory.ExecSqlCommandAsync("INSERT INTO [Test].[dbo].[Order] (Price, CreateDate) VALUES (100, GETDATE());"); // todo 仍不會產假資料

            // Arrange
            var getAllOrderReponse = await client.GetFromJsonAsync<List<OrderEntity>>("api/Order/GetAllOrder");

            // Assert
            Assert.NotNull(getAllOrderReponse);
            Assert.True(getAllOrderReponse.Count == 3);
            Assert.IsType<List<OrderEntity>>(getAllOrderReponse);
        }
    }
}
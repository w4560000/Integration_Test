using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Testcontainers.MsSql;
using Xunit;

namespace xUnit_Integration_Test.Setup
{
    public class IntegrationTestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
        where TProgram : class
    {
        private readonly MsSqlContainer _container;

        public IntegrationTestFactory()
        {
            var a = Path.GetDirectoryName(Directory.GetCurrentDirectory()); // AppContext.BaseDirectory;
            try
            {
                _container = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                                               .WithBindMount("C:\\Integration_Test\\MSSQL", "/MSSQL") // todo - 取絕對位址
                                               .Build();
            }
            catch (Exception ex)
            {
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //services.RemoveDbContext<TDbContext>();
                //services.AddDbContext<TDbContext>(options => { options.UseSqlServer(_container.ConnectionString); });
                //services.AddTransient<ArtworkCreator>();
                services.AddTransient<IDbConnection>(db => new SqlConnection(_container.GetConnectionString()));
            });
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            await _container.ExecAsync(new List<string>() { "bash /MSSQL/entrypoint.sh" }); // todo - entrypoint.sh 沒有執行 待查
        }

        public new async Task DisposeAsync() => await _container.DisposeAsync();
    }
}
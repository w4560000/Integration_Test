using Dapper;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Testcontainers.MsSql;

namespace xUnit_Integration_Test.Setup
{
    public class IntegrationTestFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
        where TProgram : class
    {
        private readonly MsSqlContainer _container;

        public IntegrationTestFactory()
        {
            try
            {
                _container = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                                               .WithPassword("Aa123456")
                                               .WithBindMount(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\MSSQL")), "/MSSQL")
                                               .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/bin/bash", "-c", "/MSSQL/entrypoint.sh"))
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
        }

        public new async Task DisposeAsync() => await _container.DisposeAsync();

        public IntegrationTestFactory<TProgram> ExecSqlCommand(string command)
        {
            using (var cn = new SqlConnection(_container.GetConnectionString()))
                cn.Execute(command);

            return this;
        }
    }
}
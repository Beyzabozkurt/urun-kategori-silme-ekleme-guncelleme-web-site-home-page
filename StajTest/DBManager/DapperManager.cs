using Dapper;
using StajTest.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using StajTest.Services.Dtos;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace StajTest.DBManager
{
    public class DapperManager : DapperRepository<StajTestDbContext>, ITransientDependency, IDapperManager
    {
        public DapperManager(IDbContextProvider<StajTestDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CategoryDto>> Listele()
        {
            var dbConnection = await GetDbConnectionAsync();
            var result = await dbConnection.QueryAsync<CategoryDto>(
                "SELECT CategoryId, CategoryName FROM Table_Category1",
                transaction: await GetDbTransactionAsync()
            );
            return result.AsList();
        }

        public async Task<List<ProductDto>> UrunListele(int CategoryId)
        {
            var dbConnection = await GetDbConnectionAsync();
            var result = await dbConnection.QueryAsync<ProductDto>(
                "SELECT ProductId, ProductName,ProductPrice,ProductColor,ProductBrand, CategoryId FROM Table_Product WHERE CategoryId = @CategoryId",
                new { CategoryId }
                ,
               
                transaction: await GetDbTransactionAsync()
            );
            return result.AsList();
        }
        public async Task<int> ProductKaydet(ProductDto product)
        {
            var dbConnection = await GetDbConnectionAsync();
            var result = await dbConnection.ExecuteAsync(
                "UPDATE Table_Product SET ProductName = @ProductName, CategoryId = @CategoryId, ProductColor = @ProductColor, ProductPrice = @ProductPrice, ProductBrand = @ProductBrand WHERE ProductId = @ProductId",
        new
        {
            product.ProductName,
            product.CategoryId,
            product.ProductColor,
            product.ProductPrice,
            product.ProductBrand,
            product.ProductId
        },
                transaction: await GetDbTransactionAsync()
            );
            return result;
        }


        public async Task<LoginDto> Login(LoginInput input)
        {
            var dbConnection = await GetDbConnectionAsync();
            var result = await dbConnection.QueryFirstAsync<LoginDto>(
                "SELECT id, username, password, email FROM   Table_Users WHERE (username = @Username) AND (password = @Password)",
                new {input.Username, input.Password},
                transaction: await GetDbTransactionAsync()
            );
            return result;
        
        }

    }
}

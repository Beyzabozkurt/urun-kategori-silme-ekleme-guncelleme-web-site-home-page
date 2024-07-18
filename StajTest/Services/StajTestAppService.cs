using Microsoft.AspNetCore.Mvc;
using StajTest.DBManager;
using StajTest.Localization;
using Volo.Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StajTest.Services
{
    [Route("/api")]
    public class StajTestAppService : ApplicationService
    {
        private readonly IDapperManager _dapperManager;

        public StajTestAppService(IDapperManager dapperManager)
        {
            _dapperManager = dapperManager;
            LocalizationResource = typeof(StajTestResource);
        }

        [HttpGet]
        [Route("Listele")]
        public async Task<List<CategoryDto>> Listele()
        {
            var categories = await _dapperManager.Listele();

            return categories;
        }


        [HttpGet]
        [Route("UrunListele/{CategoryId}")]
        public async Task<List<ProductDto>> UrunListele(int CategoryId)
        {

            var products = await _dapperManager.UrunListele(CategoryId);


            return products;
        }


        [HttpPost]
        [Route("ProductKaydet")]
        public async Task<IActionResult> ProductKaydet([FromBody] ProductDto product)
        {

            var result = await _dapperManager.ProductKaydet(product);


            return new OkObjectResult(result);
              
        }
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            
             await _dapperManager.Logout();

            return new OkResult();
        }

    }
}

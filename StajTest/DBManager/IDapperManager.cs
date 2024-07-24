using StajTest.Services.Dtos;
using System.Threading.Tasks;

namespace StajTest.DBManager
{
    public interface IDapperManager
    {
        Task<List<CategoryDto>> Listele();
        Task<int> ProductKaydet(ProductDto product);//değişiklik oluştu burada
        Task<List<ProductDto>> UrunListele(int categoryId);

        Task<LoginDto> Login(LoginInput input);
        //Task Logout();
    }

    public class LoginDto
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; } 
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductDto> Urunler { get; set; }
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string ProductColor { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductBrand { get; set; }
    }

}


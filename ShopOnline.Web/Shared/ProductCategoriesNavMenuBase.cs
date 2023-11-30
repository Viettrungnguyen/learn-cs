using Microsoft.AspNetCore.Components;
using Shoponline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Shared
{
    public class ProductCategoriesNavMenuBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductCategoryDto> ProductCategoryDtos { get; set; }
        public string Errormessage { get; set; }
        protected override async void OnInitialized()
        {
            try
            {
                ProductCategoryDtos = await ProductService.GetProductCategories();
            }
            catch (Exception ex)
            {
                Errormessage = ex.Message;
            }
        }
    }
}

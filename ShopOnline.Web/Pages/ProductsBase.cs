using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public string ErrorMessage { get; set; }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupProductDtos)
        {
            return groupProductDtos.FirstOrDefault(pg => pg.CategoryId == groupProductDtos.Key).CategoryName;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ClearLocalStorage();

                Products = await ManageProductsLocalStorageService.GetCollection();

                var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection(HardCoded.UserId);

                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task ClearLocalStorage()
        {
            await ManageProductsLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();
        }
    }
}
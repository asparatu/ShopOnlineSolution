using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<List<CartItemDto>> GetCollection(int userId);

        Task SaveCollection(List<CartItemDto> cartItemDto);

        Task RemoveCollection();
    }
}
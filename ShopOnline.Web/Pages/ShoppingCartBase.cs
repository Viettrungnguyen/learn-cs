using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shoponline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
	public class ShoppingCartBase: ComponentBase
	{
		[Inject]
		public IJSRuntime Js {  get; set; }

		[Inject]
		public IShoppingCartService ShoppingCartService { get; set; }

		public List<CartItemDto> ShoppingCartItems { get; set; }

		public string ErrorMessage { get; set; }

		public string TotalPrice { get; set; }
		public int TotalQuantity { get; set; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
				CartChanged();
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}

		}
		protected async Task DeleteCartItem_Click(int id)
		{
			var cartItemDto = await ShoppingCartService.DeleteItem(id);

			RemoveCartItem(id);
			CartChanged();
		}

		protected async Task UpdateQtyCartItem_Click(int id, int qty)
		{
			try
			{
				if (qty > 0)
				{
					var updateItemDto = new CartItemQtyUpdateDto
					{
						CartitemId = id,
						Qty = qty
					};

					var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);

					UpdateItemTotalPrice(returnedUpdateItemDto);
					CartChanged();
					await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);
				}
				else
				{
					var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
					if (item != null)
					{
						item.Qty = qty;
						item.Totalprice = item.Price;
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}

		protected async Task UpdateQty_Input(int id)
		{
			await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, true);
		}

		private void UpdateItemTotalPrice(CartItemDto cartItemDto)
		{
			var item = GetCartItem(cartItemDto.Id);

			if (item != null)
			{
				item.Totalprice = cartItemDto.Price * cartItemDto.Qty;
			}
		}

		private void CalculateCartSummaryTotals()
		{
			SetTotalPrice();
			SetTotalQuantity();
		}

		private void  SetTotalPrice()
		{
			TotalPrice = this.ShoppingCartItems.Sum(i => i.Totalprice).ToString("C");
		}

		private void SetTotalQuantity()
		{
			TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
		}

		private CartItemDto GetCartItem(int id)
		{
			return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
		}

		private void RemoveCartItem(int id)
		{
			var cartItemDto = GetCartItem(id);

			ShoppingCartItems.Remove(cartItemDto);
		}

		private void CartChanged()
		{
			CalculateCartSummaryTotals();
			ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
		}
	}
}

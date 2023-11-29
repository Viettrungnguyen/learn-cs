using Microsoft.AspNetCore.Components;
using Shoponline.Models.Dtos;

namespace ShopOnline.Web.Pages
{
	public class DisplayProductsBase:ComponentBase
	{
		[Parameter]
		public IEnumerable<ProductDto> Products { get; set; }

	}
}

﻿using Shoponline.Models.Dtos;
using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Extensions
{
	public static class DtoConversions
	{
		public static IEnumerable<ProductDto>  ConvertToDto(this IEnumerable<Product> products, 
															IEnumerable<ProductCategory> productCategories)
		{
			return (from product in products
					join productCategory in productCategories
					on product.CategoryId equals productCategory.Id
					select new ProductDto
					{
						Id = product.Id,
						Name = product.Name,
						Description = product.Description,
						ImageURL = product.ImageURL,
						Price = product.Price,
						Qty = product.Qty,
						CategoryId = product.CategoryId,
						CategoryName = productCategory.Name
					}).ToList();
		}

		public static ProductDto ConvertToDto(this Product product,
												   ProductCategory productCategory)
		{
			return new ProductDto
					{
						Id = product.Id,
						Name = product.Name,
						Description = product.Description,
						ImageURL = product.ImageURL,
						Price = product.Price,
						Qty = product.Qty,
						CategoryId = product.CategoryId,
						CategoryName = productCategory.Name
					};
		}

		public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
															IEnumerable<Product> products)
		{
			return (from cartItem in cartItems
					join product in products
					on cartItem.ProductId equals product.Id
					select new CartItemDto
					{
						Id= cartItem.Id,
						ProductId = product.Id,
						ProductName = product.Name,
						ProductDescription = product.Description,
						ProductImageURL = product.ImageURL,
						Price= product.Price,
						CartId= cartItem.CartId,
						Qty= cartItem.Qty,
						Totalprice = product.Price * cartItem.Qty,
					}).ToList();
		}

		public static CartItemDto ConvertToDto(this CartItem cartItem,
												Product product)
		{
			return new CartItemDto
					{
						Id = cartItem.Id,
						ProductId = product.Id,
						ProductName = product.Name,
						ProductDescription = product.Description,
						ProductImageURL = product.ImageURL,
						Price = product.Price,
						CartId = cartItem.CartId,
						Qty = cartItem.Qty,
						Totalprice = product.Price * cartItem.Qty,
					};
		}
	}
}

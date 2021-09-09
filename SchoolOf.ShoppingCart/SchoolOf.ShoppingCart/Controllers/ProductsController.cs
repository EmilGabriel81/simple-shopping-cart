using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolOf.Dtos;
using System;
using Microsoft.AspNetCore.Http;

namespace SchoolOf.ShoppingCart.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private List<ProductDto> myList = new List<ProductDto>();

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
		public async Task<IActionResult>GetProducts()
        {
			var myListOfProducts = new List<ProductDto>();
			

			myListOfProducts.Add(new ProductDto
			{
				Category = "test category",
				Description = "test description",
				Id = 10,
				Image = "no Image yet",
				Name = "test product",
				Price = 100m

			}) ;
			//myList.AddRange(myListOfProducts);
			return Ok(myListOfProducts);
        }

		[HttpGet("{id:int}")]
		[ProducesResponseType(typeof(ProductDto), 200)]
		public async Task<ActionResult<ProductDto>> GetProductById(int id)
		{
			myList.Add(new ProductDto
			{
				Category = "Vehicle",
				Description = "Testing vehicle",
				Id = 15,
				Image = "No Image Available",
				Name = "Dacia",
				Price = 500m

			});// This is harcoded but we can use a repository to retrieve the method with an Id as an argument

			try
			{
				ProductDto result =  myList.Find(x => x.Id == id);
				if (result == null) return NotFound();
				return Ok(result);
			}
			catch(Exception )
			{ 
				return StatusCode(StatusCodes.Status500InternalServerError,
					"Error retrieving data from the database");
			}
		}
	}


}




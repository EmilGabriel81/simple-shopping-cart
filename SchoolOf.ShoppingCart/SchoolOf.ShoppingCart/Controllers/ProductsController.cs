using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using SchoolOf.Dtos;
using SchoolOf.Data.Abstraction;
using SchoolOf.Data.Models;

namespace SchoolOf.ShoppingCart.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductsController(IUnitOfWork unitOfWork)
		{
			this._unitOfWork = unitOfWork;
		}


		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
		public async Task<IActionResult>GetProducts()
        {
			var myListOfProducts = new List<ProductDto>();
			var productsFromDb = this._unitOfWork.GetRepository<Product>().Find(product => !product.IsDeleted);
			foreach (var p in productsFromDb)
			{
				myListOfProducts.Add(new ProductDto
				{
					Category = p.Category,
					Description = p.Description,
					Id = p.Id,
					Image = p.Image,
					Name = p.Name,
					Price = p.Price
				});
			}

			return Ok(myListOfProducts);
		}


		//Implementati o functionalitate de paginare pe GET Products –
		//asta presupune ca metoda din controller sa primeasca 2 parametri:
		//pageNumber, pageSize si in functie de valoarea lor


		//-----------------------------------------------------------------------------------

		private List<ProductDto> myList = new List<ProductDto>();

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




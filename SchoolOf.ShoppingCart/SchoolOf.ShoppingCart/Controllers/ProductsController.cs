using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using SchoolOf.Dtos;
using SchoolOf.Data.Abstraction;
using SchoolOf.Data.Models;
using AutoMapper;
using SchoolOf.Common.Exceptions;

namespace SchoolOf.ShoppingCart.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			this._unitOfWork = unitOfWork;
			_mapper = mapper;
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

		//-----------------------------------------------------------------------------------

		//Implementati o functionalitate de paginare pe GET Products –
		//asta presupune ca metoda din controller sa primeasca 2 parametri:
		//pageNumber, pageSize si in functie de valoarea lor

		[HttpGet]
		[Route("{pageNumber}/{pageSize}")]
		[ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
		public async Task<IActionResult> GetPaginatedProducts(int pageNumber = 1, int pageSize = 10)
		{
			if (pageNumber < 1)// we have to substract 1
			{
				throw new InvalidParameterException("Invalid page number.");
			}
			if (pageSize < 1)
			{
				throw new InvalidParameterException("Invalid number of items.");
			}
			var productsFromDb = this._unitOfWork.GetRepository<Product>().Find(product => !product.IsDeleted, (pageNumber - 1) * pageSize, pageSize);

			var myListOfProducts = _mapper.Map<List<ProductDto>>(productsFromDb);

			return Ok(myListOfProducts);
		}

		//-----------------------------------------------------------------------------------

		/*
		[HttpGet]
		[ApiExplorerSettings(IgnoreApi = true)]
		[ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
		public async Task<IActionResult> GetOrders()
		{
			throw new System.Exception();
			var productsFromDb = this._unitOfWork.GetRepository<Order>().Find(order => !order.IsDeleted);
            var myListOfOrders = _mapper.Map<List<OrderDto>>(productsFromDb);

			return Ok(myListOfOrders);
		}
	    */

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




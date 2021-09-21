using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolOf.Common.Exceptions;
using SchoolOf.Data.Abstraction;
using SchoolOf.Data.Models;
using SchoolOf.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolOf.ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CartProductDto> _cartValidator;
        public CartsController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CartProductDto> cartValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cartValidator = cartValidator;
        }
        [HttpPost]
        [ProducesResponseType( typeof (CartProductDto), 200)]
        public async Task<IActionResult> AddProductToCart([FromBody] CartProductDto cartProduct)
        {
            var validationResult = await _cartValidator.ValidateAsync(cartProduct);
            if (!validationResult.IsValid)
            {
                throw new InternalValidationException(validationResult.Errors.Select(validationError => validationError.ErrorMessage).ToList());
            }
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var productRepo = _unitOfWork.GetRepository<Product>();

            Cart cart = null;
            cart = cartRepo.Find(x => x.Id == cartProduct.CartId, nameof(Cart.Products)).FirstOrDefault();

            if (cart == null)
            {
                var product = await productRepo.GetByIdAsync(cartProduct.ProductId);

                cart = new Cart()
                {
                    Status = Common.Enums.CartStatus.Created,
                    Products = new List<Product> {product }

                };
                cartRepo.Add(cart);
            }
            else
            {
                var product = await productRepo.GetByIdAsync(cartProduct.ProductId);
                cart.Products.Add(product);
                cartRepo.Update(cart);
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok (_mapper.Map<CartDto>(cart));
        }


        //Endpoint nou ‘/carts/id’ – trebuie sa expuna detaliile unui cart + produsele
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CartDto), 200)]
        public async Task<ActionResult> GetCartById(int id)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            Cart cart = null;
            cart = cartRepo.Find(x => x.Id == id, nameof(Cart.Products)).FirstOrDefault();

            if (cart == null || cart.IsDeleted)
            {
                throw new InvalidParameterException("Not a valid id");
            }       
            return Ok(_mapper.Map<CartDto>(cart));
        }

        //Endpoint nou ‘/carts’ – trebuie sa stearga un produs dintr-un cart
        [HttpDelete]
        [ProducesResponseType(typeof(CartProductDto), 200)]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] CartProductDto cartProduct)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var productRepo = _unitOfWork.GetRepository<Product>();
            Cart cart = null;

            cart = cartRepo.Find(x => x.Id == cartProduct.CartId, nameof(Cart.Products)).FirstOrDefault();
            if (cart == null)
            {
                throw new InvalidParameterException("Not a valid cart");
            }
            else
            {
                var product = await productRepo.GetByIdAsync(cartProduct.ProductId);
                if (!cart.Products.Contains(product))
                {
                    throw new InvalidParameterException("Not a valid product");
                }
                else
                {
                    cart.Products.Remove(product);
                    cartRepo.Update(cart);
                }
               
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok(_mapper.Map<CartDto>(cart));
        }

        /*
         *  work in progress
         * 
         * Endpoint nou ‘/orders’ – trebuie sa adauge un order pentru un anumit cart
          Cartul trebuie sa treaca in statusul Completed
          Orderul creat trebuie returnat
         
        */

        /*
        [HttpPost]
        [ProducesResponseType(typeof(CartOrderDto), 200)]
        public async Task<IActionResult> AddOrders([FromBody] CartOrderDto cartOrder)
        {
            var cartRepo = _unitOfWork.GetRepository<Cart>();
            var orderRepo = _unitOfWork.GetRepository<Order>();
            Cart cart = null;
            Order order = null;
            cart = cartRepo.Find(x => x.Id == cartOrder.CartId, nameof(Cart.Products)).FirstOrDefault();
            if (cart == null)
            {
                throw new InvalidParameterException("Not a valid cart");
            }
            else
            {
                order = await orderRepo.GetByIdAsync(cartOrder.OrderId);
                cart.Status = Common.Enums.CartStatus.Completed;
                order.CartId = cart.Id;
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(_mapper.Map<OrderDto>(order));
        }
        */
        //-------------------------------------------------------------------------------------------------------

    }
}

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dto;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.mapper = mapper;
            this.orderService = orderService;
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPincipal();

            var address = mapper.Map<AddressDto, Address>(orderDto.ShippToAddress);

            var order = await this.orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId,orderDto.BasketId,address);


            if(order == null){
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            }
            
            return Ok(order);
        }
    }
}
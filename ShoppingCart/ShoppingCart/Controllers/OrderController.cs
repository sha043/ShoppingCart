using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Entities;
using ShoppingCart.Queries;
using ShoppingCart.Commands;
using ShoppingCart.Request;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("GetOrderDetailsByIdAsync/{orderId:int}")]
        public async Task<IActionResult> GetOrderDetailsByIdAsync(int orderId)
        {
            try
            {
                var orderDetails = await _mediator.Send(new GetOrderDetailsByIdQuery { OrderId = orderId });
                if (orderDetails == null)
                {
                    return NotFound();
                }

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetAllOrdersAsync/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var orderDetailsList = await _mediator.Send(new GetAllOrderDetailsQuery { PageNumber = pageNumber, PageSize = pageSize });
                if (orderDetailsList == null)
                {
                    return NotFound();
                }

                return Ok(orderDetailsList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddNewOrderDetailsAsync([FromBody]AddNewOrderCommand request)
        {
            try
            {
                var newOrderStatus = await _mediator.Send(new AddNewOrderCommand
                {
                    CustomerId = request.CustomerId,
                    OrderDetails = request.OrderDetails,
                    ShippingAddress = request.ShippingAddress
                });
                if (!newOrderStatus.Item2.IsSuccessful)
                {
                    return NotFound(newOrderStatus.Item2.ResponseMessage);
                }
                else
                {
                    foreach (var item in request.OrderDetails)
                    {
                        var orderitemsStatus = await _mediator.Send(new AddNewOrderDetailsCommand
                        {
                            OrderId = newOrderStatus.Item1.Value,
                            ProductId = item.ProductId,
                            ProductQuantity = item.Quantity
                        });
                        if (!orderitemsStatus.Item2.IsSuccessful)
                        {
                            return NotFound(orderitemsStatus.Item2.ResponseMessage);
                        }
                    }
                }
                return Ok(newOrderStatus.Item1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpDelete]
        [Route("Cancel")]
        public async Task<IActionResult> CancelOrderAsync(int orderId)
        {
            try
            {
                var orderDetailsList = await _mediator.Send(new GetAllOrderDetailsByIdQuery { OrderId = orderId });
                if (orderDetailsList != null && orderDetailsList.Count > 0)
                {
                    foreach (var ordertDetails in orderDetailsList)
                    {
                        var deletedOrderItemStatus = await _mediator.Send(new DeleteOrderDetailsCommand { OrderDetailsId = ordertDetails.OrderDetailsId });
                        if (!deletedOrderItemStatus.Item2.IsSuccessful)
                        {
                            return NotFound(deletedOrderItemStatus.Item2.ResponseMessage);
                        }
                    }
                }
                var deletedOrderStatus = await _mediator.Send(new DeleteOrderCommand { OrderId = orderId });
                if (!deletedOrderStatus.Item2.IsSuccessful)
                {
                    return NotFound(deletedOrderStatus.Item2.ResponseMessage);
                }
                return Ok(deletedOrderStatus.Item1);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost]
        [Route("UpdateShippingAddress")]
        public async Task<IActionResult> UpdateOrderShippingAddress([FromBody]UpdateOrderShippingAddressRequest request)
        {
            try
            {
                var response = await _mediator.Send(new UpdateShippingAddressCommand { OrderId = request.OrderId, ShippingAddress = request.ShippingAddress });
                if (response.IsSuccessful)
                {
                    return Ok(response.ResponseMessage);
                }
                else
                {
                    return NotFound(response.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            try
            {
                var response = await _mediator.Send(new UpdateOrderCommand { OrderId = request.OrderId, orderDetails = request.orderDetails });
                if (response.IsSuccessful)
                {
                    return Ok(response.ResponseMessage);
                }
                else
                {
                    return NotFound(response.ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }
    }
}


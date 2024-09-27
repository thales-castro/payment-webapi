using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentWebApi.Database.Repositories;
using PaymentWebApi.Dtos.MercadoPago;
using PaymentWebApi.Entities;
using PaymentWebApi.Entities.MercadoPagoEntities;
using PaymentWebApi.Mappers;
using PaymentWebApi.MercadoPagoServices;
using System.Text.Json;

namespace PaymentWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AfePaymentController : ControllerBase
{
    private readonly ILogger<AfePaymentController> _logger;
    private readonly IMerchantOrderService _merchantOrderService;
    private readonly IOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentDeviceRepository _paymentDeviceRepository;
    private readonly IMapper _mapper;
    private readonly IMerchantOrderRepository _merchantOrderRepository;
    private readonly IMerchantOrderPaymentRepository _merchantOrderPaymentRepository;

    public AfePaymentController(ILogger<AfePaymentController> logger,
        IMerchantOrderService merchantOrderService,
        IOrderRepository orderRepository,
        IPaymentDeviceRepository paymentDeviceRepository,
        IOrderService orderService,
        IMapper mapper,
        IMerchantOrderRepository merchantOrderRepository,
        IMerchantOrderPaymentRepository merchantOrderPaymentRepository)
    {
        _logger = logger;
        _merchantOrderService = merchantOrderService;
        _orderService = orderService;
        _orderRepository = orderRepository;
        _paymentDeviceRepository = paymentDeviceRepository;
        _mapper = mapper;
        _merchantOrderRepository = merchantOrderRepository;
        _merchantOrderPaymentRepository = merchantOrderPaymentRepository;
    }

    [HttpGet]
    public async Task<bool> GetPaymentState(string payment_device_mac)
    {
        //TODO: Talvez uma resposta mais elaborada aqui.
        PaymentDevice device = await _paymentDeviceRepository.GetDeviceByMacAddressAsync(payment_device_mac);
        if (device == null || device.MacAddress == null || 
            device.StoreExternalId == null || device.CashierExternalId == null) 
        {
            return false;
        }

        //O Paymentdevice contém os campos necessários para os GET na api do mercado pago.
        //Procurar no banco de dados a última order criada
        Order currentOrder = await _orderRepository.GetLastOrder(device.MacAddress);
       
        if(currentOrder == null ||
           currentOrder.status == OrderStatus.EXPIRED ||
           currentOrder.status == OrderStatus.RETURNED)
        {
            //A última ordem já completou o seu ciclo ou não existe, então criar uma nova.
            Order? defaultOrder = default(Order);
            await _orderRepository.CreateNewDefaultOrderAsync(device.MacAddress, out defaultOrder);
            OrderDto defaultOrderDto = _mapper.Map<OrderDto>(defaultOrder);
            await _orderService.CreateNewOrderAsync(device.UserId, 
                device.StoreExternalId, 
                device.CashierExternalId, 
                defaultOrderDto);
            return false;
        }
        else if (currentOrder.status == OrderStatus.PAID)
        {
            //Atualizar a ordem corrent, retornar que o valor foi pago
            currentOrder.status = OrderStatus.RETURNED;
            _orderRepository.Update(currentOrder);
            //Esperar o próximo ciclo para criar uma nova order
            return true;
        }

        //A ordem corrent está válida, pagamento não detectado
        //Verificar na api do mercado pago se expirou, atualizar o estado e esperar próximo request.
        Order? currentApiOrder = await _orderService.GetCurrentOrderAsync(device.UserId, device.CashierExternalId);
        if (currentApiOrder == null)
        {
            currentOrder.status = OrderStatus.EXPIRED;
            _orderRepository.Update(currentOrder);
        }
        
        return false;
    }

    [HttpPost]
    public async Task<IActionResult> NotifyPayment([FromBody] JsonElement notify_data)
    {
        _logger.LogInformation(notify_data.ToString());
        try
        {
           JsonElement topic_value = default(JsonElement);
            if (notify_data.TryGetProperty("topic", out topic_value))
            {
                TopicMessage? topic_message =
                    JsonSerializer.Deserialize<TopicMessage>(notify_data);
                if (topic_message != null)
                {
                    if (topic_message.topic == "merchant_order")
                    {
                        _logger.LogInformation("An payment has started");
                        //Iniciou o pagamento, consultar a api do mercado pago para trazer as informações.
                        MerchantOrderDto? merchantOrderDto = await _merchantOrderService.GetMerchantOrderAsync(topic_message.resource);
                        if (merchantOrderDto != null)
                        {
                            var entity = MerchantOrderMapper.GetEntityFromDto(merchantOrderDto);
                            _merchantOrderRepository.Create(entity);
                            //Será que vai usar pra alguma coisa?
                            _logger.LogInformation("Save to database");
                        }
                    }
                    else if (topic_message.topic == "payment")
                    {
                        _logger.LogInformation("An payment has finished");
                        //Chegou o pagamento
                        MerchantOrderPaymentDto? merchantOrderPaymentDto = await _merchantOrderService.GetMerchantOrderPaymentAsync(topic_message.resource);
                        if(merchantOrderPaymentDto != null)
                        {
                            //Atualizar o pagamento da order
                            Order order = await _orderRepository.GetOrderByExternalReferenceAsync(merchantOrderPaymentDto.external_reference);
                            if (order != null)
                            {
                                order.status = OrderStatus.PAID;
                                _orderRepository.Update(order);
                            }
                            //TODO: O que fazer se não achar a ordem? Deve achar sempre.
                            var entity = MerchantOrderPaymentMapper.GetEntityFromDto(merchantOrderPaymentDto);
                            _merchantOrderPaymentRepository.Create(entity);
                            _logger.LogInformation("Save to database");
                        }
                    }
                }
            }
        }
        catch (Exception ex) 
        {
            _logger.LogInformation(ex.Message);
        }
        
        return Ok();            
    }
}

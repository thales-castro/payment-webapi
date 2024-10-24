﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentWebApi.Database.Repositories;
using PaymentWebApi.Dtos.MercadoPago;
using PaymentWebApi.Entities;
using PaymentWebApi.Entities.MercadoPagoEntities;
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
        //Usando o MAC encontrar os dados do dispositivo que está fazendo o request.
        PaymentDevice device = await _paymentDeviceRepository.GetDeviceByMacAddressAsync(payment_device_mac);
        if (device == null || device.MacAddress == null || 
            device.StoreExternalId == null || device.CashierExternalId == null ||
            device.Token == null) 
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
                device.Token,
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
        Order? currentApiOrder = await _orderService.GetCurrentOrderAsync(
            device.UserId, device.CashierExternalId, device.Token);
        if (currentApiOrder == null)
        {
            /*Se a ordem corrent estiver nula pode ter acontecido duas situaçoes:
             * 1 - A ordem expirou e não foi paga
             * 2 - A ordem foi paga, mas o mercado pago ainda não informou o servidor sobre o pagamento.
             * Necessário consultar o estado da ordem em API específica do mercado pago.
            */
            bool isPaid = await _orderService.CheckIfOrderPaidAsync(currentOrder.external_reference);
            if (isPaid)
            {
                currentOrder.status = OrderStatus.RETURNED;
                _orderRepository.Update(currentOrder);
                //Esperar o próximo ciclo para criar uma nova order
                return true;
            }
            else
            {
                currentOrder.status = OrderStatus.EXPIRED;
                _orderRepository.Update(currentOrder);
            }
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
                        //TODO: Essa API foi descontinuada, verificar o que fazer
                        //Iniciou o pagamento, consultar a api do mercado pago para trazer as informações.
                        /*
                        MerchantOrderDto? merchantOrderDto = await _merchantOrderService.GetMerchantOrderAsync(topic_message.resource);
                        if (merchantOrderDto != null)
                        {
                            var entity = MerchantOrderMapper.GetEntityFromDto(merchantOrderDto);
                            _merchantOrderRepository.Create(entity);
                            //Será que vai usar pra alguma coisa?
                            _logger.LogInformation("Merchant order saved to database");
                        }
                        */
                    }
                    else if (topic_message.topic == "payment")
                    {                        
                        _logger.LogInformation("A payment has finished");                       
                    }
                }
            }
            else
            {
                PaymentNotificationDto? paymentNotificationDto =
                    JsonSerializer.Deserialize<PaymentNotificationDto>(notify_data);
                /**
                 * Nessa classe vem o user_id e o id para consultar o pagamento
                 * dentro do data, pode ser usado para vários clientes
                 */
                if (paymentNotificationDto != null)
                {
                    //Recuperar o token de acordo com o usuário
                    PaymentDevice paymentDevice = await
                        _paymentDeviceRepository.GetDeviceByUserId(long.Parse(paymentNotificationDto.user_id));
                    if (paymentDevice != null && paymentDevice.Token != null)
                    {
                        PaymentInfoDto? paymentInfoDto = await 
                            _merchantOrderService.GetMerchantOrderPaymentAsync(paymentNotificationDto.data.id,
                            paymentDevice.Token);
                        if (paymentInfoDto != null)
                        {

                            //Atualizar o pagamento da order
                            Order order = await _orderRepository.GetOrderByExternalReferenceAsync(paymentInfoDto.external_reference);
                            if (order != null)
                            {
                                order.status = OrderStatus.PAID;
                                _orderRepository.Update(order);
                            }
                            //TODO: Salvar nova entidade no banco
                            _logger.LogInformation("Save to database");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Could not get Payment device using user_id:{paymentNotificationDto.user_id}");
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

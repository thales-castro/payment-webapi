using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Dtos;

public class PaymentDto
{
    public string Id { get; set; }
    public string MacAddress { get; set; }
    public string Company { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusDescription { get; set; }
    public string Item { get; set; }
    public double Value { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }

    public PaymentDto(string id, string macAddress, string company, OrderStatus status, 
        string statusDescription, string item, double value, string createdAt, string updatedAt)
    {
        Id = id;
        MacAddress = macAddress;
        Company = company;
        Status = status;
        StatusDescription = statusDescription;
        Item = item;
        Value = value;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}

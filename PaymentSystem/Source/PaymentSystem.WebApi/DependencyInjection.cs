using PaymentSystem.WebApi.Database.ConnectionStringBuilder;
using PaymentSystem.WebApi.Database.Repositories;
using PaymentSystem.WebApi.MercadoPagoServices;
using PaymentSystem.WebApi.Services.Auth;
using PaymentSystem.WebApi.Services.Companies;
using PaymentSystem.WebApi.Services.PaymentDevices;
using PaymentSystem.WebApi.Services.Users;

namespace PaymentSystem.WebApi;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IConnectionStringBuilderService, ConnectionStringBuilderService>();
        services.AddScoped<IPaymentDeviceRepository, PaymentDeviceRepository>();
        services.AddScoped<IMerchantOrderRepository, MerchantOrderRepository>();
        services.AddScoped<IMerchantOrderPaymentRepository, MerchantOrderPaymentRepository>();
        services.AddScoped<IPaymentInfoRepository, PaymentInfoRepository>();
        services.AddScoped<IPaymentDeviceRepository, PaymentDeviceRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IMerchantOrderService, MerchantOrderService>();
        services.AddScoped<IPaymentDeviceService, PaymentDeviceService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<ICashierService, CashierService>();
    }
}

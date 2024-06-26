﻿using DiscountManagement.Application;
using DiscountManagement.Application.Contracts.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountManagement.Configuration;

public class DiscountManagementBoostrapper
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddTransient<ICustomerDiscountApplication, CustomerDiscountApplication>();
        services.AddTransient<ICustomerDiscountRepository, CustomerDiscountRepository>();

        services.AddDbContext<DiscountContext>(x => x.UseSqlServer(connectionString));
    }
}
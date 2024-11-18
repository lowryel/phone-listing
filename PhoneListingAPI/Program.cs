using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MobilePhoneListing.DBContext;
using PhoneListingAPI.Configuration;
// using PhoneListingAPI.Models;
// using PhoneListingAPI.Services;
// using PhoneListingAPI.Services.ErrorService;
using PhoneListingAPI.Services.ListingServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(opts => opts.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
     .AddJsonOptions(opts =>
     {
        // this to avoid related tables cycle
         opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
         opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
     });

builder.Logging.AddConsole();

// Register the various component services
builder.Services.RegisterService();

// Access the loaded environment variable
// var myEnvVar = Environment.GetEnvironmentVariable("DB_STRING");

builder.Services.AddDbContext<MobilePhoneDbContext>(
        options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));


var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});


// app.UseHttpsRedirection();

app.Run();


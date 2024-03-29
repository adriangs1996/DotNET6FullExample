
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Implementation;
using Services.Implementation.Hubs;
using TestClientBlazor.Service;

var builder = WebApplication.CreateBuilder(args);

// Configure server environment variables
var serverApiEnvironmentAdress = Environment.GetEnvironmentVariable("SERVER_API");
if (string.IsNullOrEmpty(serverApiEnvironmentAdress))
    serverApiEnvironmentAdress = builder.Configuration.GetValue<string>("serverApi");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(serverApiEnvironmentAdress) });

builder.Services.AddHttpClient<RestService>(client =>
    client.BaseAddress = new Uri(serverApiEnvironmentAdress));

builder.Services.AddScoped<IRestService, RestService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddHostedService<AzureSubscriberService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<NotificationHub>("/notifications");
app.MapFallbackToPage("/_Host");

app.Run();

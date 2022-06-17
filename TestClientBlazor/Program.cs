
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Services;
using Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Implementation;
using TestClientBlazor.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("serverApi")) });
builder.Services.AddHttpClient<RestService>(client =>
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("serverApi")));
builder.Services.AddScoped<IRestService, RestService>();

// Configure the Connection Hub
builder.Services.AddSingleton(sp =>
{
    return new HubConnectionBuilder()
        .WithUrl(new Uri($"{builder.Configuration.GetValue<string>("serverApi")}/notifications"))
        .WithAutomaticReconnect()
        .Build();
});


builder.Services.AddScoped<ToastService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

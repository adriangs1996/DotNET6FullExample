using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services;
using Services.Contracts;
using Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("serverApi")) });
builder.Services.AddHttpClient<RestService>(client =>
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("serverApi")));
builder.Services.AddScoped<IRestService, RestService>();

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

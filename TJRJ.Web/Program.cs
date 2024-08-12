using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TJRJ.Entities;
using TJRJ.Mapper;
using TJRJ.Repository;
using TJRJ.Services;
using TJRJ.ViewModels;
using TJRJ.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql("Server=localhost;Port=3306;Database=tjrj;Uid=root;Pwd=123456", new MySqlServerVersion(new Version(8, 0, 26))));

builder.Services.AddScoped(typeof(Repository<Livro>));
builder.Services.AddScoped(typeof(Repository<Assunto>));
builder.Services.AddScoped(typeof(Repository<Autor>));
builder.Services.AddScoped<BaseService<Assunto>>();
builder.Services.AddScoped<BaseService<Autor>>();
builder.Services.AddScoped<BaseService<Livro>>();
builder.Services.AddScoped<BaseService<LivroAutor>>();
builder.Services.AddScoped<BaseService<LivroAssunto>>();
builder.Services.AddScoped<BaseService<RelatorioViewModel>>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddMvc();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ConfigurationMapping());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

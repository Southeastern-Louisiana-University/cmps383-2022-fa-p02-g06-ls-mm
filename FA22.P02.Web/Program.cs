using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();



app.MapGet("/api/products", ([FromServices] ProductRepository repo) =>
 {
     return repo.GetAll();
 });

app.MapGet("/api/customers/{id}", ([FromServices] ProductRepository repo, Guid id) =>
{
    var product = repo.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound(404);
});

app.MapPost("/api/customers", ([FromServices] ProductRepository repo, Product product) =>
{
    repo.Create(product);
    return Results.Created($"api/product/{product.Id}", product);
});

app.MapPut("/customers/{id}", ([FromServices] ProductRepository repo, Guid id, Product updatedProduct) =>
{
    var product = repo.GetById(id);
    if (product is null)
    {
        return Results.NotFound();
    }

    repo.Update(updatedProduct);
    return Results.Ok(updatedProduct);
});

app.MapDelete("/customers/{id}", ([FromServices] ProductRepository repo, Guid id) =>
    {
        repo.Delete(id);
        return Results.Ok();
 });


    app.Run();

record Product(Guid Id, 
    string? Name, 
    string? Description, 
    decimal Price);





//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
 class ProductRepository
{
    private readonly Dictionary<Guid, Product> _products = new();

    public void Create(Product product)
    {
        if (product == null)
        {
            return;
        }
        _products[product.Id] = product;
    }

    public Product GetById(Guid id)
    {
        return _products[id];
    }

    public List<Product> GetAll()
    {
        return _products.Values.ToList();
    }

    public void Update(Product product)
    {
        var existingProduct =GetById(product.Id);
        if (existingProduct is null)
        {
            return ;
        }

        _products[product.Id] = product;
    }
    public void Delete(Guid id)
    {
        _products.Remove(id);
    }
}


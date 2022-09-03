
using FA22.P02.Web.Features.Products;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

var currentId = 1;
var products = new List<ProductDto>
{
    new ProductDto
    {
        Id = currentId++,
        Name = "Xbox1",
        Description = "Newest Xbox Console",
        Price = 699.99m,
    },
    new ProductDto
    {
        Id = currentId++,
        Name = "PS5",
        Description = "Sony's newest PlayStation",
        Price = 599.99m,
    },
    new ProductDto
    {
        Id = currentId++,
        Name = "Xbox 360",
        Description = "Good Condition, 1 previous owner",
        Price = 199.99m
    }
};



app.MapGet("/api/products", () =>
 {
     return products;
 })
    .Produces(200, typeof(ProductDto[]));


app.MapGet("/api/products/{id}", ( int id) =>
{
    var result = products.FirstOrDefault(x => x.Id == id);
    if (result == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result);
})
    .WithName("GetProductById")
    .Produces(404)
    .Produces(200, typeof(ProductDto));

app.MapPost("/api/products", (ProductDto product) =>
{
    if (string.IsNullOrWhiteSpace(product.Name) ||
            product.Name.Length > 120 ||
            product.Price <= 0 ||
            string.IsNullOrWhiteSpace(product.Description))
    {
        return Results.BadRequest();
    }

    product.Id = currentId++;
    products.Add(product);
    return Results.CreatedAtRoute("GetProductById", new { id = product.Id }, product);
})
    .Produces(400)
    .Produces(201, typeof(ProductDto));

app.MapPut("api/products/{id}", ( int id, ProductDto product) =>
{
    if (string.IsNullOrWhiteSpace(product.Name) ||
           product.Name.Length > 120 ||
           product.Price <= 0 ||
           string.IsNullOrWhiteSpace(product.Description))
    {
        return Results.BadRequest();
    }

    var current = products.FirstOrDefault(x => x.Id == id);
    if (current == null)
    {
        return Results.NotFound();
    }

    current.Name = product.Name;
    current.Name = product.Name;
    current.Price = product.Price;
    current.Description = product.Description;

    return Results.Ok(current);
})
    .Produces(400)
    .Produces(404)
    .Produces(200, typeof(ProductDto));

app.MapDelete("api/products/{id}", (int id) =>
    {
        var current = products.FirstOrDefault(x => x.Id == id);
        if (current == null)
        {
            return Results.NotFound();
        }

        products.Remove(current);

        return Results.Ok();
    })
    .Produces(400)
    .Produces(404)
    .Produces(200, typeof(ProductDto));


app.Run();






//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }
/*class ProductRepository
{
    private readonly Dictionary<int, Product> _products = new();

    public void Create(Product product)
    {
        if (product == null)
        {
            return;
        }
        _products[product.Id] = product;
    }

    public Product GetById(int id)
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
    public void Delete(int id)
    {
        _products.Remove(id);
    }
}
*/

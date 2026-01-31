using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<NorthwndContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("nor")));

builder.Services.AddScoped<CustomersServiceImp>();
var manager = builder.Configuration;



string? connectionString = manager.GetConnectionString("nor");
//builder.Services.AddControllerWithViews();//客製化 掛入MVC

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Movies", "films");
});
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddScoped<ConnectionFactory>(sp =>
//    new ConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<NorthwndContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("nor")));
//builder.Services.AddDbContext<NorthwndContext>(
//    (builder) => {
//        builder.UseSqlServer(connectionString);
//    }
//    );

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//客製化
builder.Services.AddSwaggerGen(
      //Lambda
      (options) =>
      {
          options.SwaggerDoc("v1",
              new Microsoft.OpenApi.Models.OpenApiInfo()
              {
                  Title = "Parking Request API",
                  Version = "v1",
                  Description = "電影院停車管理系統",
                  //授權聲明
                  License = new Microsoft.OpenApi.Models.OpenApiLicense()
                  {
                      Url = new Uri("https://www.jack.com/license"),
                      Name = "使用條款"
                  }

              }
              );

          //
          var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
          var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
          //         options.IncludeXmlComments(xmlPath, true);
          options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);



      }

    );



//ConnectionFactory factory = new ConnectionFactory(connectionString);

//builder.Services.AddDbContext<NorthwndContext>(
//    (builder) =>
//    {
//        builder.UseSqlServer(connectionString);
//    });



//builder.Services.AddScoped<IOperations<Customers, String>, CustomersServiceImp, CustomersServiceImp>();
builder.Services.AddScoped<IOperations<Customers, string>, CustomersServiceImp>();
// 其他服務與中介軟體 攔截器middleware(地圖)
builder.Services.AddControllers();
builder.Services.AddTransient<ConnectionFactory>();
var app = builder.Build();
app.MapRazorPages();
// Configure the HTTP request pipeline,middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("mydomain");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NorthwndContext>();
        context.Database.EnsureDeleted(); // Delete existing database
        context.Database.EnsureCreated(); // Ensure database and tables are created

        if (!context.ApiKeys.Any())
        {
            context.ApiKeys.Add(new ApiKey { Key = "TEST_API_KEY_1" });
            context.ApiKeys.Add(new ApiKey { Key = "TEST_API_KEY_2" });
            context.SaveChanges();
            Console.WriteLine("Seeding ApiKey table with test data.");
        }
        else
        {
            Console.WriteLine("ApiKey table already contains data.");
        }
        Console.WriteLine("Current API Keys:");
        foreach (var key in context.ApiKeys.ToList())
        {
            Console.WriteLine($" - {key.Key}");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating or seeding the DB.");
    }
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/customers/id/{cid}", async ([FromRoute(Name = "cid")] string cid, NorthwndContext db) =>
{
    var response = new ResponseDto();
    response.ContentType = "application/json;charset=UTF-8";

    var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerID.ToString() == cid);

    return customer is not null
        ? Results.Ok(customer)       // ✅ 注意是 Results
        : Results.NotFound($"Customer {cid} not found.");
}).WithName("GetCustomerById")
.Produces<Customers>(StatusCodes.Status200OK)
.Produces<Message>(StatusCodes.Status404NotFound);



app.MapPost("/api/enter-parking/vehicle", (
    [FromQuery] string vehicleId,
    [FromQuery] int spotId,
    [FromQuery] DateTime entryTime,
    [FromQuery] DateTime exitTime,
    [FromQuery] decimal fee) =>
{
    return Results.Ok($"Create Entering parking for vehicle: {vehicleId}. Spot: {spotId}, Fee: {fee}");
})
.WithName("CreateParkingRecord")
.WithOpenApi();

// ✅ Exit parking API
app.MapDelete("/api/exit-parking/vehicle", ([FromQuery(Name = "vehicle_Id")] string vehicle_Id) =>
{
    return Results.Ok($"Vehicle {vehicle_Id} exited successfully.");
})
.WithName("ExitParking")
.WithOpenApi();


app.MapGet("/helloworld", (string who) =>
{
    return new { code = 200, message = $"Hello, {who}" };
});

app.MapPost("/api/car/request-parking-spot", ([FromQuery(Name = "vehicle_Id")] string vehicle_Id) =>
{
    return Results.Ok($"有人請求車位：2025-06-23T13:56:28.772170900");
})
.WithName("assigned")
.WithOpenApi();

app.MapGet("/testdb", (ConnectionFactory dbAccess) =>
{
    dbAccess.ConnectToDatabase();
    return Results.Ok("資料庫連線測試完成");
});

// PUT: 更新整筆資料
app.MapPut("/api/parking-records/update", (
    [FromQuery] string vehicleId,
    [FromQuery] int newSpotId) =>
{
    return Results.Ok($"Vehicle {vehicleId} moved to spot {newSpotId}");
})
.WithName("UpdateParkingSpot")
.WithOpenApi();


app.MapPost("/api/parking-records/request-parking-spot",
    ([FromQuery(Name = "state")] int state) =>
    {
        // 模擬回傳資料（可替換成資料庫查詢）
        var result = new { message = $"Requested parking spot with state: {state}" };
        return Results.Ok(result);
    })
.WithName("RequestParkingSpot{'parking_lot': 1,'spot_number': 2}")
.WithOpenApi();
// PATCH: 更新部分欄位（例如只修改費用）
app.MapPatch("/api/parking-records/update-fee", (
    [FromQuery] string vehicle_Id,
    [FromQuery] decimal newFee) =>
{
    return Results.Ok($"Vehicle {vehicle_Id} new fee updated to {newFee}");
})
.WithName("UpdateParkingFee")
.WithOpenApi();

// ✅ Get parking data API
app.MapGet("/api/parking-data/vehicle", ([FromQuery] string vehicle_Id) =>
{
    return Results.Ok($"Parking data for vehicle {vehicle_Id} retrieved successfully.");
})
.WithName("GetParkingData")
.WithOpenApi(
    (operations) =>
    {
        operations.Summary = "取得停車資料";
        operations.Description = "這個端點用來取得停車資料";
        return operations;
    }
    );



app.MapPost("/customers/add",
    (Customers customers, NorthwndContext northwndContext) =>
    {
        northwndContext.Customers.Add(customers);
        northwndContext.SaveChanges();

        var existCustomer = northwndContext.Customers.Find(customers.CustomerID);
        northwndContext.Customers.Add(customers);

        if (existCustomer != null)
        {
            return Results.BadRequest(new Message());
        }
        else
        {
            return Results.NotFound(new Message());

        }

        //return Results.Ok();


    }
    ).Accepts<Customers>("application/json")
     .Produces<Message>(StatusCodes.Status200OK)
     .Produces<Message>(StatusCodes.Status400BadRequest);
app.MapGet("/weatherforcast",
    () =>
    {
        var forcast = Enumerable.Range(0, 10).Select(index => new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();
        return forcast;
    });
app.MapPost("/insert/{id}", async (ConnectionFactory dbAccess, int id, string Type, string platform, int whool) =>
{
    dbAccess.InsertData(id, Type, platform, whool);
    return Results.Ok("資料插入請求已處理");
});
app.MapPost("/api/enter-parking/car", (
    [FromQuery] string vehicleId,
    [FromQuery] int spotId,
    [FromQuery] DateTime entryTime,
    [FromQuery] DateTime exitTime,
    [FromQuery] decimal fee) =>
{
    // 這裡可以加上資料儲存邏輯，例如寫入資料庫
    return Results.Ok(new
    {
        message = $"Create Entering parking for vehicle: {vehicleId}.",
        spotId,
        entryTime,
        exitTime,
        fee
    });
})
.WithName("EnterParkingViaQuery")
.WithOpenApi();
app.MapControllers();

app.Urls.Add("http://localhost:5065");


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
record MessageDto(string Text); // 給 POST API 使用的 DTO


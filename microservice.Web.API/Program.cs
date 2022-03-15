using microservice.Core;
using microservice.Core.IServices;
using microservice.Data.Access.Services;
using microservice.Data.SQL;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, lc) => lc
.ReadFrom.Configuration(context.Configuration)
);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials();
}));

builder.Services.AddHttpClient("localhost").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
}); 


builder.Services.AddDbContext<AppointmentsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlCon"), b => b.MigrationsAssembly("microservice.Data.SQL")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen(options => options.CustomSchemaIds(type => type.ToString()));
builder.Services.AddTransient<IUnitOfWork ,UnitOfWork>();
builder.Services.AddTransient<IAppointmentService ,AppointmentsService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSwagger();

app.UseCors();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.UseSwaggerUI();

app.Run();

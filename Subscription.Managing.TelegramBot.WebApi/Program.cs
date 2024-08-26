var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddWebServices();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.InitialiseDatabaseAsync();

    app.UseHttpsRedirection();
    app.MapControllers();

    var telegramBotService = app.Services.GetRequiredService<ITelegramBotService>();
    await telegramBotService.SendMessageAsync(5145, "From program.cs");

    app.Run();
}
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Optevus.Ethnicity.Business;
using Optevus.Ethnicity.Business.Interface;
using Optevus.Ethnicity.Business.Queries;
using Optevus.Ethnicity.Repository;
using Optevus.Ethnicity.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
        options.Events = new JwtBearerEvents();
    }, options => { builder.Configuration.Bind("AzureAdB2C", options); });

builder.Services.AddAuthorization();

builder.Services.AddMediatR(typeof(GetBusinessDivisionsQueryHandler).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IJobRepository, JobRepository>();
builder.Services.AddTransient<IJobService, JobService>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RegenbogenRadar_WebApi>("regenbogenradar-webapi");

builder.Build().Run();

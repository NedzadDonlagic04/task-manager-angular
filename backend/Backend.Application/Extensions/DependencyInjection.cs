using Backend.Application.Interfaces.Tasks;
using Backend.Application.Interfaces.Users;
using Backend.Application.Services.Tasks;
using Backend.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskStateService, TaskStateService>();
        services.AddHostedService<TaskDeadlineMonitorService>();

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}

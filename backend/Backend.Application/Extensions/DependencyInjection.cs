using Backend.Application.Services;
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

        return services;
    }
}

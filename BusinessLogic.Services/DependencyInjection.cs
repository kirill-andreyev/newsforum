using AutoMapper;
using BusinessLogic.Services.Implementations.Services;
using BusinessLogic.Services.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();

            return services;
        }
    }
}

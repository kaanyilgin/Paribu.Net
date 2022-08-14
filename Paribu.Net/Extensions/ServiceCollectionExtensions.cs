using System;
using Microsoft.Extensions.DependencyInjection;
using Paribu.Net.Contracts;
using Paribu.Net.CoreObjects;

namespace Paribu.Net.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddParibuClient(this IServiceCollection services)
        {
            services.AddSingleton<IParibuClient, ParibuClient>(x => new ParibuClient());
        }
        
        public static void AddParibuClient(this IServiceCollection services, Func<IServiceProvider, string> configureBaseUrl)
        {
            services.AddSingleton<IParibuClient, ParibuClient>(x => new ParibuClient(new ParibuClientOptions(configureBaseUrl(x))));
        }
        
        public static void AddParibuClient(this IServiceCollection services, ParibuClientOptions options)
        {
            services.AddSingleton<IParibuClient, ParibuClient>(x => new ParibuClient(options));
        }

        public static void AddParibuClient(this IServiceCollection services, string token)
        {
            services.AddSingleton<IParibuClient, ParibuClient>(x => new ParibuClient(token));
        }

        public static void AddParibuClient(this IServiceCollection services, ParibuClientOptions options, string token)
        {
            services.AddSingleton<IParibuClient, ParibuClient>(x => new ParibuClient(options, token));
        }
    }
}
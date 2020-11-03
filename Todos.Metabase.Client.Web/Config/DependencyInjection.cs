using Microsoft.Extensions.DependencyInjection;

namespace Todos.Metabase.Client.Web
{
    public static class DependencyInjection
    {
        internal static void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<QueryClient>();
            services.AddTransient<SessionClient>();
        }
    }
}

using Applicazione_1.Settings;
using System.Runtime.CompilerServices;

namespace Applicazione_1.ExtentionMethods
{
    public static class ConfigurationExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns><see cref="IServiceCollection"/></returns>
      
        public static IServiceCollection ConfigureApp(this IServiceCollection services,IConfiguration config)
        {
            services.Configure<HttpConnectionSettings>(config.GetSection("HttpConnectionSettings"));
            services.Configure<DocumentStorageSettings>(config.GetSection("DocumentStorageSettings"));
            return services;
        }
    }
}

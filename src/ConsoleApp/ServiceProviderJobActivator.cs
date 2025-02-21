using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public class ServiceProviderJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type) => _serviceProvider.GetRequiredService(type);
    }
}

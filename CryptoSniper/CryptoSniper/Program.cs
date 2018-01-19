using CryptoSniper.Config;
using Topshelf;

namespace CryptoSniper
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.AddCommandLineDefinition("config", config => Configuration.SetConfigPathToProgramData(config));

                serviceConfig.Service<CryptoSniperService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(
                        () => new CryptoSniperService());

                    serviceInstance.WhenStarted(execute => execute.Start());

                    serviceInstance.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("CryptoSniper");
                serviceConfig.SetDisplayName("CryptoSniper");
                serviceConfig.SetDescription("");

                serviceConfig.StartAutomatically();

                serviceConfig.EnableServiceRecovery(r =>
                {
                    r.RestartService(0);
                    r.RestartService(0);
                    r.RestartService(0);
                    r.OnCrashOnly();
                });
            });
        }
    }
}

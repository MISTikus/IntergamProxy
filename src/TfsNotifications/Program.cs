using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using NLog.Web;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TfsNotifications
{
    public class Program
    {
        private const string certificateName = "TfsNotifications";

        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var isService = !(Debugger.IsAttached || args.Contains("--console") || args.Contains("-c"));
                var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console" && arg != "-c").ToArray());

                if (isService)
                {
                    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                    builder.UseContentRoot(pathToContentRoot);
                }

                if (isService)
                    builder.Build().RunAsService();
                else
                    builder.Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost
                .CreateDefaultBuilder(args)
                .UseNLog()
                .UseStartup<Startup>();
    }
}
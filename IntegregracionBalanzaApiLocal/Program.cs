using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace IntegregracionBalanzaApiLocal
{

  
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<Webserver>(s => {
                    s.ConstructUsing(name => new Webserver());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                // x.RunAsLocalSystem();
                x.RunAsLocalSystem();

                //  x.UseNLog();

                x.SetServiceName("ErpWeb.IntegregracionBalanzaApiLocalFCH");
                x.SetDescription("ErpWeb.IntegregracionBalanzaApiLocalFCH");
               // x.StartAutomatically();

                //  x.RunAs(@"serviceAccountName", "********");
                x.EnableServiceRecovery(r => r.RestartService(1));
            });

            //var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());  //11
            //Environment.ExitCode = exitCode;
        }


    }
}

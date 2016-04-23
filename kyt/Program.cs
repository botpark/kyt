using System;
using System.ServiceProcess;

namespace kyt
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {   
            #if DEBUG
                kyt _service = new kyt();
                _service.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            #else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new kyt()
                };
                ServiceBase.Run(ServicesToRun);
            #endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune_Common.Services
{
    public static class ServiceLocator
    {
        private static Dictionary<String, IService> ServiceInstances = new Dictionary<String, IService>();

        public static void SetInstance(IService service)
        {
            ServiceInstances[service.GetType().Name] = service;
            Debug.WriteLine(ServiceInstances);
        }

        public static IService GetInstance(String name)
        {
            if (!ServiceInstances.ContainsKey(name)) return null;

            return ServiceInstances[name];
        }
    }
}

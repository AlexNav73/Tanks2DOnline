using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Factory.Interfaces;

namespace Tanks2DOnline.Client.ConsoleClient.Configuration.Creators
{
    public class ClientConfigurationCreator : ICreator
    {
        public object Create(Params prms)
        {
            return new ClientConfiguration()
            {
                Port = prms.GetValue<int>(Params.Port),
                RegistrationTimeout = prms.GetValue<int>(Params.ServerRegistrationTimeout),
                ServerIP = prms.GetValue<string>(Params.ServerIP),
                ServerPort = prms.GetValue<int>(Params.ServerPort)
            };
        }
    }
}

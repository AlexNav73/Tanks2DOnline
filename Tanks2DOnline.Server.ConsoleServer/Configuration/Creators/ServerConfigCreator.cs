using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Factory.Interfaces;

namespace Tanks2DOnline.Server.ConsoleServer.Configuration.Creators
{
    public class ServerConfigCreator : ICreator
    {
        public object Create(Params prms)
        {
            return new ServerConfiguration()
            {
                Port = prms.GetValue<int>(Params.Port)
            };
        }
    }
}

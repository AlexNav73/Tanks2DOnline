using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Client.ConsoleClient.Actions
{
    public interface IAction
    {
        void Execute(Client client, Packet packet);
    }
}
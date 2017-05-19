using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Interfaces
{
    public interface ICommunicationProtocol
    {
        void Send(string message);
    }

    //Dictionary<string, string> ParseMessage(string message);
    //void ProcessMessage(Dictionary<string, string> parsedMessage);
}
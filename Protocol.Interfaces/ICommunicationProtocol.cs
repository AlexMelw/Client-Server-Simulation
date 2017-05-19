using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Interfaces
{
    using System.Net;

    public interface ICommunicationProtocol
    {
        //Dictionary<string, string> ParseMessage(string message);
        //void ProcessMessage(Dictionary<string, string> parsedMessage);

        void Send(string message);
        void Receive(string message);
    }

    public interface ICommunicationProtocolRequestProcessor
    {
        Dictionary<string, string> ParseRequest(string request);
        void ProcessRequest(Dictionary<string, string> parsedRequest);
    }

    public interface ICommunicationProtocolResponseProcessor
    {
        Dictionary<string, string> ParseResponse(string response);
        void ProcessResponse(Dictionary<string, string> parsedResponse);
    }


    /* TCP/UDP Client */
    public interface IClientWorker : ICommunicationProtocol
    {
        void Init(IPAddress ipAddress, int port);
        void StartCommunication();
    }


    /* TCP/UDP Server */
    public interface IServerWorker : ICommunicationProtocol
    {
        void Init(IPAddress ipAddress, int port);
        void StartServing();
    }
}
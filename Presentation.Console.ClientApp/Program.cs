namespace Presentation.Console.ClientApp
{
    using Protocol.Implementation;
    using Protocol.Implementation.Response;
    using Protocol.Implementation.Workers;
    using Protocol.Interfaces.CommonConventions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var tcpWorker = new TcpClientWorker(new ResponseProcessor(new ResponseParser()));
            tcpWorker.Init(Conventions.Localhost, Conventions.TcpServerListeningPort);
            tcpWorker.StartCommunication();
        }
    }
}
namespace Presentation.Console.ClientApp
{
    using Protocol.Implementation;
    using Protocol.Implementation.Response;
    using Protocol.Implementation.Workers;

    internal class Program
    {
        private const string Localhost = "127.0.0.1";
        private const int ConnectionPort = 5501;

        private static void Main(string[] args)
        {
            var tcpWorker = new TcpClientWorker(new ResponseProcessor(new ResponseParser()));
            tcpWorker.Init(Localhost, ConnectionPort);
            tcpWorker.StartCommunication();
        }
    }
}
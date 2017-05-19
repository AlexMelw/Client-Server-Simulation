namespace Presentation.Console.ClientApp
{
    using Protocol.Implementation;

    internal class Program
    {
        private const string Localhost = "127.0.0.1";
        private const int ConnectionPort = 5501;

        private static void Main(string[] args)
        {
            var tcpWorker = new TcpWorker();
            tcpWorker.Init(Localhost, ConnectionPort);
            tcpWorker.StartCommunication();
        }
    }
}
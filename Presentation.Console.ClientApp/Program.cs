namespace Presentation.Console.ClientApp
{
    internal class Program
    {
        const string Localhost = "127.0.0.1";
        private const int ConnectionPort = 5501;

        private static void Main(string[] args)
        {
            TcpWorker tcpWorker = new TcpWorker();
            tcpWorker.Init(Localhost, ConnectionPort);
            tcpWorker.StartCommunication();
        }
    }
}
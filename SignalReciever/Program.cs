using SignalRecieverAnalyzer.Working;


namespace SignalReciever
{
    public class Program // client
    {
        public static async Task Main(string[] args)
        {
            var sync = new StartWorking();

            await sync.StartConnectingToServer(10);

        }
    }
}

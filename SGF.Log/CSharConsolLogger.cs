namespace SGF.Log
{
    public class CSharConsolLogger : ILoger, IOnLoggerInit
    {

        public void E(object message)
        {
            System.Console.WriteLine(message);
        }

        public void D(object message)
        {
            System.Console.WriteLine(message);
        }

        public void R(object message)
        {
            System.Console.WriteLine(message);
        }

        public void A(bool con, object message)
        {
            System.Console.WriteLine(message);
        }

        public void W(object message)
        {
            System.Console.WriteLine(message);
        }

        public void OnInit()
        {
            //TODO
        }
    }
}

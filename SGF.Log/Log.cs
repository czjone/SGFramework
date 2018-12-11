namespace SGF.Log
{
    public enum LogPlatform
    {
        CSHARP_DEFAULT_CONSOL,
        UNITY_LOGGER,
    }

    public sealed class Log
    {
        private static ILoger logger;

        static Log()
        {
            SetLogger(new CSharConsolLogger());
        }

        public static void SetLogger<T>(T logger) where T:ILoger,IOnLoggerInit
        {
            Log.logger = logger;
            logger.OnInit();
        }

        public static void E(object message)
        {
            logger.E(message);
        }

        public static void D(object message)
        {
            logger.D(message);
        }

        public static void R(object message)
        {
            logger.R(message);
        }

        public static void A(bool con, object message)
        {
            logger.A(con,message);
        }

        public static void W(object message)
        {
            logger.W(message);
        }
    }
}

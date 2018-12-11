using UnityEngine;
namespace SGF.Log
{
    public class UnityLogger : ILoger, IOnLoggerInit
    {
        public UnityLogger()
        {
            UnityEngine.Application.RegisterLogCallback(this.UnityLogHandler);
        }

        private void UnityLogHandler(string condition, string stackTrace, LogType type)
        {
           
        }

        public void E(object message)
        {
            throw new System.NotImplementedException();
        }

        public void D(object message)
        {
            throw new System.NotImplementedException();
        }

        public void R(object message)
        {
            throw new System.NotImplementedException();
        }

        public void A(bool con, object message)
        {
            throw new System.NotImplementedException();
        }

        public void W(object message)
        {
            throw new System.NotImplementedException();
        }

        public void OnInit()
        {
            throw new System.NotImplementedException();
        }
    }
}

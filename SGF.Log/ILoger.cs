namespace SGF.Log
{
    public interface ILoger
    {
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        void E(object message);
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        void D(object message);
        /// <summary>
        /// Release
        /// </summary>
        /// <param name="message"></param>
        void R(object message);
        /// <summary>
        /// Asset
        /// </summary>
        /// <param name="con"></param>
        /// <param name="message"></param>
        void A(bool con,object message);
        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        void W(object message);
    }
}

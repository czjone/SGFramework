namespace SGF.Core {
    public static class StringExtentions {
        public static string FormatWith(this string tag,params object[] args) {
            return string.Format(tag,args);
        }
    }
}
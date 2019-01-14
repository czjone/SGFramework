namespace SGF.Core {
    public static class StringExtentions {
        public static string FormatWith(this string tag,params object[] args) {
            return string.Format(tag,args);
        }

        public static string ReplateWith(this string tag,string oldstr,string newstr) {
            return tag.Replace(oldstr,newstr);
        }
    }
}
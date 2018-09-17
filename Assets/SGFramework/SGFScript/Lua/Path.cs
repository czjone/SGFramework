namespace SGF.Lua {
    public static class Path {
        /// <summary>
        /// wins.aaa.bbb to wins/aaa/bbb
        /// </summary>
        /// <param name="luaStypePat"></param>
        /// <returns></returns>
        public static string ToNormalPathWithoutFileExtention(this string luaStypePath){
            return luaStypePath.Replace(".",SGF.Core.Path.DirSplitor);
        }
    }
}
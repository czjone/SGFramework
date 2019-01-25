namespace SGF.Unity.XLuaExt {
    using SGF.Core;
    using SGF.Unity.Utils;
    using XLua;

    public class Lua {

        private Game game;
        private LuaEnv luaEnv;

        public Lua (Game game) {
            this.game = game;
            this.luaEnv = new XLua.LuaEnv ();
            this.AddLuaLoader (this.CustomLuaLoader);
        }

        public void require (string lua) {
            string luastr = "require \"{0}\"".FormatWith (lua);
            this.luaEnv.DoString (luastr);
        }

        public void AddLuaLoader (LuaEnv.CustomLoader loader) {
            this.luaEnv.AddLoader (loader);
        }

        //暂时不缓存文件系统中的数据，由resmgr来管理
        byte[] CustomLuaLoader (ref string filepath) {
            string path = filepath.ReplateWith (".", "/"); //lua格式的包引入转化成文件路
            var resMgr = IoC.Get<ResMgr> ();
            return resMgr.Load<LuaAssets> (path).bytes;
        }

        public void OnDestory () {
            this.luaEnv.Dispose ();
            this.luaEnv = null;
        }
    }

    public class LuaAssets : UnityEngine.Object {
        public byte[] bytes;
    }
}
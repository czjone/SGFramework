
namespace SGF
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SGF.Unity;

    public class XLuaMain : MonoBehaviour
    {

        private XLua.LuaEnv luaEnv;
        private ResMgr resMgr;

        void Start()
        {
            resMgr = new ResMgr(this);
            luaEnv = new XLua.LuaEnv();
            byte[] luaBytes = resMgr.GetRes("main");
            string luaString = System.Text.UTF8Encoding.UTF8.GetString(luaBytes);
            luaEnv.DoString(luaString);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Destroy()
        {
            if (luaEnv != null)
            { 
                luaEnv.Dispose();
                this.luaEnv = null;
            }
        }
       
    }
}
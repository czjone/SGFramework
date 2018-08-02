using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LuaUIEditor   {

	//添加菜单
    // [MenuItem(@"XLuaUI/GetTransforms")]
	[AddComponentMenu(@"XLuaUI/GetTransforms")]
    public static void GetTransforms()
    {
        Dictionary<string, Vector3> dic = new Dictionary<string, Vector3>();
        //transforms是Selection类的静态字段，其返回的是选中的对象的Transform
        Transform[] transforms = Selection.transforms;

        //将选中的对象的postion保存在字典中
        for (int i = 0; i < transforms.Length; i++)
        {
           dic.Add(transforms[i].name,transforms[i].position);
        }

        //将字典中的信息打印出来
        foreach (Transform item in transforms)
        {
            Debug.Log(item.name+":"+item.position);
        }
    }
}

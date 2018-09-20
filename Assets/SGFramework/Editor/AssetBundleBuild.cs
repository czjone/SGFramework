//#if UNITY_EDITRO
using System.Collections.Generic;
using SGF.Unity;
using UnityEditor;
using UnityEngine;

public interface IBuild {
    bool Build ();

    void SetBuildArgs (BuildArgs arg);
}

public class AndroidPKGConfig {

    public string Name { get; set; }

    public string UnityPlugInsPath { get; set; }
}

[System.Serializable]
public class BuildArgs : JsonSerializable<BuildArgs> {

    public int VersionCode { get; set; }

    public string VersionName { get; set; }

    public AndroidPKGConfig[] Apkes { get; set; }
}

public class BuildAndroidAssetBundle : IBuild {

    public BuildArgs BuildArgs { get; private set; }

    public BuildAndroidAssetBundle (BuildArgs args) {
        this.BuildArgs = args;
    }

    public BuildAndroidAssetBundle () {

    }

    public bool Build () {
        string outPath = Application.dataPath + "/../AndroidAssetBundles";
        UnityEditor.BuildPipeline.BuildAssetBundles (outPath, BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
        UnityEngine.Debug.Log ("生成android资源成功！");
        return true;
    }

    public void SetBuildArgs (BuildArgs arg) {
        this.BuildArgs = arg;
    }
}

public class BuildAndroidAPK : IBuild {

    public BuildArgs BuildArgs { get; private set; }
    public BuildAndroidAPK () {

    }

    public BuildAndroidAPK (BuildArgs args) {
        this.BuildArgs = args;
    }

    public bool Build () {
        throw new System.NotImplementedException ();
    }
    public void SetBuildArgs (BuildArgs arg) {
        this.BuildArgs = arg;
    }
}

public class BuildiOSAssetBundle : IBuild {

    public BuildArgs BuildArgs { get; private set; }

    public BuildiOSAssetBundle () {

    }

    public BuildiOSAssetBundle (BuildArgs args) {
        this.BuildArgs = args;
    }

    public bool Build () {
        throw new System.NotImplementedException ();
    }
    public void SetBuildArgs (BuildArgs arg) {
        this.BuildArgs = arg;
    }
}

public class BuildXCodeProject : IBuild {

    public BuildArgs BuildArgs { get; private set; }

    public BuildXCodeProject () {

    }
    public BuildXCodeProject (BuildArgs args) {
        this.BuildArgs = args;
    }

    public bool Build () {
        throw new System.NotImplementedException ();
    }
    public void SetBuildArgs (BuildArgs arg) {
        this.BuildArgs = arg;
    }
}

public class Builder {
    public static void Build (params IBuild[] builders) {
        foreach (var build in builders) {
            if (build.Build () == false) {
                break;
            }
        }
    }

    public static bool Build<T> (BuildArgs args) where T : IBuild, new () {
        T t = new T ();
        t.SetBuildArgs (arg: args);
        t.Build ();
        return t.Build ();
    }
}

public class AssetBundleBuild {

    private const string MENU_ROOT = "SGTool";

    [MenuItem (MENU_ROOT + "/Build Android AssetsBundle")]
    private static void BuildAndroidAsset () {
        BuildArgs args = null;
        Builder.Build<BuildAndroidAssetBundle> (args);
    }

    [MenuItem (MENU_ROOT + "/Build iOS AssetsBundle")]
    private static void BuildIosAssetBundle () {
        BuildArgs args = null;
        Builder.Build<BuildAndroidAssetBundle> (args);
    }

    [MenuItem (MENU_ROOT + "/Build Android APK")]
    private static void BuildAndroidAPK () {
        BuildArgs args = null;
        Builder.Build<BuildAndroidAssetBundle> (args);
    }

    [MenuItem (MENU_ROOT + "/Build iOS XCode")]
    private static void BuildiOSXCode () {
        BuildArgs args = null;
        Builder.Build<BuildAndroidAssetBundle> (args);
    }
}
// #endif
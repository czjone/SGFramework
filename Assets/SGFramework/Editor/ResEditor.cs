//#if UNITY_DITOR

using System.Collections;
using System.Collections.Generic;
using SGF.Core;
using SGF.HotUpdate;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ResEditorConfig : JsonSerializable<ResEditorConfig> {
    public string BuildAssetsPath { get; set; }

    public string BuildResDir { get; set; }

    public string DevScriptDir { get; set; }

    public string BuildScriptDir { get; set; }

    public bool IsPublic { get; set; }
}

public class ResEditor {

    private static string configPath = Path.Legalization (Application.dataPath + "/SGFConfig/SGFConfig.json");

    private static string versionFileName = ResVersion.VerFileName;
    private const string MenuAssetBuilldRoot = "SGF工具/编译资源/";
    private const string MenuPacthToolRoot = "SGF工具/补丁工具/";

    private const string MenuDebugToolRoot = "SGF工具/调试工具/";

    static SGF.HotUpdate.PlatForm BuildTargetTo (BuildTarget targetPlatform) {
        if (BuildTarget.StandaloneWindows64 == targetPlatform) return SGF.HotUpdate.PlatForm.WIN;
        else if (BuildTarget.StandaloneOSXIntel64 == targetPlatform) return SGF.HotUpdate.PlatForm.MAC;
        else if (BuildTarget.iOS == targetPlatform) return SGF.HotUpdate.PlatForm.IOS;
        else if (BuildTarget.Android == targetPlatform) return SGF.HotUpdate.PlatForm.ANDROID;
        return SGF.HotUpdate.PlatForm.UNSUPPORTS;
    }

    static void BuildAssetsBundle (BuildTarget targetPlatform) {
        var resEditorConfig = ResEditorConfig.LoadWithFile (configPath);
        var resDir = Path.Legalization (resEditorConfig.BuildAssetsPath + "/" + resEditorConfig.BuildResDir);
        SGF.Core.File.CheckDir (resDir, true);
        //编译资源
        BuildPipeline.BuildAssetBundles (resDir, BuildAssetBundleOptions.CompleteAssets, targetPlatform);
        //加密脚本
        var srcPath = Path.Legalization (resEditorConfig.DevScriptDir);
        var tagPath = Path.Legalization (resEditorConfig.BuildAssetsPath + "/" + resEditorConfig.BuildScriptDir);
        BuildScript (srcPath, tagPath);
        Patch_VersionFile (BuildTargetTo (targetPlatform));
    }

    [UnityEditor.MenuItem (MenuAssetBuilldRoot + "当前开发平台")]
    static void BuildAssetsBundleCurrent () {
        BuildAssetsBundle (EditorUserBuildSettings.activeBuildTarget);
    }

    [UnityEditor.MenuItem (MenuAssetBuilldRoot + "iOS")]
    static void BuildAssetsBundleiOS () {
        BuildAssetsBundle (BuildTarget.iOS);
    }

    [UnityEditor.MenuItem (MenuAssetBuilldRoot + "android")]
    static void BuildAssetsBundleAndroid () {
        BuildAssetsBundle (BuildTarget.Android);
    }

    [UnityEditor.MenuItem (MenuAssetBuilldRoot + "MacOS")]
    static void BuildAssetsBundleStandaloneOSXIntel64 () {
        BuildAssetsBundle (BuildTarget.StandaloneOSXIntel64);
    }

    [UnityEditor.MenuItem (MenuAssetBuilldRoot + "Clean Build Cache")]
    static void CleanBuildAssetsDir () {
        var resEditorConfig = ResEditorConfig.LoadWithFile (configPath);
        var outDir = Path.Legalization (resEditorConfig.BuildAssetsPath);
        if (System.IO.Directory.Exists (outDir)) System.IO.Directory.Delete (outDir, true);
        Debug.Log ("clean build cache success!");
    }

    [UnityEditor.MenuItem (MenuAssetBuilldRoot + "Windows")]
    static void BuildAssetsBundleStandaloneWindows64 () {
        BuildAssetsBundle (BuildTarget.StandaloneWindows64);
    }

    [UnityEditor.MenuItem (MenuPacthToolRoot + "打补丁到包体中")]
    static void Patch_toPakage () {
        var resEditorConfig = ResEditorConfig.LoadWithFile (configPath);
        BuildAssetsBundle (EditorUserBuildSettings.activeBuildTarget);
        SGF.Core.File.CopyDirectory (resEditorConfig.BuildAssetsPath, Application.streamingAssetsPath + Path.DirSplitor + "dres");
        AssetDatabase.Refresh ();
        Debug.Log ("补丁已经生成到StreamAssets!");
    }

    [UnityEditor.MenuItem (MenuPacthToolRoot + "清理包体中的资源")]
    static void Patch_cleanPakage () {
        var resEditorConfig = ResEditorConfig.LoadWithFile (configPath);
        var dresPath = Application.streamingAssetsPath + Path.DirSplitor + SGF.HotUpdate.TaskDecompressionPKGRes.DresFolder;
        if (System.IO.Directory.Exists (dresPath)) {
            System.IO.Directory.Delete (dresPath, true);
        }
        AssetDatabase.Refresh ();
        Debug.Log ("StreamAssets的补丁已清理");
    }

    [UnityEditor.MenuItem (MenuDebugToolRoot + "刷新调试资源")]
    static void RefDebugRes () {
        Patch_cleanPakage ();
        Patch_toPakage ();
        Debug.Log ("开发环境已配置好！");
    }

    /// <summary>
    /// 生成补丁版本文件
    /// </summary>
    /// <param name="targetPlatform"></param>
    /// <param name="isPublic"></param>
    static void Patch_VersionFile (SGF.HotUpdate.PlatForm targetPlatform) {
        var resEditorConfig = ResEditorConfig.LoadWithFile (configPath);
        //读取全部文件名
        var files = SGF.Core.File.GetDirFileList (resEditorConfig.BuildAssetsPath);
        //补丁版本文件完整路径
        var versionFile = Path.Legalization (resEditorConfig.BuildAssetsPath + Path.DirSplitor + versionFileName);
        //补丁版本对象
        SGF.HotUpdate.ResVersion ResVersion = null;
        if (System.IO.File.Exists (versionFile) == false)
            ResVersion = new SGF.HotUpdate.ResVersion ();
        else
            ResVersion = SGF.HotUpdate.ResVersion.LoadWithFile (versionFile);
        //发布版的版本号
        if (resEditorConfig.IsPublic == true)
            ResVersion.Version++;
        //开发版本的版本号
        ResVersion.devVersion++;
        //文件的版本描述
        var fVersions = new List<SGF.HotUpdate.FileVersion> ();
        //生成文件的md5
        foreach (var item in files) {
            var fname = Path.Legalization (item);
            if (fname.EndsWith (versionFileName) == true) continue;
            if (fname.EndsWith (".meta") == true) continue;
            SGF.HotUpdate.FileVersion fv = new SGF.HotUpdate.FileVersion ();
            fv.Path = fname.Replace (resEditorConfig.BuildAssetsPath + Path.DirSplitor, "");
            fv.MD5 = SGF.Core.File.EncryptWithMD5 (fname).ToUpper ();
            fVersions.Add (fv);
        }
        ResVersion.ResVer = fVersions.ToArray ();
        ResVersion.Platform = targetPlatform;
        //保存为json文件
        ResVersion.ToFile (versionFile);
        Debug.Log ("资源版本文件生成成功！");
    }

    static void BuildScript (string srcPath, string tagPath) {
        SGF.Core.File.DelDirectory (tagPath);
        SGF.Core.File.CheckDir (tagPath, true);
        SGF.Core.File.CopyDirectory (srcPath, tagPath);
    }
}
// #endif
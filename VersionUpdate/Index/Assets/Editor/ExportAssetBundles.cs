using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ExportAssetBundles : MonoBehaviour {

    //在Unity编辑器中添加菜单  
    [MenuItem("Custom Editor/Create AssetBunldes ALL")]
    static void ExportResourceRGB2()
    {
        // 打开保存面板，获得用户选择的路径  
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // 选择的要保存的对象  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包  
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);
        }
    }  


    [MenuItem("Custom Editor/Create Scene")]
    static void CreateSceneALL()
    {
        //清空一下缓存
        Caching.CleanCache();
        string Path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");  
        string[] levels = { "Assets/Scenes/LoginScene.unity" };
        //打包场景
        BuildPipeline.BuildPlayer(levels, Path, BuildTarget.WebPlayer, BuildOptions.BuildAdditionalStreamedScenes);
        AssetDatabase.Refresh();
    }


// C# Example
// Builds an asset bundle from the selected folder in the project view.
// Bare in mind that this script doesnt track dependencies nor is recursive
//在项目视图从选择的文件夹生成资源包
//记住，这个脚本不跟踪依赖关系，也不是递归

    [@MenuItem("Custom Editor/Build AssetBundles From Directory of Files")]
    static void ExportDirectoryAssetBundles()
    {
		// Get the selected directory
		//获取选择的目录
            string[] fileEntries = { "Assets/Scenes/LoginScene.unity", "Assets/Scenes/MainScene.unity" };// Directory.GetFiles(Application.dataPath + "/" + path);
			foreach(string fileName in fileEntries) {
                //string filePath = fileName.Replace("", "/");
                //int index = filePath.LastIndexOf("/");
                //filePath = filePath.Substring(index);
                //Debug.Log(filePath);
                //string localPath = "Assets/" + path;
                //if (index > 0)
                //localPath += filePath;
                Object t = AssetDatabase.LoadMainAssetAtPath(fileName);
				if (t != null) {
					Debug.Log(t.name);
                    string bundlePath = "Bundle/" + t.name + ".unity3d";
					Debug.Log("Building bundle at: " + bundlePath);
					// Build the resource file from the active selection.
					//从激活的选择编译资源文件
					BuildPipeline.BuildAssetBundle
					(t, null, bundlePath, BuildAssetBundleOptions.CompleteAssets);
				}

			}
		
	}

}

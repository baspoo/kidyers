#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Unity​Editor.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EditorMenu : MonoBehaviour
{


    static string pathOutput => "Assets/StreamingAssets/AssetBundle";
    static string pathInput => "Assets/AssetBundles";

    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        ChangeBundleNameAlls();

        string assetBundleDirectory = pathOutput;
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
    }
    [MenuItem("Assets/AssetBundle/Get AssetBundle names")]
    static void GetNames()
    {
        var names = AssetDatabase.GetAllAssetBundleNames();
        foreach (var name in names)
            Debug.Log("AssetBundle: " + name);
    }
    [MenuItem("Assets/ChangeBundleNames")]
    static void ChangeBundleNameAlls()
    {
        foreach (var guid in AssetDatabase.FindAssets("*", new[] { pathInput }))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant("assets", "");
        }
    }



    [MenuItem("Assets/CX")]
    static void CX()
    {
        //FileUtil.CopyFileOrDirectory("C:/Projects/SchoolProjects/GSP290/MyFirstGame/vector2.dll", "Desktop/vector2.dlll");

        var img = FindObjectOfType<ImagesDict>().imgs[0];

        XRReferenceImageLibrary x = (XRReferenceImageLibrary)Selection.activeObject;
        XRReferenceImageLibraryExtensions.SetTexture(x, 0, img, false);
        XRReferenceImageLibraryExtensions.SetSpecifySize(x, 0, true);
        XRReferenceImageLibraryExtensions.SetSize(x, 0, Vector2.one * 0.1f);
        XRReferenceImageLibraryExtensions.SetName(x, 0, "baspoo");
    }


}





#endif

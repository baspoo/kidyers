using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class URLData 
{
    public string Key;
    public string Url;
}

public class LoadAssetsBundle : MonoBehaviour
{
    public enum LoadType
    {
        Local, Streaming , URL
    }
    [SerializeField]
    private LoadType loadType;

    //Editor
    //MainContent
    public string midPath;

    public List<URLData> Urls;
    public string Find(string key) {
        var find = Urls.Find(x => x.Key == key);
        if (find == null)
            return string.Empty;
        else
            return find.Url;
    }

    public void Initialize()
    {

    }





    public void Initz(string bundlePath , System.Action<bool> callback)
    {
        StartCoroutine(IEInitz(bundlePath, callback));
    }
    public void LoadAsset(string bundlePath, string filePath, System.Action<Object,string> callback)
    {
        StartCoroutine(IELoadAsset(bundlePath, filePath, callback));
    }




    Dictionary<string, AssetBundle> dictstore = new Dictionary<string, AssetBundle>();
    string GetPathFile(string bundle) {

       
        if(loadType == LoadType.Streaming) 
        {
            string pathSeparator = "/";
            var url = $"{Application.streamingAssetsPath}{pathSeparator}AssetBundle{pathSeparator}{bundle.ToLower()}";
            return url;
        }
        return bundle;
    }


    WWW current;
    public float progress => current == null ? 0.0f : current.progress ;

    IEnumerator IEInitz(string bundlePath , System.Action<bool> callback)
    {
        yield return new WaitForEndOfFrame();

        if (loadType == LoadType.Local)
        {
            callback?.Invoke(true);
        }
        else
        {
            AssetBundle bundle = null;
            string path = GetPathFile(bundlePath);
            if (!dictstore.ContainsKey(bundlePath))
            {
                WWW www = new WWW(path);
                current = www;
                yield return www;
                
                if (www.error == null)
                {
                    bundle = www.assetBundle;
                    dictstore.Add(bundlePath, bundle);
                    callback?.Invoke(true);
                }
                else 
                {
                    Debug.LogError($"{path} : {www.error}");
                    callback?.Invoke(false);
                    yield break;
                }
               

            }
            else
            {
                callback?.Invoke(true);
            }
        }
    }
    IEnumerator IELoadAsset( string bundlePath , string filePath ,System.Action<Object,string> callback )
    {
        yield return new WaitForEndOfFrame();

        if (loadType == LoadType.Local)
        {
            var path = $"{midPath}/{bundlePath}/{filePath}";
            var result = Resources.Load(path);
            callback?.Invoke(result,null);
        }
        else 
        {
            AssetBundle bundle = null;
            string path = GetPathFile(bundlePath);
            if (!dictstore.ContainsKey(bundlePath))
            {
                WWW www = new WWW(path);
                current = www;
                yield return www;
                if (www.error == null)
                {
                    bundle = www.assetBundle;



                    var materials = bundle.LoadAllAssets(typeof(Material));
                    foreach (Material m in materials)
                    {
                        var shaderName = m.shader.name;
                        var newShader = Shader.Find(shaderName);
                        if (newShader != null)
                        {
                            m.shader = newShader;
                        }
                        else
                        {
                            Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
                        }
                    }



                    dictstore.Add(bundlePath, bundle);
                }
                else 
                {
                    Debug.LogError($"{path} : {www.error}");
                    callback?.Invoke(null, www.error);
                    yield break;
                }
            }
            else 
            {
                bundle = dictstore[bundlePath];
            }

            //var fullpath = $"Assets/AssetBundles/{midPath}/{bundlePath}/{filePath}.prefab";
            var fullpath = $"Assets/AssetBundles/{filePath}";
            Debug.Log(fullpath);
            var loadAsset = bundle.LoadAssetAsync(fullpath);
            yield return loadAsset;
            var result = loadAsset.asset;
            callback?.Invoke(result,null);
        }
    }




}





































/*
public void Load1() {
    string path =   "http://junk.onemoby.com/baspoo/webinteractive/buildversion/v001/StreamingAssets/image.unity3d";
    //StartCoroutine(LoadStart(path));
}
public void Load2() 
{
    string path = Path.Combine(Application.streamingAssetsPath, "image.unity3d");
    //StartCoroutine(LoadStart(path));
}
*/
/*
   public SpriteRenderer[] sprites;
   IEnumerator Starts()
   {
       var uwr = UnityWebRequestAssetBundle.GetAssetBundle("http://junk.onemoby.com/baspoo/GameHtml5-2019/b4/Assets/image.unity3d");
       yield return uwr.SendWebRequest();


       Debug.LogError(uwr.isDone);
       Debug.LogError(uwr.error);
       // Get an asset from the bundle and instantiate it.
       AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
       //var loadAsset = bundle.LoadAssetAsync<GameObject>("Assets/Players/MainPlayer.prefab");
       //yield return loadAsset;

       var loadAsset = bundle.LoadAssetAsync<Texture>("Assets/Img/1.png");
       yield return loadAsset;
       img.texture = (Texture)loadAsset.asset;

       int index = 1;
       foreach (var spr in sprites) 
       {
           var a = bundle.LoadAssetAsync<Sprite>($"Assets/Img/{index}.png");
           yield return a;
           spr.sprite = (Sprite)a.asset;
           index++;
       }

       //Instantiate(loadAsset.asset);
   }
   */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ImagesDict : MonoBehaviour
{
    public RawImage img;
    public ARTrackedImageManager m_TrackedImageManager;
    public List<Texture2D> imgs;
    public LoadAssetsBundle m_LoadAssetsBundle;
    public XRReferenceImageLibrary m_ImageLibrary;
    public ImageTrackingObjectManager m_ImageTrackingObjectManager;
    public Text text;
    public Text textProgress;
    public string url;
    public InputField inp;
    // Start is called before the first frame update
    void Start()
    {

        url += inp.text;
        //foreach (var g in m_ImageLibrary) 
        //{
        //    Debug.Log(g.name + (g.texture!=null));
        //    img.texture = g.texture;
        //}


        text.text = "startload";
        m_LoadAssetsBundle.Initz(url, (r)=> {


            text.text = "Initz";

            m_LoadAssetsBundle.LoadAsset(url, "one.png",(obj,err)=> {


                img.texture = (Texture)obj;



            });

            m_LoadAssetsBundle.LoadAsset(url, "ReferenceImageLibrary.asset", (obj,err) => {

                

                m_ImageLibrary = (XRReferenceImageLibrary)obj;
                text.text = $"doneload   {m_ImageLibrary != null}   {err}";
                m_TrackedImageManager.referenceLibrary = m_ImageLibrary;
                m_TrackedImageManager.gameObject.SetActive(true);

                m_ImageTrackingObjectManager.ImageLibrary = m_ImageLibrary;
                m_ImageTrackingObjectManager.enabled = true;


            });
            

        });





    }

    // Update is called once per frame
    void Update()
    {
        textProgress.text = ((int)(m_LoadAssetsBundle.progress*100)).ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;

public class SnapshotController : MonoBehaviour
{
    [Header("Photo Taker")]
    private Camera myCamera;
    private static SnapshotController instance;
    private bool takeScreenshotOnNextFrame;
    int screenshotId = 0;
    public static int photosTakenQuantity = 0;
    Texture2D renderResult;
    public static Photo newPhoto;

    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject polaroidUI;
    private bool viewingPhoto;
    RenderTexture renderTexture;

    [HideInInspector]
    public static bool canTakePicture;
    Rect rect;

    RaycastHit hit;
    [SerializeField] private float range;

    private Organism targetedOrganism;


    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void Start()
    {
        polaroidUI.SetActive(true);
        newPhoto = new Photo();
        EventManager.instance.ShowPolaroidUI += ShowPolaroidUI;

    }

    private void OnDestroy()
    {
        EventManager.instance.ShowPolaroidUI -= ShowPolaroidUI;
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnNextFrame)
        {
            takeScreenshotOnNextFrame = false;
            renderTexture = myCamera.targetTexture;

            renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            //byte[] byteArray = renderResult.EncodeToPNG();
            //System.IO.File.WriteAllBytes(Application.dataPath + "/Pictures/" + screenshotId.ToString() + "CameraScreenshot.png", byteArray);
            //Debug.Log("Saved CameraScreenshot.png");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;

            //AssetDatabase.Refresh();
            renderResult.Apply();


            if (Physics.Raycast(myCamera.gameObject.transform.position, myCamera.gameObject.transform.forward, out hit, range))
            {
                if (hit.transform.tag == "Animal")
                {
                    Debug.Log("sacamos foto a un animal");
                    Organism animal = hit.transform.gameObject.GetComponent<Organism>();
                    newPhoto.id = screenshotId;
                    newPhoto.animalType = animal.organismType;
                    newPhoto.animalName = animal.organismName;
                    newPhoto.infoId = animal.infoId;
                    newPhoto.picture = renderResult;
                }
                else
                {
                    Debug.Log("sacamos foto a algo que no es animal x");
                    newPhoto.id = screenshotId;
                    newPhoto.animalType = Organism.Type.typeGeneric;
                    newPhoto.animalName = "newPhoto_" + screenshotId.ToString();
                    newPhoto.infoId = "no_id";
                    newPhoto.picture = renderResult;
                }
            }
            else {
                Debug.Log("sacamos foto a algo que no está en el rango de la cámara");
                newPhoto.id = screenshotId;
                newPhoto.animalType = Organism.Type.typeGeneric;
                newPhoto.animalName = "newPhoto_" + screenshotId.ToString();
                newPhoto.infoId = "no_id";
                newPhoto.picture = renderResult;
            }
            
            screenshotId++;
            photosTakenQuantity++;

            //renderResult.LoadImage(byteArray);

            //Texture2D tex = new Texture2D(500, 500, TextureFormat.ARGB32, false);
            //tex.LoadRawTextureData(byteArray);
            //tex.Apply();

            ShowPhoto(newPhoto.picture);
            EventManager.instance.OnTakePicture();
            
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(myCamera.gameObject.transform.position, forward, Color.green);

        if (Physics.Raycast(myCamera.gameObject.transform.position, myCamera.gameObject.transform.forward, out hit, range))
        {
            if (hit.transform.tag == "Animal")
            {
                if (targetedOrganism == null && !viewingPhoto)
                {
                    targetedOrganism = hit.transform.gameObject.GetComponent<Organism>();
                    targetedOrganism.organismNameUI.SetActive(true);
                }
            }
        }
        else {

            if (targetedOrganism != null)
            {
                targetedOrganism.organismNameUI.SetActive(false);
                targetedOrganism = null;
            }
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!viewingPhoto)
            {
                if (photosTakenQuantity < 15) {
                    //Take Screenshot
                    polaroidUI.SetActive(false);
                    viewingPhoto = true;
                    TakeScreenshot_Static(1024, 576);
                }                
            }
            else
            {
                polaroidUI.SetActive(true);
                RemovePhoto();
            }
        }
    }

    void ShowPhoto(Texture2D photo)
    {
        Sprite photoSprite = Sprite.Create(photo, new Rect(0.0f, 0.0f, photo.width, photo.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        photoFrame.SetActive(true);

    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);       
    }

    public void ShowPolaroidUI(bool show) {
        if (show)
        {
            polaroidUI.SetActive(true);
        }
        else {
            polaroidUI.SetActive(false);
        }
    }
}

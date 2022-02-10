using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
    private float originalRange;

    private Organism targetedOrganism;

    public FirstPersonController fpsController;

    public Image flashImage;


    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void Start()
    {
        DOTween.Init();
        polaroidUI.SetActive(true);
        newPhoto = new Photo();
        originalRange = range;
        EventManager.instance.ShowPolaroidUI += ShowPolaroidUI;
        EventManager.instance.AddCameraZoom += AddCameraRange;


    }

    private void OnDestroy()
    {
        EventManager.instance.ShowPolaroidUI -= ShowPolaroidUI;
        EventManager.instance.AddCameraZoom -= AddCameraRange;
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
                    Organism animal = hit.transform.gameObject.GetComponent<Organism>();
                    newPhoto.id = screenshotId;
                    newPhoto.animalName = animal.animalName;
                    newPhoto.animalName = animal.animalName;
                    newPhoto.infoId = animal.infoId;
                    newPhoto.picture = renderResult;
                }
                else
                {
                    newPhoto.id = screenshotId;
                    newPhoto.animalName = Organism.AnimalName.typeGeneric;
                    newPhoto.infoId = "no_id";
                    newPhoto.picture = renderResult;
                }

            }
            else {
                newPhoto.id = screenshotId;
                newPhoto.animalName = Organism.AnimalName.typeGeneric;
                newPhoto.infoId = "no_id";
                newPhoto.picture = renderResult;
            }
            
            screenshotId++;
            photosTakenQuantity++;

            //renderResult.LoadImage(byteArray);

            //Texture2D tex = new Texture2D(500, 500, TextureFormat.ARGB32, false);
            //tex.LoadRawTextureData(byteArray);
            //tex.Apply();

            //Camera flash
            flashImage.DOColor(Color.white, 0.15f).SetLoops(2, LoopType.Yoyo).OnComplete(() => { ShowPhoto(newPhoto.picture); });
            
            EventManager.instance.OnTakePicture();
            StartCoroutine(WaitForPolaroidClose(1.5f));
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
            else {
                if (targetedOrganism != null)
                {
                    targetedOrganism.organismNameUI.SetActive(false);
                    targetedOrganism = null;
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
                    polaroidUI.SetActive(false);
                    viewingPhoto = true;
                    TakeScreenshot_Static(1024, 576);
                }
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

    IEnumerator WaitForPolaroidClose(float waitTime) {
        
        fpsController.cameraCanMove = false;
        fpsController.playerCanMove = false;
        EventManager.instance.OnShowAnimalNames(false);

        yield return new WaitForSeconds(waitTime);
        
        fpsController.cameraCanMove = true;
        fpsController.playerCanMove = true;
        polaroidUI.SetActive(true);
        EventManager.instance.OnShowAnimalNames(true);
        RemovePhoto();
    }

    public void AddCameraRange(bool isZoomed) {

        if (isZoomed)
        {
            range = originalRange * 1.5f;
        }
        else {
            range = originalRange;
        }
    }
}

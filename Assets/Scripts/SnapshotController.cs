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
    public Camera photoCamera;
    private static SnapshotController instance;
    private bool takeScreenshotOnNextFrame;
    [HideInInspector]
    public int screenshotId = 0;
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

    private OrganismObject targetedOrganism;
    private OrganismObject currentOrganism;


    public FirstPersonController fpsController;

    public Image flashImage;

    public RenderTexture rTex;


    private void Awake()
    {
        instance = this;
        myCamera = this.gameObject.GetComponent<Camera>();
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

            //renderTexture = photoCamera.targetTexture;
            //Debug.Log("renderTexture.width: " + renderTexture.width + " - " + "renderTexture.height: " + renderTexture.height);
            //renderResult = new Texture2D(renderTexture.width/2, renderTexture.height/2, TextureFormat.RGB24, false);
            //rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            //renderResult.ReadPixels(rect, 0, 0);



            renderResult = new Texture2D(500, 500, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = rTex;
            renderResult.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);

            RenderTexture.ReleaseTemporary(renderTexture);
            //photoCamera.targetTexture = null;

            //AssetDatabase.Refresh();
            renderResult.Apply();


            if (Physics.Raycast(photoCamera.gameObject.transform.position, photoCamera.gameObject.transform.forward, out hit, range))
            {
                if (hit.transform.tag == "Animal")
                {
                    
                    OrganismObject animal = hit.transform.gameObject.GetComponent<OrganismObject>();
                    newPhoto.id = screenshotId;
                    newPhoto.photoAnimalName = animal.animalName;
                    newPhoto.photoAnimalType = animal.animalType;
                    newPhoto.infoId = animal.infoId;
                    newPhoto.picture = renderResult;
                }
                else
                {
                    newPhoto.id = screenshotId;
                    newPhoto.photoAnimalName = OrganismObject.AnimalName.typeGeneric;
                    newPhoto.photoAnimalType = OrganismObject.AnimalType.typeGeneric;
                    newPhoto.infoId = "no_id";
                    newPhoto.picture = renderResult;
                }

            }
            else {
                newPhoto.id = screenshotId;
                newPhoto.photoAnimalName = OrganismObject.AnimalName.typeGeneric;
                newPhoto.photoAnimalType = OrganismObject.AnimalType.typeGeneric;
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
            flashImage.DOColor(Color.white, 0.15f).OnComplete(() => { ShowPhoto(newPhoto.picture); });

            if (SoundSfxController.instance != null) {
                SoundSfxController.instance.PlayCameraShutter();
            }            
            
            EventManager.instance.OnTakePicture();
            StartCoroutine(WaitForPolaroidClose(1.5f));
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        //photoCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
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
                //Debug.Log("Targeteamos a un animal");
                if (targetedOrganism == null && !viewingPhoto)
                {
                    Debug.Log("Encontramos al anteloper");
                    currentOrganism = targetedOrganism;
                    targetedOrganism = hit.transform.gameObject.GetComponent<OrganismObject>();
                    targetedOrganism.organismNameUI.transform.gameObject.SetActive(true);
                }
                else if (targetedOrganism != null && !viewingPhoto ) {
                    //if the ray didn't exit a collider but checks another kind of animal
                    if (currentOrganism != targetedOrganism) {
                        targetedOrganism.organismNameUI.transform.gameObject.SetActive(false);
                        targetedOrganism = hit.transform.gameObject.GetComponent<OrganismObject>();
                        targetedOrganism.organismNameUI.transform.gameObject.SetActive(true);
                    }
                }
            }
            else {
                if (targetedOrganism != null)
                {
                    targetedOrganism.organismNameUI.transform.gameObject.SetActive(false);
                    targetedOrganism = null;
                }
            }
        }
        else {

            if (targetedOrganism != null)
            {
                targetedOrganism.organismNameUI.transform.gameObject.SetActive(false);
                targetedOrganism = null;
            }
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!viewingPhoto)
            {
                if (photosTakenQuantity < Constants.TOTAL_PHOTO_SLOTS) {
                    polaroidUI.SetActive(false);
                    viewingPhoto = true;
                    TakeScreenshot_Static(1024, 576);
                }
            }
        }
    }

    void ShowPhoto(Texture2D photo)
    {
        Color transparentColor = new Color(1,1,1,0);
        flashImage.DOColor(transparentColor, 0.05f);

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

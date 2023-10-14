using DG.Tweening;
using LoLSDK;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public string nextLevel = "TutorialLevel";

    public GameObject CatHeadSilhouette;

    public GameObject startButton;

    public GameObject continueButton;

    private void Start() {
        StartButtonAnimation();
        ContinueButtonHandler();
    }


    public void GoToTutorialLevel() {

        SaveDataNewGame();

        SoundSfxController.instance.PlayClick();

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(CatHeadSilhouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => {
            SceneManager.LoadScene(nextLevel);
        });
    }

    public void StartButtonAnimation() {

        startButton.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetLoops(-1, LoopType.Yoyo);

    }

    public void SaveDataNewGame() {
        LOLSDK.Instance.SubmitProgress(0, Loader.PROGRESS_DATA.currentProgress, Loader.PROGRESS_DATA.maxProgress);
        Loader.SaveData();
    }

    void ContinueButtonHandler() {


        //var localStorage = new LocalStorageManager();
        //string serializedDataString = localStorage.LoadData("saved_photos");


        //if (serializedDataString != "empty") {

        //    Debug.Log("Data is not empty");

        //    SerializedProgressData deserializedData = JsonUtility.FromJson<SerializedProgressData>(serializedDataString);

        //    if (deserializedData != null) {


        //        Debug.Log("DeserializedData is not null");
        //        Debug.Log("El nivel es: " + deserializedData.currentLevel);
        //        Debug.Log("El current progress es: " + deserializedData.currentProgress);


        //        if (deserializedData.currentLevel == "" || deserializedData.currentLevel == null)
        //            return;

        //        if (deserializedData.currentLevel != "TutorialLevel" || deserializedData.currentLevel != "EndGameLevel") {

        //            Loader.LoadLoader(deserializedData);
        //            Debug.Log("Guaranteed");

        //            //if (deserializedData.currentLevel == "SavannaLevel" || deserializedData.currentLevel == "ForestLevel") {
        //            //    Debug.Log("It was from the savannah level");
        //            //    Loader.PROGRESS_DATA.photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];
        //            //}

        //            continueButton.SetActive(true);
        //        }

        //    }
        //}
        //else {
        //    Debug.Log("Data is empty");
        //}




        LOLSDK.Instance.LoadState<SerializedProgressData>(state => {
            if (state != null) {

                if (state.data.currentLevel == "" || state.data.currentLevel == null)
                    return;

                if (state.data.currentLevel != "TutorialLevel" || state.data.currentLevel != "EndGameLevel") {

                    Loader.LoadLoader(state.data);
                    Debug.Log("Current level at start is: " + Loader.PROGRESS_DATA.currentLevel);

                    if (state.data.currentLevel == "SavannaLevel" || state.data.currentLevel == "ForestLevel") {
                        Loader.PROGRESS_DATA.photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];
                    }

                    continueButton.SetActive(true);
                }
            }

        });
    }

    public void ContinueGame() {
        
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(CatHeadSilhouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => {
           
            LOLSDK.Instance.SubmitProgress(
                Loader.PROGRESS_DATA.currentProgress, 
                Loader.PROGRESS_DATA.currentProgress, 
                Loader.PROGRESS_DATA.maxProgress);

            SceneManager.LoadScene(Loader.PROGRESS_DATA.currentLevel);
        });

    }

}

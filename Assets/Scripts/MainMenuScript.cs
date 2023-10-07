using DG.Tweening;
using LoLSDK;
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
        LOLSDK.Instance.LoadState<SerializedProgressData>(state => {
            if (state != null) {

                if (state.data.currentLevel == "" || state.data.currentLevel == null)
                    return;               

                if (state.data.currentLevel != "TutorialLevel" || state.data.currentLevel != "EndGameLevel") {
                                       
                    Loader.LoadLoader(state.data);

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

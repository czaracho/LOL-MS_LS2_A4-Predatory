using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public string nextLevel = "TutorialLevel";

    public GameObject CatHeadSilhouette;

    public GameObject startButton;

    private void Start() {
        StartButtonAnimation();
    }


    public void GoToTutorialLevel() {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(CatHeadSilhouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => {
            SceneManager.LoadScene(nextLevel);
        });
    }

    public void StartButtonAnimation() {

        startButton.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetLoops(-1, LoopType.Yoyo);

    }

}

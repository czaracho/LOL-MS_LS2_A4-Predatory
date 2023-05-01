using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public string nextLevel = "SavannaLevel";

    public GameObject CatHeadSilhouette;
    public GameObject StartText;

    private void Start() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(StartText.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 1f)).SetLoops(-1, LoopType.Yoyo);
    }


    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            TransitionToNextLevel();
        }

    }

    private void TransitionToNextLevel() {
        
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(CatHeadSilhouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => {
            SceneManager.LoadScene(nextLevel);
        });
    }

}

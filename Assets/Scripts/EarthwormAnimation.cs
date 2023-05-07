using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthwormAnimation : MonoBehaviour
{
    public GameObject wormHead1;
    public GameObject wormHead2;
    public GameObject wormHead3;

    private void Start() {
        WormAnimation();
    }

    private void WormAnimation() {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(wormHead1.transform.DOScale(new Vector3(1.2f, 0.9f, 1), 0.8f)).SetLoops(-1, LoopType.Yoyo);
        mySequence.Join( wormHead2.transform.DOScale(new Vector3(1.1f, 2.3f, 1.2f), 1.1f)).SetLoops(-1, LoopType.Yoyo);
        mySequence.Join( wormHead3.transform.DOScale(new Vector3(1.3f, 0.5f, 1.1f), 1.0f)).SetLoops(-1, LoopType.Yoyo);
    }
}

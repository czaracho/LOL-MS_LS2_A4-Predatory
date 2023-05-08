using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public TextTrigger finalDialog;

    public void Start() {
        LOLSDK.Instance.CompleteGame();
    }
}

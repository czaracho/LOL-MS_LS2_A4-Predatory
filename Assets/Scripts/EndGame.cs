using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void Start() {
        Debug.Log("Meowsnap game completed");
        LOLSDK.Instance.CompleteGame();
    }
}

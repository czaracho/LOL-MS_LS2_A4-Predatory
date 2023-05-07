using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfxController : MonoBehaviour
{

    int counterSound = 0;

    public void PlayeStepSound() {


        if (GameManagerScript.instance.isFieldLevel) {
            SoundSfxController.instance.PlayStepGrass();
        }
        else {
            SoundSfxController.instance.PlayStepWood();
        }

        counterSound++;
        Debug.Log("CurrentSound counter: " + counterSound);
    }

}

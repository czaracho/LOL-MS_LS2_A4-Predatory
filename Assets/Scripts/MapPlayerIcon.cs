using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapPlayerIcon : MonoBehaviour
{
    public GameObject player;

    public void Update() {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.rotation = Quaternion.Euler(-90, 180 + player.transform.eulerAngles.y, 0);
    }
}

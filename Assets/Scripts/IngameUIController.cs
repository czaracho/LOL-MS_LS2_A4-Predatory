using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : MonoBehaviour
{
    public GameObject IngameUI;
    [HideInInspector]
    public bool albumIsOpen = false;
    public static IngameUIController instance;
    public List<GameObject> animalNamesUI = new List<GameObject>();
    public GameObject notificationBlock;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        EventManager.instance.ShowIngameUI += ShowIngameUI;
    }

    private void OnDestroy()
    {
        EventManager.instance.ShowIngameUI -= ShowIngameUI;
    }

    public void ShowIngameUI(bool show) {
        
    }

    public void HideAnimalNames() { 
        
    }

    public void ShowNotificationText() {
        notificationBlock.gameObject.SetActive(true);
    }

    public void HideNotificationText() {
        notificationBlock.gameObject.SetActive(false);
    }


}

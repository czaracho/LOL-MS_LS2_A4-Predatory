using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIController : MonoBehaviour
{
    public GameObject IngameUI;
    [HideInInspector]
    public bool albumIsOpen = false;
    public static IngameUIController instance;
    public List<GameObject> animalNamesUI = new List<GameObject>();


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
        IngameUI.SetActive(show);
    }

    public void HideAnimalNames() { 
        
    }
}

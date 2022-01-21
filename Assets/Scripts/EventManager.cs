using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

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

    public event Action TakePicture;
    public event Action<bool> ShowPolaroidUI;
    public event Action<bool> ShowIngameUI;


    public void OnTakePicture()
    {
        TakePicture?.Invoke();
    }


    public void OnShowPolaroidUI(bool show)
    {
        ShowPolaroidUI?.Invoke(show);
    }

    public void OnShowIngameUI(bool show)
    {
        ShowIngameUI?.Invoke(show);
    }
}

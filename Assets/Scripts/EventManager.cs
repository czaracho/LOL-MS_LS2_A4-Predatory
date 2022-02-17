﻿using System;
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
    public event Action<bool> ShowPromptActionUI;
    public event Action<bool> AddCameraZoom;
    public event Action<bool> ShowAnimalNames;
    public event Action<string> SetGameAction;


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

    public void OnShowPromptActionUI (bool show) {
        ShowPromptActionUI?.Invoke(show);
    }

    public void OnAddCameraZoom(bool isZoomed) {
        AddCameraZoom?.Invoke(isZoomed);
    }

    public void OnShowAnimalNames(bool show) {
        ShowAnimalNames?.Invoke(show);
    }

    public void OnSetGameAction(string action) {
        SetGameAction?.Invoke(action);
    }
}

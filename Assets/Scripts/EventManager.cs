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
    public event Action<bool> ShowFpsUI;
    public event Action<bool> ShowPromptActionUI;
    public event Action<bool> AddCameraZoom;
    public event Action<bool> ShowAnimalNames;
    public event Action StartBoardInitialConversation;
    public event Action GoToBoard;
    public event Action<OrganismIdentifier> AddOrganismToBoardToReview;
    public event Action<OrganismIdentifier> RemoveOrganismFromBoardToReview;
    public event Action GenericAction;
    public event Action ResetNPCFirstDialogue;
    public event Action<Photo> ShowPhotoPreview;
    public event Action HidePhotoPreview;
    public event Action ShowIncorrectBoardDialogue;
    public event Action MakePlayerOnBoardStatus;


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

    public void OhShowFpsUI(bool show)
    {
        ShowFpsUI?.Invoke(show);
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

    public void OnStartBoardInitialConversation() {
        StartBoardInitialConversation?.Invoke();
    }

    public void OnGoToBoard() {
        GoToBoard?.Invoke();
    }

    public void OnAddOrganismToBoardToReview(OrganismIdentifier organism) { 
        AddOrganismToBoardToReview?.Invoke(organism);
    }

    public void OnRemoveOrganismFromBoardToReview(OrganismIdentifier organism) { 
        RemoveOrganismFromBoardToReview?.Invoke(organism);
    }

    public void OnGenericAction() {
        GenericAction?.Invoke();
    }

    public void OnResetNPCFirstDialogue() { 
        ResetNPCFirstDialogue?.Invoke();
    }

    public void OnShowPhotoPreview(Photo photo) {
        ShowPhotoPreview?.Invoke(photo);
    }

    public void OnHidePhotoPreview() {
        HidePhotoPreview?.Invoke();
    }

    public void OnShowIncorrectBoardDialogue() { 
        ShowIncorrectBoardDialogue?.Invoke();
    }

    public void OnMakePlayerOnBoardStatus() { 
        MakePlayerOnBoardStatus?.Invoke();
    }
}

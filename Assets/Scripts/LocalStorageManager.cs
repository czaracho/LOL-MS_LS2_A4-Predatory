using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LocalStorageManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string data);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void ClearLocalStorage();

    public void SaveData(string key, string data) {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            SaveToLocalStorage(key, data);
        }
        else {
            Debug.LogError("LocalStorage is available only in WebGL builds.");
        }
    }

    public string LoadData(string key) {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            return LoadFromLocalStorage(key);
        }
        else {
            Debug.LogError("LocalStorage is available only in WebGL builds.");
            return string.Empty;
        }
    }
}

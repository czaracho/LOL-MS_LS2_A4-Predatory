using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;
using SimpleJSON;
using System.IO;
using System;

public class ProgressData {
    public int currentProgress = 0;
    public int maxProgress = 10;
    public string currentLevel = "";
    public Photo[] photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];

    public ProgressData() {



        for (int i = 0; i < photoCollection.Length; i++) {
            photoCollection[i] = new Photo();
            photoCollection[i].photoIsSaved = false;
            photoCollection[i].photoAnimalName = OrganismObject.AnimalName.typeGeneric;
            photoCollection[i].photoAnimalType = OrganismObject.AnimalType.typeGeneric;
        }
    }

    public void LoadProgressData(SerializedProgressData serializedData) {         
        maxProgress = serializedData.maxProgress;
        currentLevel = serializedData.currentLevel;        

        switch (currentLevel) {
            case "TutorialLevel":
                currentProgress = 0; 
                break;
            case "SavannaLevel":
                currentProgress = serializedData.currentProgress;
                break;
            case "ForestLevel":
                currentProgress = serializedData.currentProgress;
                break;
            case "TentLevel1":
                currentLevel = "SavannaLevel";
                currentProgress = 0;
                break;
            case "TentLevel2":
                currentLevel = "ForestLevel";
                currentProgress = 4;    //The number of progress from the first tent level
                break;
            case "EndGameLevel":
                currentProgress = maxProgress;
                break;
            default:
                break;
        }

        //LoadSerializedPhotos(serializedData.photoCollection);

    }

    private void LoadSerializedPhotos(SerializablePhoto[] serializedPhotoCollection) {

        Debug.Log("Start of serialized photos deconverter");

        for (int i = 0; i < photoCollection.Length; i++) {


            Debug.Log("Animal name: " + serializedPhotoCollection[i].photoAnimalName);
            Debug.Log("Serialized photo: " + serializedPhotoCollection[i].picture);


            photoCollection[i].id = serializedPhotoCollection[i].id;
            photoCollection[i].photoAnimalName = serializedPhotoCollection[i].photoAnimalName;
            photoCollection[i].photoAnimalType = serializedPhotoCollection[i].photoAnimalType;
            photoCollection[i].photoAnimalNameAdditional = serializedPhotoCollection[i].photoAnimalNameAdditional;
            photoCollection[i].infoId = serializedPhotoCollection[i].infoId;
            //photoCollection[i].picture = serializedPhotoCollection[i].picture;
            if (photoCollection[i].picture != null) {
                photoCollection[i].picture = Convert.FromBase64String(serializedPhotoCollection[i].picture);
            }
            else {
                photoCollection[i].picture = null;
            }
            
            photoCollection[i].photoIsSaved = serializedPhotoCollection[i].photoIsSaved;
            photoCollection[i].indexPhoto = serializedPhotoCollection[i].indexPhoto;
        }

        Debug.Log("End of of serialized photos deconverter");

    }
}

public class SerializedProgressData {
    public int currentProgress = 0;
    public int maxProgress = 10;
    public string currentLevel = "";
    public SerializablePhoto[] photoCollection = new SerializablePhoto[Constants.TOTAL_PHOTO_SLOTS];

    public SerializedProgressData() {
        for (int i = 0; i < photoCollection.Length; i++) {
            photoCollection[i] = new SerializablePhoto();
            photoCollection[i].photoIsSaved = false;
            photoCollection[i].photoAnimalName = OrganismObject.AnimalName.typeGeneric;
            photoCollection[i].photoAnimalType = OrganismObject.AnimalType.typeGeneric;
        }
    }

    public void LoadSerializedPhotos(Photo[] progressDataCollection) {


        Debug.Log("Start of LoadSerializedPhotos (saving)");
        


        for (int i = 0; i < progressDataCollection.Length; i++) {
            photoCollection[i].id = progressDataCollection[i].id;
            photoCollection[i].photoAnimalName = progressDataCollection[i].photoAnimalName;
            photoCollection[i].photoAnimalType = progressDataCollection[i].photoAnimalType;
            photoCollection[i].photoAnimalNameAdditional = progressDataCollection[i].photoAnimalNameAdditional;
            photoCollection[i].infoId = progressDataCollection[i].infoId;
            //photoCollection[i].picture = progressDataCollection[i].picture;
            
            if (progressDataCollection[i].picture == null) {
                photoCollection[i].picture = null;
            }
            else {
                photoCollection[i].picture = Convert.ToBase64String(progressDataCollection[i].picture);
            }
            
            photoCollection[i].photoIsSaved = progressDataCollection[i].photoIsSaved;
            photoCollection[i].indexPhoto = progressDataCollection[i].indexPhoto;
        }

        Debug.Log("first animal name is: " + photoCollection[0].photoAnimalName);
        Debug.Log("first animal picture is is: " + photoCollection[0].picture);
        Debug.Log("End of LoadSerializedPhotos");


        //LOLSDK.Instance.SaveState(this);
        //var localStorage = new LocalStorageManager();
        //SerializedProgressData myData = this;  // This could be your base64 encoded or compressed image data
        //string myData = JsonUtility.ToJson(this);
        //localStorage.SaveData("saved_photos", myData);
        Debug.Log("Data has been saved!");
    }

    public void SaveSerializedData() {

        maxProgress = Loader.PROGRESS_DATA.maxProgress;
        currentProgress = Loader.PROGRESS_DATA.currentProgress;
        currentLevel = Loader.PROGRESS_DATA.currentLevel;

        Debug.Log("Saved current level is: " + currentLevel);
        Debug.Log("Saved the basic stuff");
        LOLSDK.Instance.SaveState(this);




        //LoadSerializedPhotos(Loader.PROGRESS_DATA.photoCollection);


        //try {
        //    LoadSerializedPhotos(Loader.PROGRESS_DATA.photoCollection);
        //}
        //catch {
        //    Debug.Log("Error trying to save serialized data");
        //}

    }
}

public class Loader : MonoBehaviour
{
    public static ProgressData PROGRESS_DATA = new ProgressData();

    // Relative to Assets /StreamingAssets/
    private const string languageJSONFilePath = "language.json";
    private const string questionsJSONFilePath = "questions.json";
    private const string startGameJSONFilePath = "startGame.json";

    
    // Use to determine when all data is preset to load to next state.
    // This will protect against async request race conditions in webgl.
    LoLDataType _receivedData;

    // This should represent the data you're expecting from the platform.
    // Most games are expecting 2 types of data, Start and Language.
    LoLDataType _expectedData = LoLDataType.START | LoLDataType.LANGUAGE;

    [System.Flags]
    enum LoLDataType
    {
        START = 0,
        LANGUAGE = 1 << 0,
        QUESTIONS = 1 << 1
    }

    void Awake ()
    {
        // Create the WebGL (or mock) object
#if UNITY_EDITOR
		    ILOLSDK webGL = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
        ILOLSDK webGL = new LoLSDK.WebGL();
#endif

        // Initialize the object, passing in the WebGL
        LOLSDK.Init(webGL, "com.legends-of-learning.unity.sdk.v5.3.meowsnap");

        // Register event handlers
        LOLSDK.Instance.StartGameReceived += new StartGameReceivedHandler(HandleStartGame);
        LOLSDK.Instance.LanguageDefsReceived += new LanguageDefsReceivedHandler(HandleLanguageDefs);
        LOLSDK.Instance.QuestionsReceived += new QuestionListReceivedHandler(HandleQuestions);
        LOLSDK.Instance.GameStateChanged += new GameStateChangedHandler(HandleGameStateChange);

        // Mock the platform-to-game messages when in the Unity editor.
#if UNITY_EDITOR
			LoadMockData();
#endif

        // Then, tell the platform the game is ready.
        LOLSDK.Instance.GameIsReady();
        StartCoroutine(_WaitForData());
    }

    IEnumerator _WaitForData ()
    {
        yield return new WaitUntil(() => (_receivedData & _expectedData) != 0);
        SceneManager.LoadScene("MainMenuLevel", LoadSceneMode.Single);
    }

    // Start the game here
    void HandleStartGame (string json)
    {
        SharedState.StartGameData = JSON.Parse(json);
        _receivedData |= LoLDataType.START;
    }


    // Use language to populate UI
    void HandleLanguageDefs (string json)
    {
        JSONNode langDefs = JSON.Parse(json);

        // Example of accessing language strings
        // Debug.Log(langDefs);
        // Debug.Log(langDefs["welcome"]);

        SharedState.LanguageDefs = langDefs;
        _receivedData |= LoLDataType.LANGUAGE;
    }

    // Store the questions and show them in order based on your game flow.
    void HandleQuestions (MultipleChoiceQuestionList questionList)
    {
        Debug.Log("HandleQuestions");
        SharedState.QuestionList = questionList;
        _receivedData |= LoLDataType.QUESTIONS;
    }

    // Handle pause / resume
    void HandleGameStateChange (GameState gameState)
    {
        // Either GameState.Paused or GameState.Resumed
        Debug.Log("HandleGameStateChange");
    }

    public static void SaveData() {

        Debug.Log("Chequeamos si el progress data es null o no");

        //if (PROGRESS_DATA == null) {
        //    Debug.Log("El PROGRESS_DATA es null");
        //}
        //else {
        //    Debug.Log("El PROGRESS_DATA NO es null");
        //    Debug.Log("Progress data current progress: " + PROGRESS_DATA.currentProgress);
        //    Debug.Log("Progress data max progress: " + PROGRESS_DATA.maxProgress);
        //    Debug.Log("Verificamos si el data photoCollection se inicializó: ");
        //    Debug.Log("PROGRESS_DATA.photoCollection.Length: " + PROGRESS_DATA.photoCollection.Length);
        //    Debug.Log("PROGRESS_DATA.photoCollection[0].photoAnimalName: " + PROGRESS_DATA.photoCollection[0].photoAnimalName);
        //}

        SerializedProgressData serializedProgress = new SerializedProgressData();
        serializedProgress.SaveSerializedData();

    }

    private void LoadMockData ()
    {
#if UNITY_EDITOR
			// Load Dev Language File from StreamingAssets

			string startDataFilePath = Path.Combine (Application.streamingAssetsPath, startGameJSONFilePath);
			string langCode = "en";

			Debug.Log(File.Exists (startDataFilePath));

			if (File.Exists (startDataFilePath))  {
				string startDataAsJSON = File.ReadAllText (startDataFilePath);
				JSONNode startGamePayload = JSON.Parse(startDataAsJSON);
				// Capture the language code from the start payload. Use this to switch fonts
				langCode = startGamePayload["languageCode"];
				HandleStartGame(startDataAsJSON);
			}

			// Load Dev Language File from StreamingAssets
			string langFilePath = Path.Combine (Application.streamingAssetsPath, languageJSONFilePath);
			if (File.Exists (langFilePath))  {
				string langDataAsJson = File.ReadAllText (langFilePath);
				// The dev payload in language.json includes all languages.
				// Parse this file as JSON, encode, and stringify to mock
				// the platform payload, which includes only a single language.
				JSONNode langDefs = JSON.Parse(langDataAsJson);
				// use the languageCode from startGame.json captured above
				HandleLanguageDefs(langDefs[langCode].ToString());
			}

			// Load Dev Questions from StreamingAssets
			string questionsFilePath = Path.Combine (Application.streamingAssetsPath, questionsJSONFilePath);
			if (File.Exists (questionsFilePath))  {
				string questionsDataAsJson = File.ReadAllText (questionsFilePath);
				MultipleChoiceQuestionList qs =
					MultipleChoiceQuestionList.CreateFromJSON(questionsDataAsJson);
				HandleQuestions(qs);
			}
#endif
    }

    public static void LoadLoader(SerializedProgressData savedData) {      
        PROGRESS_DATA.LoadProgressData(savedData);
    }


}

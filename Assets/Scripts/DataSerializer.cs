using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSerializer {

    //public static string SerializeProgressData(ProgressData data) {
    //    SerializableProgressData serializableData = new SerializableProgressData {
    //        currentProgress = data.currentProgress,
    //        maxProgress = data.maxProgress,
    //        currentLevel = data.currentLevel,
    //        photoCollection = new SerializablePhoto[Constants.TOTAL_PHOTO_SLOTS]
    //    };

    //    for (int i = 0; i < data.photoCollection.Length; i++) {
    //        serializableData.photoCollection[i] = ConvertPhotoToSerializablePhoto(data.photoCollection[i]);
    //    }

    //    return JsonUtility.ToJson(serializableData);
    //}

    //private static SerializablePhoto ConvertPhotoToSerializablePhoto(Photo photo) {
    //    SerializablePhoto serializablePhoto = new SerializablePhoto {
    //        id = photo.id,
    //        photoAnimalName = photo.photoAnimalName,
    //        photoAnimalType = photo.photoAnimalType,
    //        photoAnimalNameAdditional = photo.photoAnimalNameAdditional,
    //        infoId = photo.infoId,
    //        picture = Convert.ToBase64String(photo.picture), // Convert byte array to base64 string
    //        photoIsSaved = photo.photoIsSaved,
    //        indexPhoto = photo.indexPhoto
    //    };

    //    return serializablePhoto;
    //}
}

//[Serializable]
//public class SerializableProgressData {
//    public int currentProgress;
//    public int maxProgress;
//    public string currentLevel;
//    public SerializablePhoto[] photoCollection;
//}

//[Serializable]
//public class SerializablePhoto {
//    public int id;
//    public OrganismObject.AnimalName photoAnimalName;
//    public OrganismObject.AnimalType photoAnimalType;
//    public OrganismObject.AnimalName photoAnimalNameAdditional;
//    public string infoId;
//    public string picture;  // This is now a base64 encoded string
//    public bool photoIsSaved;
//    public int indexPhoto = 999;
//}

using UnityEngine;
using System.Collections;
using System;
using System.IO;
public class GeneralShare
{

    public void shareText(string title, string text)
    {
#if UNITY_EDITOR
#elif UNITY_IOS
        GeneralSharingiOSBridge.ShareSimpleText (text);
#elif UNITY_ANDROID
        ShareAndroidText (title, text);
#endif
    }

    public void shareImageWithText(string title, string text, Texture2D _textureShareImage)
    {
#if UNITY_EDITOR
#elif (UNITY_IOS || UNITY_ANDROID)
        // Save your image on designate path
        byte[] bytes = _textureShareImage.EncodeToPNG ();
        string path = Application.persistentDataPath + "/ShareImage.png";
        File.WriteAllBytes (path, bytes);
#endif

#if UNITY_EDITOR
#elif UNITY_IOS
        GeneralSharingiOSBridge.ShareTextWithImage (path, text);
#elif UNITY_ANDROID
        ShareAndroidImageWithText (path, title, text, _textureShareImage);
#endif
    }

    private void ShareAndroidText(string title, string text)
    {
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
        intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
        intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), title);
        intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TITLE"), title);
        intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), text);
        AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", jChooser);
#endif
    }

    private void ShareAndroidImageWithText(string path, string title, string text, Texture2D _textureShareImage)
    {
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
        intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject> ("setType", "image/*");
        intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), title);
        intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TITLE"), title);
        intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), text);
        AndroidJavaClass uriClass = new AndroidJavaClass ("android.net.Uri");
        AndroidJavaObject fileObject = new AndroidJavaObject ("java.io.File", path);    // Set Image Path Here
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject> ("fromFile", fileObject);
        bool fileExist = fileObject.Call<bool> ("exists");
        Debug.Log ("[GeneralShare] File exist : " + fileExist);
        Debug.Log ("[GeneralShare] file path : " + uriObject.Call<string> ("getPath"));
        // Attach image to intent
        if (fileExist)
            intentObject.Call<AndroidJavaObject> ("putExtra", intentClass.GetStatic<string> ("EXTRA_STREAM"), uriObject);
        AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
        currentActivity.Call("startActivity", intentObject);
#endif
    }
}
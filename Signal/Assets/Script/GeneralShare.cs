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

    private void ShareAndroidText(string title, string text)
    {
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), title);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), title);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intentObject);
#endif
    }

}
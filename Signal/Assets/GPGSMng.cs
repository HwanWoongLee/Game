using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using GooglePlayGames.BasicApi;

public class GPGSMng : MonoBehaviour
{
    public static GPGSMng gpgsInstance;

    private void Awake()
    {
        if (gpgsInstance == null)
            gpgsInstance = this;
    }

    void Start()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
#endif
        ConectarGoogle();
    }

    public void ConectarGoogle()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
            if (true == success)
            {
                Debug.Log("Login");
            }
            else
            {
                Debug.Log("Login Fail !!");
            }
        });
    }

    public void ShowBoard() //리더보드
    {
        Social.ShowLeaderboardUI();
    }

    public void ShowAchievement()//업적
    {
        Social.ShowAchievementsUI();
    }

    public void ReportScore(int score)
    {
#if UNITY_ANDROID
        Social.ReportScore(score, "CgkI3IC9vIEPEAIQAg", success =>
        {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
#elif UNITY_IOS
        Social.ReportScore(score, "rotangle_bestscroe", success =>
        {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
#endif
    }

    public void ReportStage(int stage)
    {
#if UNITY_ANDROID
        Social.ReportScore(stage, "CgkI3IC9vIEPEAIQDg", success =>
        {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
#elif UNITY_IOS
        Social.ReportScore(score, "rotangle_bestscroe", success =>
        {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
#endif
    }

    public void ReportProgress(string id)
    {
#if UNITY_ANDROID
        Social.ReportProgress(id, 100.0, result =>
        {
            if (result)
                Debug.Log("Successfully reported achievement progress");
            else
                Debug.Log("Failed to report achievement");
        });
#elif UNITY_IOS
#endif
    }
}
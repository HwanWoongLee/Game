using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api; // 구글 애드몹 API 네임 스페이스

public class Admob : MonoBehaviour
{
    BannerView mBannerView = null; // 배너 출력

    public string mBannerView_Key; // 배너 Key

    string SEE_YOUR_LOGCAT_TO_GET_YOUR_DEVICE_ID = "3EB94BF573AD95FB";


    void Awake() // 제일 먼저 호출되는 함수입니다.( Start 보다 빠릅니다 )
    {
        DontDestroyOnLoad(this.gameObject);
        // BannerView(애드몹 사이트에 등록된 아이디, 크기, 위치) / AdSize.SmartBanner : 화면 해상도에 맞게 늘임, AdPosition.Bottom : 화면 바닥에 붙음
        mBannerView = new BannerView(mBannerView_Key, AdSize.SmartBanner, AdPosition.Bottom);

        // 애드몹 리퀘스트 초기화
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
            .AddTestDevice(SEE_YOUR_LOGCAT_TO_GET_YOUR_DEVICE_ID)  // My test device.
            .Build();
        // 테스트 할 때는 테스트 디바이스로 등록을 해야한다고 합니다. 테스트를 상용으로 하면 광고가 안나올 수 도 있다고 하더군요.
        // AdRequest request = builder.Build(); <-- 실제 빌드시에는 이렇게 바꿔줍니다.

        // 애드몹 배너 광고를 로드합니다.
        mBannerView.LoadAd(request); //배너 광고 요청
    }

    void Start()
    {
        mBannerView.Show();  // 배너 광고 출력
    }

}
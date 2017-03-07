using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {
    float delayTime = 0f;


    private void Awake()
    {
        Screen.SetResolution(576 , 1024, true);
    }
    // Update is called once per frame
    void Update () {
        delayTime += Time.deltaTime;
        
        if(delayTime >= 1.9f)
        {
            Application.LoadLevel("MainScene");
        }	
	}
}

using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public GameObject[] tutorialScene = new GameObject[5];

    int sceneNum = 0;
    float dTime = 0f;
    bool play = false;

void Start()
{
    Time.timeScale = 1;
}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
			SoundManager.instance.PlayEffectSound(0);

            switch (sceneNum)
            {
                case 0:
                    tutorialScene[0].SetActive(false);
                    tutorialScene[1].SetActive(true);
                    tutorialScene[2].SetActive(false);
                    tutorialScene[3].SetActive(false);
                    tutorialScene[4].SetActive(false);

                    sceneNum++;
                    break;
                case 1:
                    tutorialScene[0].SetActive(false);
                    tutorialScene[1].SetActive(false);
                    tutorialScene[2].SetActive(true);
                    tutorialScene[3].SetActive(false);
                    tutorialScene[4].SetActive(false);

                    sceneNum++;
                    break;
                case 2:
                    tutorialScene[0].SetActive(false);
                    tutorialScene[1].SetActive(false);
                    tutorialScene[2].SetActive(false);
                    tutorialScene[3].SetActive(true);
                    tutorialScene[4].SetActive(false);

                    sceneNum++;
                    break;
                case 3:
                    tutorialScene[0].SetActive(false);
                    tutorialScene[1].SetActive(false);
                    tutorialScene[2].SetActive(false);
                    tutorialScene[3].SetActive(false);
                    tutorialScene[4].SetActive(true);

                    sceneNum++;
                    break;
                case 4:
                    play = true;
                    break;

            }
        }

        if (play)
        {
            dTime += Time.deltaTime;
            Fade.instance.curFadeState = Fade.FadeState._out;

            if (dTime >= .5f)
            {
				sceneNum = -1;
				play = false;
                dTime = 0f;
				PlayerPrefs.SetInt("TUTORIAL",1);
                Application.LoadLevel("GameScene");
            }
        }
    }
}
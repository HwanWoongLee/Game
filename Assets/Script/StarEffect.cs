using UnityEngine;
using System.Collections;

public class StarEffect : MonoBehaviour {
    public GameObject[] bullets;

    private void OnEnable()
    {
        for(int i=0;i<5;i++)
        {
            bullets[i].GetComponent<TrailRenderer>().startWidth = 0.3f;
        }
    }

    void Update () {
        this.transform.position = GameManager.instance.player.transform.position;

        for (int i = 0; i < 5; i++)
        {
            bullets[i].GetComponent<TrailRenderer>().startWidth += 0.002f;
        }
    }
}

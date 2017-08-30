using UnityEngine;
using System.Collections;

public class ItemObj : MonoBehaviour {

    float offTime = 0f;

    //플레리어와 거리
    private float DistanceToPlayer()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = this.transform.position;

        return Vector3.Distance(myPos, playerPos);

    }

    private void OnEnable()
    {
        offTime = 0f;
        if (this.transform.tag.Equals("PowerUpItem"))
        {
            this.GetComponent<TweenScale>().ResetToBeginning();
            this.GetComponent<TweenScale>().Play();        }
    }

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.game
            || GameManager.instance.curGameState == GameState.store)
        {
            //거리가 10이상이면 꺼줌.
            if (DistanceToPlayer() >= 15f)
            {
                this.gameObject.SetActive(false);
            }

            if(this.transform.tag.Equals("PowerUpItem"))
            {
                if(GameManager.instance.player.getMagnetItem)
                {
                    if( DistanceToPlayer() < 3f)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                                       GameManager.instance.player.transform.position,
                                                                       Time.deltaTime * 8f);
                    }
                }
            }
            if(this.transform.tag.Equals("Warning") 
                || this.transform.tag.Equals("Effect")
                || this.transform.tag.Equals("BobmEffect"))
            {
                offTime += Time.deltaTime;
                if (offTime >= 2.5f)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}

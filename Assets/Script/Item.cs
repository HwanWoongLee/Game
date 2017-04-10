using UnityEngine;
using System.Collections;

public class Item : Obj
{
    public GameObject get;

    private void Update()
    {
        if (moveStart)
        {
            MoveBlock(this.transform);
        }

        if (this.transform.position.y >= 4f)
        {
            if(this.transform.tag.Equals("Plus")){ Destroy(this.gameObject); }
            else if(this.transform.tag.Equals("Warp")) { BlockManager.instance.warpState = true; }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag.Equals("Ball") && this.transform.tag.Equals("Plus"))
        {
            SoundManager.instance.PlayEffectSound(2);

            GameManager.instance.AddBall();
            BallManager.instance.MakeBalls();

            GameObject Effect = Instantiate(get);
            Effect.GetComponent<TweenPosition>().from = this.transform.position;
            Effect.GetComponent<TweenPosition>().to = this.transform.position + Vector3.up;

            Destroy(this.gameObject);
        }
        if (col.transform.tag.Equals("Ball") && this.transform.tag.Equals("Warp"))
        {
            if (!col.transform.GetComponent<Ball>().potal
                && BlockManager.instance.warpOut.activeInHierarchy)
            {
                BlockManager.instance.warpState = true;

                SoundManager.instance.PlayEffectSound(9);

                col.transform.position = BlockManager.instance.warpOut.transform.position;

                col.transform.GetComponent<Ball>().potal = true;
            }
        }
    }
}

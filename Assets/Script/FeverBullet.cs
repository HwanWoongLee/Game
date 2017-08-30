using UnityEngine;
using System.Collections;

public class FeverBullet : MonoBehaviour {
    public Vector3 fireVec;
    private float moveSpeed;

    public TrailRenderer tRender;
    public Material[] mat;

    private void Start()
    {
        moveSpeed = 8f;   
    }
    

    private void OnEnable()
    {
        int rendomNum = Random.Range(0, 6);

        this.tRender.material = mat[rendomNum];
    }

    private void Update()
    {
        if (GameManager.instance.curGameState == GameState.game)
        {
            this.transform.position += fireVec * Time.deltaTime * moveSpeed;

            //거리가 10이상이면 꺼줌.
            if (DistanceToPlayer() >= 15f)
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    //플레리어와 거리
    private float DistanceToPlayer()
    {
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = this.transform.position;

        return Vector3.Distance(myPos, playerPos);
    }
}

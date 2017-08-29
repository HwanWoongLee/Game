using UnityEngine;
using System.Collections;

public class StarItem : MonoBehaviour
{
    public float rotSpeed;
    public float moveSpeed;
    public Player plaeyr;

    private Vector3 playerPos;
    private Vector3 distancePos;
    private Vector3 moveVec;

    bool aiming = false;

    //활성화시 조준
    private void OnEnable()
    {
        Invoke("Disable", 30f);
        if (!aiming)
        {
            playerPos = plaeyr.transform.position;
            distancePos = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0);

            moveVec = (playerPos + distancePos) - this.transform.position;
            moveVec.Normalize();

            aiming = true;
        }
    }

    private void OnDisable()
    {
        aiming = false;
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        this.transform.Rotate(new Vector3(0, 0, rotSpeed));

        if (aiming)
        {
            this.transform.position += moveVec * moveSpeed;
        }

        if(GameManager.instance.curGameState != GameState.game)
        {
            this.gameObject.SetActive(false);
        }
    }
}

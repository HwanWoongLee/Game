using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    bool moveState;
    Vector3 targetPos;

    //내리기.
    public void FallItem()
    {
        if (!moveState)
        {
            targetPos = transform.position + Vector3.down;
            moveState = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                                 targetPos,
                                                 2f * Time.deltaTime);
            if (transform.position == targetPos)
            {         
                moveState = false;
            }
        }
    }

    private void Update()
    {
        if(this.transform.position.y <= -4f)
        {
            this.gameObject.SetActive(false);
        }
    }

}

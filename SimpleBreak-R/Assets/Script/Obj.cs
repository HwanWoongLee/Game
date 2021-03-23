using UnityEngine;
using System.Collections;

public class Obj : MonoBehaviour
{
    private Vector3 movePos;

    public bool moveStart = false;
    private bool setState = false;
    private float moveSpeed = 3.0f;

    public void MoveBlock(Transform obj)
    {
        if (!setState)
        {
            SetPos(obj.position);
        }

        obj.position = Vector3.MoveTowards(obj.position, movePos, Time.deltaTime * moveSpeed);

        if (obj.transform.position == movePos)
        {
            obj.transform.position = movePos;
          
            moveStart = false;
            setState = false;
        }
    }

    public void SetPos(Vector3 curPosition)
    {
        movePos = curPosition + Vector3.up;

        setState = true;
    }
}

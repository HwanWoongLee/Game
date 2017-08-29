using UnityEngine;
using System.Collections;

public class ObjDestroy : MonoBehaviour {
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    public void OffThis()
    {
        this.gameObject.SetActive(false);
    }
}

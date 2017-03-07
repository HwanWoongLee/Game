using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    private void Start()
    {
        Destroy(this.gameObject, .6f);
    }

    private void delete()
    {
        Destroy(this.gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class HitPoint : MonoBehaviour {
    [SerializeField]
    GameObject tartget;
    Vector3 setPos;

    private void Start()
    {
        setPos = new Vector3(0f, 0f, -0.15f);
    }

    void Update () {
	    if(GameManager.instance.curGameState == GameState.game)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.transform.position = tartget.transform.position + setPos;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
	}
}

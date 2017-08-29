using UnityEngine;
using System.Collections;

public class StarEffect : MonoBehaviour {

	void Update () {
        this.transform.position = GameManager.instance.player.transform.position;	
	}
}

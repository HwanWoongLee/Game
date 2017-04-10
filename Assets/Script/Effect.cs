using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {
	void Start(){
		Destroy(this.gameObject, .5f);
	}
}

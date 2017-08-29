using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {

    void Update () {
	    if(Input.GetMouseButton(0))
        {
            Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray.origin,ray.direction * 10f,out hit))
            {
                if (hit.collider.transform.tag.Equals("PopUp"))
                {
                    this.GetComponent<TweenScale>().ResetToBeginning();
                    this.GetComponent<TweenScale>().Play();

                    this.gameObject.SetActive(false);
                }
            }
        }
	}
}

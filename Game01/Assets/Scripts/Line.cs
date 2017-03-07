using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Line : MonoBehaviour {

    public GameObject dotPrefab;
    private List<GameObject> dots = new List<GameObject>();
    private int dotNum = 30;

    float tTime = 0f;


	// Use this for initialization
	void Start () {
        for (int i=0; i < dotNum; i++)
        {          
            GameObject newDot = Instantiate(dotPrefab) as GameObject;
            newDot.transform.parent = this.transform;
            newDot.transform.position = this.transform.position + new Vector3(0f, 0f, 0f + i);     
            newDot.SetActive(false);

            if (newDot)
            {
                dots.Add(newDot);
            }
            else
            {
                Debug.Log("newDot is null");
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        tTime += Time.deltaTime;
        
        if (this.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < dotNum; i++)
            {
                dots[i].transform.localPosition += Vector3.forward * Time.deltaTime;
                if (dots[i].transform.localPosition.z >= 30.0f)
                {
                    dots[i].transform.localPosition = Vector3.zero;
                    dots[i].GetComponent<MeshRenderer>().enabled = true;
                }
                dots[i].SetActive(true);
            }
        }
    }
}

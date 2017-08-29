using UnityEngine;
using System.Collections;

public class GetBox : MonoBehaviour
{
    SpriteRenderer renderer;
    public Sprite[] image;

    public enum boxType
    {
        normal,
        big,
        laser,
        bounce,
        guided,
        sword,
        explosion,
        coin
    }

    public boxType thisBoxType;

    void Awake()
    {
        renderer = this.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (this.thisBoxType == boxType.coin)
        {
            SetImage(7);
            return;
        }
        int boxNum = Random.Range(0, 7);

        switch (boxNum)
        {
            case 0:
                thisBoxType = boxType.normal;
                break;
            case 1:
                thisBoxType = boxType.big;
                break;
            case 2:
                thisBoxType = boxType.laser;
                break;
            case 3:
                thisBoxType = boxType.bounce;
                break;
            case 4:
                thisBoxType = boxType.guided;
                break;
            case 5:
                thisBoxType = boxType.sword;
                break;
            case 6:
                thisBoxType = boxType.explosion;
                break;
        }

        SetImage(boxNum);
    }

    private void SetImage(int imageNum)
    {
        renderer.sprite = image[imageNum];

        this.GetComponent<TweenPosition>().from = this.transform.position;
        this.GetComponent<TweenPosition>().to = new Vector3(this.transform.position.x + Random.Range(-3f, 3f),
                                                            this.transform.position.y + Random.Range(-3f, 3f),
                                                            0f);
    }

    //Tween후에 써줌
    public void BoxOn()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }
}

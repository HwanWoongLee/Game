using UnityEngine;
using System.Collections;

public class GetEffect : MonoBehaviour
{
    public GameObject[] _Obj;
    public Sprite[] _sprite;

    public void OnEnable()
    {
        Destroy(this.gameObject, 2f);
    }

    public void SetSprite(int _sNum)
    {
        for (int i = 0; i < _Obj.Length; i++)
        {
            _Obj[i].GetComponent<SpriteRenderer>().sprite = _sprite[_sNum];
        }
    }
}

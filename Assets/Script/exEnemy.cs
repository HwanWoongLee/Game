using UnityEngine;
using System.Collections;

public class exEnemy : MonoBehaviour
{
    public GameObject damageLabel;

    private void OnTriggerEnter(Collider other)
    {
        //총알에 닿으면
        if (other.transform.tag.Equals("Bullet"))
        {
            //닿아도 사라지지 않는 총알들.
            if (BulletManager.instance.curBulletType != BulletManager.bulletType.laser
                && BulletManager.instance.curBulletType != BulletManager.bulletType.bounce
                && BulletManager.instance.curBulletType != BulletManager.bulletType.sword)
                other.gameObject.SetActive(false);


            //데미지 표시
            ShowDamage(GameManager.instance.player.Damage, this.transform.position);

        }

    }

    float curTime = 0;
    float delayTime = 1;

    private void OnTriggerStay(Collider other)
    {
        curTime += Time.deltaTime;

        if (curTime >= delayTime)
        {
            if (other.transform.tag.Equals("BobmEffect"))
            {
                //데미지 표시
                ShowDamage(GameManager.instance.player.Damage, this.transform.position);

                delayTime += 1;
            }
        }
    }

    //Damage표시.
    private void ShowDamage(int _damage, Vector3 pos)
    {
        GameObject newlabel = Instantiate(damageLabel);

        newlabel.transform.localScale = new Vector3(2, 2, 2);
        newlabel.transform.position = pos;

        newlabel.transform.FindChild("label").GetComponent<UILabel>().text = _damage.ToString();

        newlabel.GetComponent<TweenPosition>().from = pos;
        newlabel.GetComponent<TweenPosition>().to = pos + (Vector3.up * 2f);

        Destroy(newlabel, 1f);
    }
}

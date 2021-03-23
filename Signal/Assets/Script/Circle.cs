using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour
{
    public TouchManager touchManager;
    public int segments;
    public float radius;

    private float distance;
    private float limit;

    LineRenderer line;
    Player player;
    public Material mat;

    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        player = GameManager.instance.player;
        limit = 17f;
    }

    private void Update()
    {
        if(limit >= 6)
            limit = touchManager.limitDistance - 3f;
        radius = touchManager.limitDistance;
        CreatePoints();

        //보스 등장 전
        if (GameManager.instance.enemyRend)
        {
            //거리가 멀어지면 서서히 alpha값 증가
            if (DistanceToPlayer() <= limit)
            {
                mat.color = new Color(1, 1, 1, 0f);
            }
            else
            {
                mat.color = new Color(1, 1, 1, ((DistanceToPlayer() - limit) * 85f) / 255f);
            }
        }
        //보스 등장 시
        else
        {
            mat.color = Color.red;
        }
    }

    //원그리기
    void CreatePoints()
    {
        float x, y, z = 0;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }

    //플레이어 거리측정
    float DistanceToPlayer()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);

        return distance;
    }
}

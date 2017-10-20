using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

    public List<Transform> tileList = new List<Transform>();

    //맵 그려줌
    public void GenerateMap()
    {
        string holderName = "Generated Map";

        //이미 tile들이 있다면 지우고 다시 생성.
        if (transform.FindChild(holderName))
        {
            EnemyManager.instance.wayPoints.Clear();
            tileList.Clear();

            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + .5f + x, mapSize.y / 2 - .5f - y, 0);

                Transform newTile = Instantiate(tilePrefab);
                newTile.parent = mapHolder;
                newTile.transform.position = tilePosition;
                newTile.gameObject.SetActive(true);
                newTile.localScale = Vector3.one * (1 - outlinePercent);

                //맵 가장자리 부분은 way타입으로.
                if (x == 0
                    || y == 0
                    || x == mapSize.x - 1
                    || y == mapSize.y - 1)
                {
                    newTile.GetComponent<Tile>().SetTileType(Tile.TileType.way);
                }

                SetWayPoint(x, y, newTile);

                tileList.Add(newTile);
            }
        }
    }

    //지나가는 좌표 설정
    private void SetWayPoint(int _x, int _y, Transform _tile)
    {
        if (_x == 0 && _y == 0)
        {
            EnemyManager.instance.wayPoints.Add(_tile);
        }
        else if (_x == 0 && _y == mapSize.y - 1)
        {
            EnemyManager.instance.wayPoints.Add(_tile);
        }
        else if (_x == mapSize.x - 1 && _y == mapSize.y - 1)
        {
            EnemyManager.instance.wayPoints.Insert(2, _tile);
        }
        else if (_x == mapSize.x - 1 && _y == 0)
        {
            EnemyManager.instance.wayPoints.Add(_tile);
        }
    }

    //normal블록 찾아 Unit블록으로 변경
    public Vector3 GetTilePos()
    {
        int index = 0;
        while (true)
        {
            index = Random.Range(0, tileList.Count);

            if (tileList[index].GetComponent<Tile>().thisTileType == Tile.TileType.normal)
            {
                tileList[index].GetComponent<Tile>().SetTileType(Tile.TileType.unit);
                return tileList[index].position;
            }
        }
        return Vector3.zero;
    }
    public Vector3 GetTilePos(Vector3 _pos)
    {
        foreach (Transform tile in tileList)
        {
            if (tile.transform.position == _pos)
            {
                tile.GetComponent<Tile>().SetTileType(Tile.TileType.unit);
                return tile.position;
            }
        }
        return Vector3.zero;
    }

    //유닛타일의 유닛이 Off될때 호출
    public void TileUnitOff(Vector3 _unitPos)
    {
        foreach (Transform tile in tileList)
        {
            if (tile.transform.position == _unitPos)
            {
                tile.GetComponent<Tile>().SetTileType(Tile.TileType.normal);
            }
        }
    }
}

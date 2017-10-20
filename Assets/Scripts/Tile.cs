using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public enum TileType
    {
        normal,
        way,
        unit
    }
    
    public TileType thisTileType = TileType.normal;

    public Sprite[] tileImage;
    public SpriteRenderer tileRenderer;

    public BoxCollider thisBoxCol;


    //타일타입 지정.
    public void SetTileType(TileType _type)
    {
        if (this.thisTileType == _type)
            return;

        this.thisTileType = _type;

        switch (_type)
        {
            case TileType.normal:
                tileRenderer.sprite = tileImage[0];
                thisBoxCol.enabled = true;
                break;
            case TileType.way:
                tileRenderer.sprite = tileImage[1];
                thisBoxCol.enabled = false;
                break;
            case TileType.unit:
                tileRenderer.sprite = tileImage[0];
                thisBoxCol.enabled = false;
                break;
        }
    }
	
}


using UnityEngine;
using UnityEngine.Tilemaps;

public class TiledGrid : MonoBehaviour
{

    [SerializeField]
    private int rowCount = 9;
    [SerializeField]
    private int colCount = 8;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Color[] colors;


    private Tile[,] tiles;
    private float dragDistance;
    private Vector2 fp, lp;

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for(int x=0;x < colCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                Vector3Int pos = new Vector3Int(x,y,0);
                tilemap.SetTile(pos,CreateTile());
            }
        }
        tilemap.RefreshAllTiles();
        Vector3 size = tilemap.cellSize;
        transform.position = new Vector2(-colCount / 2 + size.x/2, -rowCount + size.y/2);
    }

    private Tile CreateTile()
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        int index = Random.Range(0, colors.Length - 1);
        tile.sprite = sprite;
        tile.color = colors[index];
        return tile;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lp = touch.position;
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    float swipeAngle = Mathf.Atan2(lp.y - fp.y, lp.x - fp.x) * 180 / Mathf.PI;
                    Debug.Log(swipeAngle);
                    if (swipeAngle > -45 && swipeAngle < 135)
                    {
                        Debug.Log("Clockwise!");
                    }
                    if (swipeAngle < -45 || swipeAngle > 135)
                    {
                        Debug.Log("CounterClockwise!");
                    }
                }
                else
                {
                    Debug.Log("Tap on Test Scene");
                    TileBase tile = tilemap.GetTile(new Vector3Int(0,0,0));
                    tilemap.SetTile(new Vector3Int(0, 0, 0), null);
                    tilemap.RefreshAllTiles();
                    tile = tilemap.GetTile(new Vector3Int(1, 1, 0));
                    tilemap.SetTile(new Vector3Int(2, 2, 0),tile);
                    tilemap.RefreshAllTiles();
                    tilemap.SetTile(new Vector3Int(3, 3, 0),tile);
                    tilemap.RefreshAllTiles();
                }
            }
        }
    }

    
}

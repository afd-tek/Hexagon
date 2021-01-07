using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    [SerializeField]
    public GameObject selectionDot;


    public GameObject instatiatedDot;
    [SerializeField]
    private int rowCount = 9;
    [SerializeField]
    private int colCount = 8;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private float tileSize = 1.0f;
    [SerializeField]
    private Color[] colors;


    
    private float dragDistance = 0.5f;
    private Vector2 firstPosition, lastPosition;
    public GameObject[,] hexagons;
    private Vector2[] centerPoints;

    void Start()
    {
        centerPoints = new Vector2[1000];
        hexagons = new GameObject[colCount,rowCount];
        for (int x = 0; x < colCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                CreateHexagon(x,y);
            }
        }
        MoveGridToCenter();
        InitializeCenterPoints();
    }


    void Update()
    {

    }

    private void CreateHexagon(int column,int row)
    {
        GameObject obj = Instantiate(prefab, transform);
        int colorIndex = Random.Range(0, colors.Length);
        obj.GetComponent<SpriteRenderer>().color = colors[colorIndex];
        obj.GetComponent<HexagonController>().SetColumnAndRow(column, row);
        obj.transform.position = GetHexagonPosition(column,row);
        hexagons[column, row] = obj;
    }

    private Vector2 GetHexagonPosition(int column,int row)
    {
        float offsetX;
        float offsetY;
        offsetX = column * (tileSize) * Mathf.Sin(Mathf.PI / 6) / 2;
        offsetY = (column % 2) * (tileSize) * Mathf.Cos(Mathf.PI / 6) / 2;
        return new Vector2(column - offsetX, row + offsetY - (tileSize / 6 * row));
    }
    
    private void MoveGridToCenter()
    {
        float gridH = rowCount * tileSize;
        float gridW = colCount * tileSize; 
        float offsetX = colCount * (tileSize) * Mathf.Sin(Mathf.PI / 6) / 2;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2 + offsetX / 2, -(gridH / 1.5f - tileSize / 2));
        var size = new Vector3(gridW, gridH);
        this.transform.GetComponent<BoxCollider2D>().size = size;
        this.transform.GetComponent<BoxCollider2D>().offset = new Vector2(size.x/2 - tileSize / 2 - offsetX / 2,size.y/2f - tileSize);
        
    }


    private void OnMouseDown()
    {
        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(firstPosition, lastPosition) < dragDistance)
        {
            if(instatiatedDot != null)
            {
                Destroy(instatiatedDot);
                instatiatedDot = null;
            }
            int index = FindSelectedPoint(lastPosition);
            instatiatedDot = Instantiate(selectionDot,centerPoints[index], Quaternion.identity);
            for(int x = 0; x < colCount; x++)
            {
                for(int y = 0; y < rowCount; y++)
                {
                    if (hexagons[x, y].GetComponent<HexagonController>().centerPoints.Contains(centerPoints[index]))
                    {
                        hexagons[x, y].transform.parent = instatiatedDot.transform;
                    }
                    else
                    {
                        hexagons[x, y].transform.parent = this.transform;
                    }
                }
            }
        }
        else
        {
            float swipeAngle = CalculateAngle(lastPosition, firstPosition);
            Debug.Log($"Swipe Angle : {swipeAngle}");
            if (swipeAngle > 0 && swipeAngle < 180)
            {
                Debug.Log("OutherClockwise!");
                instatiatedDot.GetComponent<RotateController>().SetUpRotate(false);
            }
            if (swipeAngle < 0 && swipeAngle > -179)
            {
                Debug.Log("Clockwise!");
                instatiatedDot.GetComponent<RotateController>().SetUpRotate(true);

            }
        }
    }
    private float CalculateAngle(Vector2 pos1, Vector2 pos2)
    {
        return Mathf.Atan2(pos2.y - pos1.y, pos2.x - pos1.x) * 180 / Mathf.PI;
    }


    private Vector2 FindCenterPointGameObjects(GameObject[] gameObjects)
    {
        float totalX = 0f;
        float totalY = 0f;
        for (var i = 0; i < gameObjects.Length; i++)
        {
            totalX += gameObjects[i].transform.position.x;
            totalY += gameObjects[i].transform.position.y;
        }
        totalX /= gameObjects.Length;
        totalY /= gameObjects.Length;
        return new Vector2(totalX, totalY);
    }
    private Vector2 FindCenterPointVectors(Vector2[] vector2s)
    {
        float totalX = 0f;
        float totalY = 0f;
        for (var i = 0; i < vector2s.Length; i++)
        {
            totalX += vector2s[i].x;
            totalY += vector2s[i].y;
        }
        totalX /= vector2s.Length;
        totalY /= vector2s.Length;
        return new Vector2(totalX, totalY);
    }

    private void InitializeCenterPoints()
    {
        int index = 0;
        for (int x = 0; x < colCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                    
                if (x % 2 == 0)
                {
                    //centerPoints[index].y = centerPoints[index].y + (tileSize) * Mathf.Cos(Mathf.PI / 6) / 3;
                    if (x < colCount-1 && y < rowCount -1)
                    {
                        centerPoints[index] = FindCenterPointGameObjects(
                        new GameObject[]{
                            hexagons[x,y],
                            hexagons[x,y+1],
                            hexagons[x+1,y]
                        });
                        hexagons[x, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x, y + 1].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x + 1, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        //Instantiate(selectionDot, centerPoints[index], Quaternion.identity).name = $"{x},{y},{x},{y + 1},{x + 1},{y}";
                        index++;
                    }

                    if (x < colCount - 1 && y > 0)
                    {
                        centerPoints[index] = FindCenterPointGameObjects(
                        new GameObject[]{
                            hexagons[x,y],
                            hexagons[x+1,y],
                            hexagons[x+1,y-1]
                        });
                        hexagons[x, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x + 1, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x + 1, y - 1].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        //Instantiate(selectionDot, centerPoints[index], Quaternion.identity).name = $"{x},{y},{x + 1},{y},{x + 1},{y - 1}";
                        index++;
                    }
                }
                else
                {
                    if (x < colCount - 1 && y < rowCount - 1)
                    {
                        centerPoints[index] = FindCenterPointGameObjects(
                        new GameObject[]{
                            hexagons[x,y],
                            hexagons[x+1,y+1],
                            hexagons[x+1,y]
                        });
                        hexagons[x, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x + 1, y + 1].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x + 1, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        //Instantiate(selectionDot, centerPoints[index], Quaternion.identity).name = $"{x},{y},{x + 1},{y + 1},{x + 1},{y}";
                        index++;
                    }
                    if (x < colCount - 1 && y < rowCount - 1)
                    {
                        centerPoints[index] = FindCenterPointGameObjects(
                        new GameObject[]{
                            hexagons[x,y],
                            hexagons[x,y+1],
                            hexagons[x+1,y+1]
                        });
                        hexagons[x, y].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x, y + 1].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        hexagons[x + 1, y + 1].GetComponent<HexagonController>().centerPoints.Add(centerPoints[index]);
                        //Instantiate(selectionDot, centerPoints[index], Quaternion.identity).name = $"{x},{y},{x + 1},{y},{x + 1},{y + 1}";
                        index++;
                    }
                }
                
                
            }
        }
        Debug.Log($"Size Of Centers {index}");
    } 

    private int FindSelectedPoint(Vector2 pos)
    {
        float distance = float.MaxValue;
        int index = -1;
        for (int i = 0; i < centerPoints.Length; i++)
        {
            float tempDis = Vector2.Distance(centerPoints[i], pos);
            if (tempDis < distance)
            {
                distance = tempDis;
                index = i;
            }
        }
        return index;
    }

    private void FindMatch()
    {

    }
}

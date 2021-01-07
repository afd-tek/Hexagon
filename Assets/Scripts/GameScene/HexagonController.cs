using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonController : MonoBehaviour
{
    private GridController gridController;
    private Vector2[] centerPositions;
    public Color selectionColor;
    public bool isSelected;
    private bool state = false;
    public List<Vector2> centerPoints = new List<Vector2>();

    public void SetSelected(bool value)
    {
        //Destroy(selectionBrush);
        //selectionBrush = null;
        //isSelected = value;
        //if (value)
        //{
        //    selectionBrush = Instantiate(this.gameObject,this.transform.position,Quaternion.identity);
        //    selectionBrush.GetComponent<SpriteRenderer>().color = selectionColor;
        //    selectionBrush.transform.parent = this.transform.parent;
        //    for(int i= 0; i < 6; i++)
        //    {
        //        if(Vector2.Distance(centerPositions[i],gridController.instatiatedDot.transform.position) == 0)
        //        {
        //            selectedPosition = i;
        //        }
        //    }
        //}
        if (value)
        {
            this.transform.parent = gridController.instatiatedDot.transform;
        }
        else
        {
            this.transform.parent = gridController.transform;
        }

    }

    private bool isMatched;
    private Vector2 firstPosition, lastPosition;
    private float dragDistance;
    public int column,row,targetX,targetY;
    public int selectedPosition;
    GameObject selectionBrush;

    void Start()
    {
        this.name = $"({column},{row})";
        gridController = FindObjectOfType<GridController>();
        dragDistance = this.transform.localScale.x;
        centerPositions = new Vector2[6];
        for (int i= 0;i < 6;i++)
        {
            centerPositions[i] = new Vector2(float.MaxValue,float.MaxValue);
        }
        InitializeCenterPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstPosition = touch.position;
                lastPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lastPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lastPosition = touch.position;
                if (isSelected && (Mathf.Abs(lastPosition.x - firstPosition.x) > dragDistance || Mathf.Abs(lastPosition.y - firstPosition.y) > dragDistance))
                {
                    float swipeAngle = CalculateAngle(lastPosition,firstPosition);
                    Debug.Log($"Swipe Angle : {swipeAngle}");
                    if (swipeAngle > 0 && swipeAngle < 180)
                    {
                        Debug.Log("Clockwise!");
                        RotateObjects(true);
                    }
                    if (swipeAngle < 0 && swipeAngle > -179)
                    {
                        Debug.Log("OutherClockwise!");
                        RotateObjects(false);
                    }
                }
                else
                {
                    if (column % 2 == 0) {
                        var pos = Camera.main.ScreenToWorldPoint(lastPosition);
                        if (column == Mathf.RoundToInt(pos.x) && row == Mathf.RoundToInt(pos.y))
                        {
                            Debug.Log("Tabbed : " + this.gameObject.name);
                            int i = MatchTapPositionWithCenterPositions(pos);
                            if (gridController.instatiatedDot != null) Destroy(gridController.instatiatedDot);
                            if (i != -1)
                            {
                                Debug.Log("Instatiated : " + this.gameObject.name);
                                gridController.instatiatedDot = Instantiate(gridController.selectionDot, centerPositions[i], Quaternion.identity);
                                MarkAsSelected(i);
                            }
                        }
                    }
                }
            }
        }

      
       
    }

    private void SetTargetXY(int index,int step, bool isClockwise)
    {
        if (isClockwise)
        {
            if (index == 0)
            {
                if (step == 0)
                {
                    targetX = column + 1;
                    targetY = row;
                }
                else if (step == 1)
                {
                    targetX = column + 1;
                    targetY = row - 1;
                } else if(step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 1)
            {
                if (step == 0)
                {
                    targetX = column + 1;
                    targetY = row - 1;
                }
                else if (step == 1)
                {
                    targetX = column;
                    targetY = row - 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 2)
            {
                if (step == 0)
                {
                    targetX = column;
                    targetY = row-1;
                }
                else if (step == 1)
                {
                    targetX = column - 1;
                    targetY = row - 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 3)
            {
                if (step == 0)
                {
                    targetX = column - 1;
                    targetY = row - 1;
                }
                else if (step == 1)
                {
                    targetX = column - 1;
                    targetY = row;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 4)
            {
                if (step == 0)
                {
                    targetX = column - 1;
                    targetY = row;
                }
                else if (step == 1)
                {
                    targetX = column;
                    targetY = row + 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 5)
            {
                if (step == 0)
                {
                    targetX = column;
                    targetY = row + 1;
                }
                else if (step == 1)
                {
                    targetX = column + 1;
                    targetY = row;
                } else if(step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
        }
        else
        {
            if (index == 0)
            {
                if (step == 0)
                {
                    targetX = column + 1;
                    targetY = row - 1;
                }
                else if (step == 1)
                {
                    targetX = column + 1;
                    targetY = row;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 1)
            {
                if (step == 0)
                {
                    targetX = column;
                    targetY = row - 1;
                }
                else if (step == 1)
                {
                    targetX = column + 1;
                    targetY = row - 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 2)
            {
                if (step == 0)
                {
                    targetX = column - 1;
                    targetY = row - 1;
                }
                else if (step == 1)
                {
                    targetX = column;
                    targetY = row - 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 3)
            {
                if (step == 0)
                {
                    targetX = column - 1;
                    targetY = row;
                }
                else if (step == 1)
                {
                    targetX = column - 1;
                    targetY = row - 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 4)
            {
                if (step == 0)
                {
                    targetX = column;
                    targetY = row + 1;
                }
                else if (step == 1)
                {
                    targetX = column - 1;
                    targetY = row;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
            if (index == 5)
            {
                if (step == 0)
                {
                    targetX = column + 1;
                    targetY = row;
                }
                else if (step == 1)
                {
                    targetX = column;
                    targetY = row + 1;
                }
                else if (step == 2)
                {
                    targetX = column;
                    targetY = row;
                }
            }
        }
        Debug.Log($"TX:{targetX},TY:{targetY},SI:{index},Step:{step}");
    }

    private void RotateObjects(bool isClockWise)
    {
        Debug.Log("RotateState");
        this.gridController.GetComponent<GridController>().instatiatedDot.GetComponent<RotateController>().SetUpRotate(isClockWise);
    }

   
    private int MatchTapPositionWithCenterPositions(Vector2 pos)
    {
        if (pos != null)
        {
            float distance = float.MaxValue;
            int index = -1;
            for (int i = 0; i < 6; i++)
            {
                if (centerPositions[i].x != float.MaxValue && centerPositions[i].y != float.MaxValue)
                {
                    float tempDis = Vector2.Distance(pos, centerPositions[i]);
                    if (tempDis < distance)
                    {
                        distance = tempDis;
                        index = i;
                    }
                }
            }
            Debug.Log("Returned Index : "+index);
            return index;
        }
        Debug.Log("Tab Position is null");
        return -1;
    }

    private void FindMatches()
    {
        Debug.Log("Finding Matches");
    }

    private Vector2 FindCenterPoint(GameObject[] gameObjects)
    {
        //if (gameObjects.Length == 0)
        //    return Vector2.zero;
        //if (gameObjects.Length == 1)
        //    return gameObjects[0].transform.position;
        //var bounds = new Bounds(gameObjects[0].transform.position, Vector2.zero);
        //Debug.Log($"------P{0}: {gameObjects[0].transform.position}");
        //for (var i = 1; i < gameObjects.Length; i++)
        //{
        //    Debug.Log($"------P{i}: {gameObjects[i].transform.position}");
        //    bounds.Encapsulate(gameObjects[i].transform.position);
        //}
        //Debug.Log($"------Center: {bounds.center}");
        //return bounds.center;
        float totalX = 0f;
        float totalY = 0f;
        for (var i = 0; i < gameObjects.Length; i++)
        {
            totalX += gameObjects[i].transform.position.x;
            totalY += gameObjects[i].transform.position.y;
        }
        totalX /= gameObjects.Length;
        totalY /= gameObjects.Length;
        return new Vector2(totalX,totalY);
    }


    private float CalculateAngle(Vector2 pos1,Vector2 pos2)
    {
        return Mathf.Atan2(pos2.y - pos1.y, pos2.x - pos1.x) * 180 / Mathf.PI;
    }

    public void SetColumnAndRow(int column,int row)
    {
        this.column = column;
        this.row = row;
    }

    private void MarkAsSelected(int index)
    {
        for(int i =0;i < gridController.hexagons.GetLength(0); i++)
        {
            for (int j = 0;j < gridController.hexagons.GetLength(1); j++)
            {
                gridController.hexagons[i, j].GetComponent<HexagonController>().SetSelected(false);
            }
        }
        if (index == 0)
        {
            this.SetSelected(true);
            gridController.hexagons[column + 1, row].GetComponent<HexagonController>().SetSelected(true);
            gridController.hexagons[column + 1, row - 1].GetComponent<HexagonController>().SetSelected(true);
        }
        if (index == 1)
        {
            this.SetSelected(true);
            gridController.hexagons[column + 1, row - 1].GetComponent<HexagonController>().SetSelected(true);
            gridController.hexagons[column, row - 1].GetComponent<HexagonController>().SetSelected(true);
        }
        if (index == 2)
        {
            this.SetSelected(true);
            gridController.hexagons[column, row - 1].GetComponent<HexagonController>().SetSelected(true);
            gridController.hexagons[column - 1, row - 1].GetComponent<HexagonController>().SetSelected(true);
        }
        if (index == 3)
        {
            this.SetSelected(true);
            gridController.hexagons[column - 1, row - 1].GetComponent<HexagonController>().SetSelected(true);
            gridController.hexagons[column - 1, row].GetComponent<HexagonController>().SetSelected(true);
        }
        if (index == 4)
        {
            this.SetSelected(true);
            gridController.hexagons[column - 1, row].GetComponent<HexagonController>().SetSelected(true);
            gridController.hexagons[column, row + 1].GetComponent<HexagonController>().SetSelected(true);
        }
        if (index == 5)
        {
            this.SetSelected(true);
            gridController.hexagons[column, row + 1].GetComponent<HexagonController>().SetSelected(true);
            gridController.hexagons[column + 1, row].GetComponent<HexagonController>().SetSelected(true);
        }
    }

    private void InitializeCenterPositions()
    {
        if (row > 0 && column < gridController.hexagons.GetLength(0) - 1)
        {
            centerPositions[0] = FindCenterPoint(new GameObject[] {
            this.gameObject,
            gridController.hexagons[column + 1, row],
            gridController.hexagons[column + 1, row - 1] });
        }
        if (row > 0 && column < gridController.hexagons.GetLength(0) - 1)
        {
            centerPositions[1] = FindCenterPoint(new GameObject[] {
            this.gameObject,
            gridController.hexagons[column + 1, row - 1],
            gridController.hexagons[column, row - 1] });
        }
        if (row > 0 && column > 0)
        {
            centerPositions[2] = FindCenterPoint(new GameObject[] {
            this.gameObject,
            gridController.hexagons[column, row - 1],
            gridController.hexagons[column - 1, row - 1] });
        }
        if (row > 0 && column > 0)
        {
            centerPositions[3] = FindCenterPoint(new GameObject[] {
            this.gameObject,
            gridController.hexagons[column - 1, row - 1],
            gridController.hexagons[column - 1, row] });
        }
        if (row < gridController.hexagons.GetLength(1) - 1 && column > 0)
        {
            centerPositions[4] = FindCenterPoint(new GameObject[] {
            this.gameObject,
            gridController.hexagons[column - 1, row],
            gridController.hexagons[column, row + 1] });
        }
        if (row < gridController.hexagons.GetLength(1) - 1 && column < gridController.hexagons.GetLength(0) - 1)
        {
            centerPositions[5] = FindCenterPoint(new GameObject[] {
            this.gameObject,
            gridController.hexagons[column, row + 1],
            gridController.hexagons[column + 1, row] });
        }

    }

}

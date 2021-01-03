using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
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


    private float offsetX;
    private float offsetY;
    private float dragDistance;
    private Vector2 fp, lp;

    void Start()
    {

        for (int x = 0; x < colCount; x++)
        {
            offsetX = (x * (tileSize/6) * Mathf.Sin(Mathf.PI / 3));
            Debug.Log(offsetX);
            for (int y = 0; y < rowCount; y++)
            {
                offsetY = (x % 2) * (tileSize) * Mathf.Cos(Mathf.PI / 3);
                GameObject obj = Instantiate(prefab, transform);
                obj.GetComponent<Transform>().Rotate(0,0,90.0f);
                obj.transform.position = new Vector2(x - offsetX, y + offsetY);
                int colorIndex = Random.Range(0, colors.Length - 1);
                obj.GetComponent<SpriteRenderer>().color = colors[colorIndex];
            }
        }

        float gridH = rowCount * tileSize;
        float gridW = colCount * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize, -(gridH - tileSize / 2));
    }


    void Update()
    {
        
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    float swipeAngle = Mathf.Atan2(lp.y - fp.y, lp.x - fp.x) * 180 / Mathf.PI;
                    if (swipeAngle >= 0 && swipeAngle <= 60)
                    {
                        Debug.Log("Clockwise!");
                    }
                    else if(swipeAngle >= -60 && swipeAngle <= 0)
                    {
                        Debug.Log("Clockwise!");
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }




}

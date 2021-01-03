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
    private float tileSize = 1f;
    [SerializeField]
    private Color[] colors;


    private float offsetX;
    private float offsetY;

    void Start()
    {

        for (int y = 0; y < rowCount; y++)
        {
            offsetX = (y % 2) * (tileSize / 2) * Mathf.Sin(30);
            offsetY = -((tileSize) * Mathf.Sin(60));
            for (int x = 0; x < colCount; x++)
            {
                GameObject obj = Instantiate(prefab, transform);
                //obj.transform.rotation = Quaternion.identity;
                //obj.transform.Rotate(90.0f, 0.0f, 0.0f);
                obj.transform.position = new Vector2(x + offsetX, y + offsetY);
                int colorIndex = Random.Range(0, colors.Length - 1);
                obj.GetComponent<SpriteRenderer>().color = colors[colorIndex];
            }
        }

        float gridH = rowCount * tileSize;
        float gridW = colCount * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, -gridH + tileSize / 2);
    }


    void Update()
    {

    }




}

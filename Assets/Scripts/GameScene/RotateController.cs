using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    private bool rotate = false;
    private bool isClockWise = true;
    private float rotZ = 0;
    public float rotSpeed = 360;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            //Disable User Controls
            if (isClockWise)
                rotZ -= rotSpeed * Time.deltaTime;
            else
                rotZ += rotSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0,0,rotZ);
            if (Mathf.Abs(rotZ/360) >= 1)
            {
                rotate = false;
                rotZ = 0; 
                transform.rotation = Quaternion.Euler(0, 0, 0);
                //Enable User Controls
            }
        }
    }



    public void SetUpRotate(bool isClockWise)
    {
        this.rotate = true;
        this.isClockWise = isClockWise;
    }
}

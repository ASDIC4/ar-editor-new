//<董静涛>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureControl : MonoBehaviour
{

    public static int status = -1;
  
    public static float TouchTime = 0;

    protected bool isSelected = false;

    protected void OnMouseDown()
    {
        isSelected = true;
    }

    protected void OnMouseUp()
    {
        isSelected = false;
        status = -1;
        TouchTime = 0;
    }
    // Update is called once per frame
    protected void Update()
    {
        if (!isSelected)
        {
            return;
        }
        else if (status == -1)
        {
            InputCheck();
        }
    }

    protected virtual void InputCheck() { }
}
//</董静涛>
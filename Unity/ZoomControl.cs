//<董静涛>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomControl : GestureControl
{
     
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;
    
    Vector3 RealScale = new Vector3(1f, 1f, 1f);
    
    float InitialScale = 0;
   
    public float ScaleSpeed = 0.1f;
  
    public float MaxScale = 2.5f;
    public float MinScale = 0.5f;

    void Start()
    {
        
        InitialScale = this.transform.localScale.x;
    }

    protected override void InputCheck()
    {
       

        if (Input.touchCount > 1)
        {
            status = 2;
            StartCoroutine(CustomOnMouseDown());
        }
 
    }

    IEnumerator CustomOnMouseDown()
    {
        
        while (Input.GetMouseButton(0))
        {
            
            RealScale = this.transform.localScale;
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                
                Vector3 tempPosition1 = Input.GetTouch(0).position;
                Vector3 tempPosition2 = Input.GetTouch(1).position;
                if (isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    
                    if (RealScale.x < InitialScale * MaxScale)
                    {
                        this.transform.localScale += this.transform.localScale * ScaleSpeed;
                    }
                }
                else
                {
                    
                    if (RealScale.x > InitialScale * MinScale)
                    {
                        this.transform.localScale -= this.transform.localScale * ScaleSpeed;
                    }
                }
            
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
//</董静涛>
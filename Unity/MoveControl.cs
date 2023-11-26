//<董静涛>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveControl : GestureControl
{
    protected override void InputCheck()
    {
        
        if (Input.touchCount == 1)
        {
            
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                TouchTime += Time.deltaTime;
                if (TouchTime > 1)
                {
                    status = 0;
                }
            }
            if (status == 0)
            {
                StartCoroutine(CustomOnMouseDown());
            }
        }
    }
    IEnumerator CustomOnMouseDown()
    {
        Vector3 ScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

      
        Vector3 WorldPostion = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z));

   
        Vector3 distance = transform.position - WorldPostion;

        while (Input.GetMouseButton(0))
        {
           
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
   
            Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + distance;
              
            transform.position = CurPosition;
       
            yield return new WaitForFixedUpdate();
        }
    }
}
//</董静涛>
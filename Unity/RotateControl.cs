//<董静涛>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class RotateControl : GestureControl
{
    protected override void InputCheck()
    {
        #region  
        if (Input.touchCount == 1)
        {
            
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                status = 1;
                try
                {
                    StartCoroutine(CustomOnMouseDown());
                }
                catch (Exception e)
                {
                    Debug.Log("Rotating"+e.ToString());
                }
            }

        }
        #endregion

        #region   
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(Vector3.up, 45 * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(Vector3.up, -45 * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Rotate(Vector3.left, 45 * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(Vector3.left, -45 * Time.deltaTime, Space.World);
        }
        #endregion
    }

    IEnumerator CustomOnMouseDown()
    {
       
        while (Input.GetMouseButton(0))
        {
           
#if UNITY_ANDROID || UNITY_IPHONE
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
                if (EventSystem.current.IsPointerOverGameObject())
#endif
            {
                Debug.Log("1");
            }
            else
            {
                float XX = Input.GetAxis("Mouse X");
                float YY = Input.GetAxis("Mouse Y");
                #region
             
                if (Mathf.Abs(XX) >= Mathf.Abs(YY))
                {
                   
                    if (XX < 0)
                    {
                        transform.Rotate(Vector3.up, 45 * Time.deltaTime, Space.World);
                    }
           
                    if (XX > 0)
                    {
                        transform.Rotate(-Vector3.up, 45 * Time.deltaTime, Space.World);
                    }
                }
                else
                {
                    
                    if (YY < 0)
                    {
                        transform.Rotate(Vector3.left, 45 * Time.deltaTime, Space.World);
                    }
                  
                    if (YY > 0)
                    {
                        transform.Rotate(-Vector3.left, 45 * Time.deltaTime, Space.World);
                    }
                }
                #endregion
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
//</董静涛>
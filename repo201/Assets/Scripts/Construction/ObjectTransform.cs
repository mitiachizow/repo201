using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

public class ObjectTransform : MonoBehaviour
{

    //RaycastHit hit;
    void Start()
    {
        
    }

    int oldTouchCount;
    float distance;
    void Update()
    {
        if (Multiplatform.TouchCount == 0) return;
        if (oldTouchCount == 0 && Multiplatform.TouchCount == 1 )
        {
            oldTouchCount = 0;
            //distance = Vector2Int.Distance((Camera.main.ScreenToViewportPoint(Multiplatform.TouchPosition(0)).x, Camera.main.ScreenToViewportPoint(Multiplatform.TouchPosition(0)).y)
            //    ,);

        }


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Multiplatform.TouchPosition(0));
        Physics.Raycast(ray, out hit);

        if(hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);

        }
    }
}

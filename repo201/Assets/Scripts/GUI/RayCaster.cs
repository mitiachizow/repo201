using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneBehavior
{
    public class RayCaster : MonoBehaviour
    {
        public static RaycastHit hit;
        public static bool isHit;
        Ray ray;
        int touchCount, oldTouchCount;

        void Update()
        {
            oldTouchCount = touchCount;
            touchCount = Multiplatform.TouchCount;

            if(oldTouchCount == 0 && touchCount == 0) isHit = false;
            if (touchCount != 1) return;
            ray = Camera.main.ScreenPointToRay(Multiplatform.TouchPosition(0));

            isHit = Physics.Raycast(ray, out hit);
        }
    }


    //if (oldTouchCount == 1 && Multiplatform.TouchCount == 0) Debug.Log(oldHit.collider.gameObject.name + "/" + currentHit.collider.gameObject.name);
    //if (Multiplatform.TouchCount != 1) return;
    //Debug.Log("CAST");
    //ray = Camera.main.ScreenPointToRay(Multiplatform.TouchPosition(0));
    //Physics.Raycast(ray, out currentHit);
    //if (Multiplatform.TouchCount == 1 && oldTouchCount == 0) oldHit = currentHit;
    //oldTouchCount = Multiplatform.TouchCount;

}



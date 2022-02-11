using UnityEngine;
using SceneBehavior;

namespace RayBehaviour
{
    public class RayCaster : MonoBehaviour
    {
        public static RaycastHit hit;
        public static bool isHit;
        Ray ray;

        void Update()
        {
            if(Input2.OldTouchCount == 0 && Input2.TouchCount == 0) isHit = false;
            if (Input2.TouchCount != 1) return;
            ray = Camera.main.ScreenPointToRay(Input2.GetTouchPosition(0));

            isHit = Physics.Raycast(ray, out hit);
        }
    }
}



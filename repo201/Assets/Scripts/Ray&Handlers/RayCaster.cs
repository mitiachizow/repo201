using UnityEngine;
using SceneBehavior;

namespace RayBehaviour
{
    /// <summary>
    /// RayCaster содержит в себе каст луча в точку, на которую нажал пользователь
    /// </summary>
    public class RayCaster : MonoBehaviour
    {
        public static RaycastHit hit;
        public static bool IsHit;

        private void Start()
        {
            IsHit = false;
        }
        private void Update()
        {
            if(Input2.OldTouchCount == 0 && Input2.TouchCount == 0) IsHit = false;

            if (Input2.TouchCount != 1) return;

            Ray ray = Camera.main.ScreenPointToRay(Input2.GetTouchPosition(0));

            IsHit = Physics.Raycast(ray, out hit);
        }
    }
}



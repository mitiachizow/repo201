using UnityEngine;
using SceneBehavior;
using RayBehaviour;

namespace UIModules
{
    public class InfoPlane : MonoBehaviour
    {
        [SerializeField]
        private GameObject plane;

        bool IsMoving;
        public UIPlaneState CurrentState { get; private set; }

        private void Update()
        {
            if (Input2.TouchCount != 1) { IsMoving = false; return; }
            if (RayCaster.isHit && RayCaster.hit.collider.gameObject.name == "Ticket") IsMoving = true;
            if (!IsMoving) return;

            Debug.Log("It's me, mario");
            //gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3();
        }

        public void SetTicketBehaviour(UIPlaneState state)
        {
            switch (state)
            {
                case UIPlaneState.Hide:
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180, 0, 0);
                    break;
                case UIPlaneState.Normal:
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    break;
            }
            CurrentState = state;
        }
    }

}


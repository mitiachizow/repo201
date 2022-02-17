using UnityEngine;
using SceneBehavior;
using RayBehaviour;
using UnityEngine.UI;
using ConstructionBehaviour;

namespace UIModules
{
    public class InfoPlane : MonoBehaviour
    {
        [SerializeField]
        private GameObject plane;
        [InspectorName("Name")]
        [SerializeField]
        private Text localName;
        //private static string staticLocalName;

        bool IsMoving;
        public UIPlaneState CurrentState { get; private set; }

        private void Update()
        {
            if (Input2.TouchCount != 1) { IsMoving = false; return; }
            if (RayCaster.isHit && RayCaster.hit.collider.gameObject.name == "Ticket") IsMoving = true;
            if (!IsMoving) return;

            //gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3();
        }

        //private void OnEnable()//Может быть стоит вызывать его из вне явно
        //{
        //    //ReDraw();
        //}

        //public /*static*/ void SetInfo(string name = "default name")
        //{
        //    localName.text = name;
        //}

        public void SetInfo(IInfo obj)
        {
            var a = obj.GetInfo();
            localName.text = a;
        }

        //public void ReDraw()
        //{
        //    localName.text = staticLocalName ?? default;
        //}

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


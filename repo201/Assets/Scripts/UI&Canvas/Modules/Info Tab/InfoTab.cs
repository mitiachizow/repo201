using UnityEngine;
using SceneBehavior;
using RayBehaviour;
using UnityEngine.UI;
using ConstructionBehaviour;

namespace UIModules
{
    public class InfoTab : MonoBehaviour
    {
        [SerializeField] private GameObject plane, bookmark;
        [SerializeField] private new Text name;

        private bool IsMoving;
        public TabState CurrentState { get; private set; }

        private void Update()
        {
            if (Input2.TouchCount != 1) { IsMoving = false; return; }
            if (RayCaster.IsHit && RayCaster.hit.collider.gameObject.name == bookmark.name) IsMoving = true;
            if (!IsMoving) return;
        }

        public void SetInfo(IInfo obj)
        {
            var a = obj.GetInfo();
            name.text = a;
        }
        public void SetTicketBehaviour(TabState state)
        {
            switch (state)
            {
                case TabState.Hide:
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180, 0, 0);
                    break;
                case TabState.Normal:
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    break;
            }
            CurrentState = state;
        }
    }

}


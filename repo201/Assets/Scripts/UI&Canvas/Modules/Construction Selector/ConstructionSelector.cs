using UnityEngine;

namespace UIModules
{
    public class ConstructionSelector : MonoBehaviour
    {
        [SerializeField]
        private GameObject selector;
        public UIPlaneState CurrentConstructionSelector { get; private set; }

        public void SetConstructionSelector(UIPlaneState state)
        {
            if (state == UIPlaneState.Hide)
            {
                selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            }
            else if (state == UIPlaneState.Normal)
            {
                selector.GetComponent<RectTransform>().anchoredPosition = new Vector3(200, 0, 0);
            }

            CurrentConstructionSelector = state;
        }

    }

}


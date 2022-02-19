using UnityEngine;

namespace UIModules
{
    public class ConstructionSelectorPosition : MonoBehaviour
    {
        [SerializeField] private RectTransform constructionSelectorPosition;
        public TabState Current { get; private set; }

        public void SetConstructionSelector(TabState state)
        {
            if (state == TabState.Hide)
            {
                constructionSelectorPosition.anchoredPosition = new Vector3(0, 0, 0);
            }
            else if (state == TabState.Normal)
            {
                constructionSelectorPosition.anchoredPosition = new Vector3(200, 0, 0);
            }

            Current = state;
        }

    }

}


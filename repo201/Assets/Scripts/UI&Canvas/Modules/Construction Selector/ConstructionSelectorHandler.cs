using UnityEngine;
using ConstructionBehaviour;
using RayBehaviour;

namespace UIModules
{
    public class ConstructionSelectorHandler : MonoBehaviour
    {
        [SerializeField] private GameObject bookmark, buttonBack;
        [SerializeField] private ConstructionSystem constructionSystem;
        [SerializeField] private ConstructionSelectorPosition constructionList;
        [SerializeField] private CanvasController canvasController;
        [SerializeField] private GameObject[] buttonsPool;



        private void Start() => RayHandler.AddHandler(BehaviourLogic);

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            if (gameObject.name == bookmark.name)
            {
                if (constructionList.Current == TabState.Normal)
                {
                    constructionList.SetConstructionSelector(TabState.Hide);
                }
                else if (constructionList.Current == TabState.Hide)
                {
                    constructionList.SetConstructionSelector(TabState.Normal);
                }
            }
            else if (gameObject.name == buttonBack.name)
            {
                canvasController.ForceChangeCanvasState(constructionSelector: false, constructionModeSelector: true);
            }
            else
            {
                //В buttonspool хранятся все кнопки, далее указана логика обработки всех этих кнопок
                foreach (GameObject obj in buttonsPool)
                {
                    if (obj.name == gameObject.name)
                    {
                        constructionSystem.SpawnConstruction(gameObject.GetComponent<PrefabContainer>().Construction);
                        return;
                    }
                }
            }
        }
    }

}

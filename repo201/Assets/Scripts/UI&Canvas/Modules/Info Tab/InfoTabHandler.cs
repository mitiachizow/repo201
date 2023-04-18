using UnityEngine;
using ConstructionBehaviour;
using SceneBehavior;
using RayBehaviour;

namespace UIModules
{
    public class InfoTabHandler : MonoBehaviour
    {
        [SerializeField] private GameObject bookmark, upButton, buttomButton;

        [SerializeField] private ConstructionSystem constructionSystem;
        [SerializeField] private InfoTab infoTab;
        [SerializeField] private CanvasController canvasController;

        private void Start()
        {
            RayHandler.AddHandler(BehaviourLogic);
        }

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            if (upButton.name == gameObject.name)
            {
                if (SceneStateController.CurrentSceneState == SceneState.Building)
                {
                    canvasController.ForceChangeCanvasState(constructionSelector: true, sceneStateSelector: true, infoTab:false);
                    constructionSystem.CreateConstruction();
                }
                else if (SceneStateController.CurrentSceneState == SceneState.Normal)
                {
                    constructionSystem.UpgradeConstruction(/*gameObject*/);
                }
            }
            else if (buttomButton.name == gameObject.name)
            {
                if (SceneStateController.CurrentSceneState == SceneState.Building)
                {
                    canvasController.ForceChangeCanvasState(constructionSelector: true, sceneStateSelector: true, infoTab: false);
                    constructionSystem.CanselCreateConstruction();
                }
                else if (SceneStateController.CurrentSceneState == SceneState.Normal)
                {
                    constructionSystem.DeCreateConstruction(/*gameObject*/);
                }
            }
            else if (bookmark.name == gameObject.name)
            {
                if (infoTab.CurrentState == TabState.Normal)
                {
                    infoTab.SetTicketBehaviour(TabState.Hide);
                }
                else if ((infoTab.CurrentState == TabState.Hide))
                {
                    infoTab.SetTicketBehaviour(TabState.Normal);
                }
            }
        }
    }

}



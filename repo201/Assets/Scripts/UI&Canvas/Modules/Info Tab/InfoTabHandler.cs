using UnityEngine;
using ConstructionBehaviour;
using SceneBehavior;
using RayBehaviour;

namespace UIModules
{
    public class InfoTabHandler : MonoBehaviour
    {
        [SerializeField] private GameObject bookmark, upButton, buttomButton, infoTab;

        [SerializeField] private ConstructionSystem constructionSystem;
        [SerializeField] private InfoTab infoPlane;
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
                    constructionSystem.UpgradeConstruction();
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
                    constructionSystem.DeCreateConstruction();
                }
            }
            else if (bookmark.name == gameObject.name)
            {
                if (infoPlane.CurrentState == TabState.Normal)
                {
                    infoPlane.SetTicketBehaviour(TabState.Hide);
                }
                else if ((infoPlane.CurrentState == TabState.Hide))
                {
                    infoPlane.SetTicketBehaviour(TabState.Normal);
                }
            }
        }
    }

}



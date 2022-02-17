using UnityEngine;
using ConstructionBehaviour;
using SceneBehavior;
using RayBehaviour;

namespace UIModules
{
    public class InfoPlaneHandler : MonoBehaviour
    {
        [SerializeField] private GameObject Ticket;
        [SerializeField] private GameObject UpButton;
        [SerializeField] private GameObject DownButton;
        [SerializeField] private GameObject InfoPlane;

        [Space(10)] [SerializeField] private ConstructionSystem ConstructionSystem;
        [SerializeField] private InfoPlane infoPlane;
        [SerializeField] private CanvasController canvasController;

        private void Start()
        {
            RayHandler.AddHandlerClick(BehaviourLogic);
        }

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            if (UpButton.name == gameObject.name)
            {
                if (SceneStateController.CurrentSceneState == SceneState.Building)
                {
                    canvasController.ForceChangeCanvasParts(addConstruction: true, sceneStateSwitcher: true, infoPlane:false);
                    ConstructionSystem.CreateConstruction();
                }
                else if (SceneStateController.CurrentSceneState == SceneState.Normal)
                {
                    ConstructionSystem.UpgradeConstruction();
                }
            }
            else if (DownButton.name == gameObject.name)
            {
                if (SceneStateController.CurrentSceneState == SceneState.Building)
                {
                    canvasController.ForceChangeCanvasParts(addConstruction: true, sceneStateSwitcher: true, infoPlane: false);
                    ConstructionSystem.CanselCreateConstruction();
                }
                else if (SceneStateController.CurrentSceneState == SceneState.Normal)
                {
                    ConstructionSystem.DeCreateConstruction();
                }
            }
            else if (Ticket.name == gameObject.name)
            {
                if (infoPlane.CurrentState == UIPlaneState.Normal)
                {
                    infoPlane.SetTicketBehaviour(UIPlaneState.Hide);
                }
                else if ((infoPlane.CurrentState == UIPlaneState.Hide))
                {
                    infoPlane.SetTicketBehaviour(UIPlaneState.Normal);
                }
            }
        }
    }

}



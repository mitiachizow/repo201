using UnityEngine;
using SceneBehavior;
using ConstructionBehaviour;
using UIModules;

namespace RayBehaviour
{
    public class UIHandler
    {
        private static UIHandler handler;

        private UIHandler()
        {
            RayHandler.AddHandlerClick(BehaviourLogic);
        }
        public static UIHandler GetUIHandler()
        {
            if (handler == null) return handler = new UIHandler();
            else return handler;
        }

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            switch (gameObject.name)
            {
                case "Factory Button":
                case "Apartment Button":
                    GameObject.Find("Construction System").GetComponent<ConstructionSystem>().SpawnConstruction(gameObject.GetComponent<ButtonContainer>().Construction);
                    break;
                case "Up Button":
                    if (SceneStateController.CurrentSceneState == SceneState.Building)
                    {
                        GameObject.Find("Canvas Controller").GetComponent<CanvasController>().ForceChangeCanvasParts(addConstruction: true, sceneStateSwitcher: true);
                        GameObject.Find("Construction System").GetComponent<ConstructionSystem>().CreateConstruction();
                    }
                    else if (SceneStateController.CurrentSceneState == SceneState.Normal) GameObject.Find("Construction System").GetComponent<ConstructionSystem>().UpgradeConstruction();

                    break;
                case "Bottom Button":
                    if (SceneStateController.CurrentSceneState == SceneState.Building)
                    {
                        GameObject.Find("Canvas Controller").GetComponent<CanvasController>().ForceChangeCanvasParts(addConstruction: true, sceneStateSwitcher: true);
                        GameObject.Find("Construction System").GetComponent<ConstructionSystem>().CanselCreateConstruction();

                    }
                    else if (SceneStateController.CurrentSceneState == SceneState.Normal) GameObject.Find("Construction System").GetComponent<ConstructionSystem>().DeCreateConstruction();
                    break;
                case "Info Ticket":
                    if (GameObject.Find("Info Plane").GetComponent<InfoPlane>().CurrentState == UIPlaneState.Normal)
                        GameObject.Find("Info Plane").GetComponent<InfoPlane>().SetTicketBehaviour(UIPlaneState.Hide);
                    else if ((GameObject.Find("Info Plane").GetComponent<InfoPlane>().CurrentState == UIPlaneState.Hide))
                        GameObject.Find("Info Plane").GetComponent<InfoPlane>().SetTicketBehaviour(UIPlaneState.Normal);
                    break;
                case "Construction List Ticket":
                    if (GameObject.Find("Construction Selector").GetComponent<ConstructionSelector>().CurrentConstructionSelector == UIPlaneState.Normal)
                        GameObject.Find("Construction Selector").GetComponent<ConstructionSelector>().SetConstructionSelector(UIPlaneState.Hide);
                    else if (GameObject.Find("Construction Selector").GetComponent<ConstructionSelector>().CurrentConstructionSelector == UIPlaneState.Hide)
                        GameObject.Find("Construction Selector").GetComponent<ConstructionSelector>().SetConstructionSelector(UIPlaneState.Normal);
                    break;
                case "Global View":
                case "External View":
                case "Normal View":
                case "Building View":
                    GameObject.Find("Scene State Switcher").GetComponent<ModeSwitcher>().SwitchSceneState(gameObject.GetComponent<NodeStatus>().State);
                    break;

            }
        }
    }

}

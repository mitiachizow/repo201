using UnityEngine;
using SceneBehavior;

namespace UIModules
{
    /// <summary>
    /// Вообще не уверен, что этот класс нужен
    /// </summary>
    public class CanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameObject addConstruction, infoPlane, sceneStateSwitcher, addLandButton;

        private void Start()
        {
            addConstruction.SetActive(true);
            infoPlane.SetActive(false);
            sceneStateSwitcher.SetActive(true);
            addLandButton.SetActive(false);
            SceneStateController.AddHandler(ChangeCanvasState);
        }

        private void ChangeCanvasState()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.External:
                    addConstruction.SetActive(false);
                    infoPlane.SetActive(false);
                    sceneStateSwitcher.SetActive(true);
                    addLandButton.SetActive(false);
                    break;
                case SceneState.Normal:
                    addConstruction.SetActive(false);
                    infoPlane.SetActive(false);
                    sceneStateSwitcher.SetActive(true);
                    addLandButton.SetActive(false);
                    break;
                case SceneState.Building:
                    addConstruction.SetActive(true);
                    infoPlane.SetActive(false);
                    sceneStateSwitcher.SetActive(true);
                    addLandButton.SetActive(false);
                    break;
                case SceneState.Global:
                    addConstruction.SetActive(false);
                    infoPlane.SetActive(false);
                    sceneStateSwitcher.SetActive(true);
                    addLandButton.SetActive(false);
                    break;
            }
        }

        public void ForceChangeCanvasParts(bool addConstruction = false, bool infoPlane = false, bool sceneStateSwitcher = false, bool addLandButton = false)
        {
            this.addConstruction.SetActive(addConstruction);
            this.infoPlane.SetActive(infoPlane);
            this.sceneStateSwitcher.SetActive(sceneStateSwitcher);
            this.addLandButton.SetActive(addLandButton);
        }
    }

}
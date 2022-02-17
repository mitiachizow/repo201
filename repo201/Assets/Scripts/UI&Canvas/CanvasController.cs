using UnityEngine;
using SceneBehavior;

namespace UIModules
{
    public class CanvasController : MonoBehaviour
    {
        [Header("Canvas Modules")]

        [SerializeField]
        private GameObject ConstructionUIModule;
        [SerializeField]
        private GameObject InfoPlaneUIModule;
        [SerializeField]
        private GameObject SceneStateSwitcherUIModule;
        private static GameObject staticConstructionUIModule, staticInfoPlaneUIModule, staticSceneStateSwitcherUIModule;

        private void Start()
        {
            staticConstructionUIModule = ConstructionUIModule;
            staticInfoPlaneUIModule = InfoPlaneUIModule;
            staticSceneStateSwitcherUIModule = SceneStateSwitcherUIModule;

            ConstructionUIModule.SetActive(true);
            InfoPlaneUIModule.SetActive(false);
            SceneStateSwitcherUIModule.SetActive(true);

            SceneStateController.AddHandler(ChangeCanvasState);//При изменении режима игры происходит переключение интерфейса
        }

        private void ChangeCanvasState()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.External:
                    ConstructionUIModule.SetActive(false);
                    InfoPlaneUIModule.SetActive(false);
                    SceneStateSwitcherUIModule.SetActive(true);
                    break;
                case SceneState.Normal:
                    ConstructionUIModule.SetActive(false);
                    InfoPlaneUIModule.SetActive(false);
                    SceneStateSwitcherUIModule.SetActive(true);
                    break;
                case SceneState.Building:
                    ConstructionUIModule.SetActive(true);
                    InfoPlaneUIModule.SetActive(false);
                    SceneStateSwitcherUIModule.SetActive(true);
                    break;
                case SceneState.Global:
                    ConstructionUIModule.SetActive(false);
                    InfoPlaneUIModule.SetActive(false);
                    SceneStateSwitcherUIModule.SetActive(true);
                    break;
            }
        }

        public void ForceChangeCanvasParts(bool? addConstruction = null, bool? infoPlane = null, bool? sceneStateSwitcher = null)
        {
            if (addConstruction != null) staticConstructionUIModule.SetActive((bool)addConstruction);
            if (infoPlane != null) staticInfoPlaneUIModule.SetActive((bool)infoPlane);
            if (sceneStateSwitcher != null) staticSceneStateSwitcherUIModule.SetActive((bool)sceneStateSwitcher);
            //staticConstructionUIModule.SetActive(addConstruction);
            //staticInfoPlaneUIModule.SetActive(infoPlane);
            //staticSceneStateSwitcherUIModule.SetActive(sceneStateSwitcher);
        }
    }

}
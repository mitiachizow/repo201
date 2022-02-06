using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneBehavior
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameObject scrollView, buildingInfo, SceneStateSwitcher, addLandButton;

        void Start()
        {
            scrollView.SetActive(true);
            buildingInfo.SetActive(false);
            SceneStateSwitcher.SetActive(true);
            addLandButton.SetActive(false);
            SceneStateController.AddHandler(ChangeCanvasState);
        }

        void ChangeCanvasState()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.External:
                    scrollView.SetActive(false);
                    buildingInfo.SetActive(false);
                    SceneStateSwitcher.SetActive(true);
                    addLandButton.SetActive(false);
                    break;
                case SceneState.Normal:
                    scrollView.SetActive(true);
                    buildingInfo.SetActive(true);
                    SceneStateSwitcher.SetActive(true);
                    addLandButton.SetActive(false);
                    break;
                //case SceneState.NormalBuildingSelected:
                //    scrollView.SetActive(false);
                //    buildingInfo.SetActive(false);
                //    changeViewButton.SetActive(false);
                //    addLandButton.SetActive(false);
                //    break;
            }
        }
    }

}
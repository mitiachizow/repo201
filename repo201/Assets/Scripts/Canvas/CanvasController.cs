using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneBehavior
{
    public class CanvasController : MonoBehaviour
    {
        public GameObject canvasState1, canvasState2, canvasState3;

        void Start()
        {
            canvasState1.SetActive(true);
            canvasState2.SetActive(false);
            canvasState3.SetActive(false);
            SceneStateController.AddHandler(ChangeCanvasState);
        }

        void ChangeCanvasState()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.External:
                    canvasState1.SetActive(false);
                    canvasState2.SetActive(true);
                    canvasState3.SetActive(false);
                    break;
                case SceneState.Normal:
                    canvasState1.SetActive(true);
                    canvasState2.SetActive(false);
                    canvasState3.SetActive(false);
                    break;
                case SceneState.NormalBuildingSelected:
                    canvasState1.SetActive(false);
                    canvasState2.SetActive(false);
                    canvasState3.SetActive(false);
                    break;
            }
        }
    }

}
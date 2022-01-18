using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneBehavior
{
    public class SceneStateController : MonoBehaviour
    {
        public static SceneState CurrentSceneState { get; private set; }
        public static SceneState OldSceneState { get; private set; }

        public delegate void ChangeState();
        event ChangeState Notyfi;

        public void Start()
        {
            CurrentSceneState = OldSceneState = SceneState.Normal;
        }

        public void ChangeSceneState(string sceneState)
        {
            OldSceneState = CurrentSceneState;
            switch (sceneState)
            {
                case "External":
                    CurrentSceneState = SceneState.External;
                    Notyfi.Invoke();
                    break;
                case "Normal":
                    CurrentSceneState = SceneState.Normal;
                    Notyfi.Invoke();
                    break;
                case "BuildingMovement":
                    CurrentSceneState = SceneState.BuildingMovement;
                    Notyfi.Invoke();
                    break;
            }
        }

        public void AddHandler(ChangeState func)
        {
            Notyfi += func;
        }
    }

    public enum SceneState : byte
    {
        External = 20,
        ExternalEconomic = 21,
        Normal = 1,
        BuildingMovement = 3,
        Default = 0
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneBehavior
{
    public class SceneStateController
    {
        public static SceneState CurrentSceneState { get; private set; }
        public static SceneState OldSceneState { get; private set; }

        public delegate void ChangeState();
        static event ChangeState Notyfi;

        static SceneStateController()
        {
            CurrentSceneState = OldSceneState = SceneState.Normal;
        }

        public static void ChangeSceneState(SceneState sceneState)
        {
            OldSceneState = CurrentSceneState;
            switch (sceneState)
            {
                case SceneState.External:
                    CurrentSceneState = SceneState.External;
                    Notyfi.Invoke();
                    break;
                case SceneState.Normal:
                    CurrentSceneState = SceneState.Normal;
                    Notyfi.Invoke();
                    break;
                case SceneState.NormalBuildingSelected:
                    CurrentSceneState = SceneState.NormalBuildingSelected;
                    Notyfi.Invoke();
                    break;
            }
        }

        public static void AddHandler(ChangeState func)
        {
            Notyfi += func;
        }
    }

    public enum SceneState : byte
    {
        External = 20,
        ExternalEconomic = 21,
        Normal = 1,
        NormalBuildingSelected = 3,
        Default = 0
    }
}
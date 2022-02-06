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
        static event ChangeState Notify;

        static SceneStateController()
        {
            CurrentSceneState = OldSceneState = SceneState.Building;
        }

        public static void ChangeSceneState(SceneState sceneState)
        {
            OldSceneState = CurrentSceneState;
            switch (sceneState)
            {
                case SceneState.External:
                    CurrentSceneState = SceneState.External;
                    Notify.Invoke();
                    break;
                case SceneState.Normal:
                    CurrentSceneState = SceneState.Normal;
                    Notify.Invoke();
                    break;
                //case SceneState.NormalBuildingSelected:
                //    CurrentSceneState = SceneState.NormalBuildingSelected;
                //    Notyfi.Invoke();
                //    break;
            }
        }

        public static void AddHandler(ChangeState func)
        {
            Notify += func;
        }
    }
}
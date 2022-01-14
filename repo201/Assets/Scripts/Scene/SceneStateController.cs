using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneBehavior
{
    public class SceneStateController : MonoBehaviour
    {
        
        public CameraBehavior.CameraLogic camLogic;
        SceneState sceneState;

        public delegate void ChangeState(SceneState sceneState);
        event ChangeState Notyfi;

        public void Start()
        {
            //Instantiate(Resources.Load<GameObject>("Factory"));
        }
        public void ChangeSceneState(string sceneState)
        {
            Debug.Log("Wow");
            switch (sceneState)
            {
                case "External":
                    Debug.Log("Ext");
                    Notyfi.Invoke(SceneState.External);
                    break;
                //case "ExternalEconomic":
                //    Notyfi.Invoke(SceneState.ExternalEconomic);
                //    break;
                case "Normal":
                    Notyfi.Invoke(SceneState.Normal);
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
        BuildingMovement,
        Default = 0 //????? вписывается ли дефолт в логику?
    }
}
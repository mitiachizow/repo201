using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SceneBehavior;
//using SceneClasses;


namespace CameraBehavior
{
    public class CameraLogic : MonoBehaviour
    {
        private int touchCount, oldTouchCount;
        // Start is called before the first frame update

        CameraFunctional cameraFunctional;

        void Start()
        {
            //Инициализировать камеру и сцену в отдельном скрипте
            CameraParams cameraParams = new CameraParams(Camera.main,GameObject.Find("Global Anchor"),
                GameObject.Find("Local Anchor"), maxCircle: 7f, midCircle: 2f, minCircle: 1f,
                externalCircle: 8.5f, externalRadius: 50f, standartRadius: 45f);

            SceneParams sceneParams = new SceneParams(50,50,2);

            cameraFunctional = new CameraFunctional(cameraParams,sceneParams);

        }

        // Update is called once per frame
        void Update()
        {
            oldTouchCount = touchCount;
            touchCount = Multiplatform.TouchCount();

            if(touchCount != oldTouchCount && touchCount != 0)
            {
                cameraFunctional.FirstTouch();
            }

            switch (touchCount)
            {
                case 1:
                    cameraFunctional.TransformManual();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
    }
}

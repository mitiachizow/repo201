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

        //public Transform CameraTransform { get; private set; }
        //public Transform GlobalAnchor { get; private set; } //Вокруг GlobalAnchor происходит вращение камеры в верхнем положении
        //public Transform CameraAnchor { get; private set; } //Вокруг LocalAnchor происходит вращение камеры в верхнем положении

        public readonly float minCircle, midCircle, maxCircle, externalCircle;
        /* lowCircle - минимальная высота, на которую мы можем опуститься с камерой
         * middleCircle - высота, на которую происходит возвращение камеры из externalCircle
         * maxCircle - высота, выше которой мы не можем подняться вручную
         * externalCircle - высота, на которую происходит переход при смене режима игры */

        public readonly float externalRadius, standartRadius;
        /* standartRadius - радиус, за который не может заходить камера в обычном режиме
         * externalRadius - радиус, по которому перемещается камера в верхнем положении
         * в будущем, когда можно будет добавлять больше регионов в игре, необходимо будет убрать externalRadius*/

        CameraFunctional cameraFunctional;

        void Start()
        {
            //Transform GlobalAnchor = GameObject.Find("Global Anchor").GetComponent<Transform>();
            //Transform CameraAnchor = GameObject.Find("Camera Anchor").GetComponent<Transform>();
            //Инициализировать камеру и сцену в отдельном скрипте
            //CameraParams cameraParams = new CameraParams(Camera.main,GameObject.Find("Global Anchor"),
            //    GameObject.Find("Local Anchor"), maxCircle: 7f, midCircle: 2f, minCircle: 1f,
            //    externalCircle: 8.5f, externalRadius: 50f, standartRadius: 45f);

            //SceneParams sceneParams = new SceneParams(50, 50, 2);

            cameraFunctional = new CameraFunctional(Camera.main.transform,Camera.main.transform.Find("Camera Anchor"),0f,10f,15f,30f);
        }

        // Update is called once per frame
        void Update()
        {
            oldTouchCount = touchCount;
            touchCount = Multiplatform.TouchCount();

            if(touchCount != oldTouchCount && touchCount != 0) //при первом нажатии необходимо считать необходимые параметры
            {
                cameraFunctional.FirstTouch();
            }

            if(touchCount == 0 && cameraFunctional.IsResidual) //остаточное движение
            {
                cameraFunctional.TransformResidual();
            }

            switch (touchCount)
            {
                case 1:
                    cameraFunctional.TransformManual();
                    break;
                case 2:
                    cameraFunctional.TwoTouchRotate();
                    cameraFunctional.ScaleManual();
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
    }
}

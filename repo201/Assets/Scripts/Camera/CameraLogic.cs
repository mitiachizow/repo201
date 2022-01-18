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

        public readonly float minCircle, midCircle, maxCircle, externalCircle;
        /* minCircle - минимальная высота, на которую мы можем опуститься с камерой
         * midCircle - высота, на которую происходит возвращение камеры из externalCircle
         * maxCircle - высота, выше которой мы не можем подняться вручную
         * externalCircle - высота, на которую происходит переход при смене режима игры */

        public readonly float externalRadius, standartRadius;
        /* standartRadius - радиус, за который не может заходить камера в обычном режиме
         * externalRadius - радиус, по которому перемещается камера в верхнем положении
         * в будущем, когда можно будет добавлять больше регионов в игре, необходимо будет убрать externalRadius*/

        CameraFunctional currentCamera;

        delegate void CurrentLogic();
        CurrentLogic currentLogic;

        void Start()
        {
            currentLogic = NormalMode;
            GameObject.Find("Scene State Controller").GetComponent<SceneStateController>().AddHandler(ChangeCamLogic);

            currentCamera = new CameraFunctional(Camera.main.transform, Camera.main.transform.Find("Camera Anchor"), GameObject.Find("Global Anchor").transform,
                minNormalCircleHeigh: 15f, midNormalCircleHeigh: 30f, maxNormalCircleHeigh: 50f, externalCircleHeigh: 120f, normalCircleRadius: 350f, externalCircleRadius: 400f);
        }

        void Update()
        {
            oldTouchCount = touchCount;
            touchCount = Multiplatform.TouchCount;
            //if (Multiplatform.IsPointerOverGameObject()) return;

            currentLogic();
        }

        public void ChangeCamLogic()
        {
            if ((SceneStateController.CurrentSceneState == SceneState.External && SceneStateController.OldSceneState == SceneState.Normal) || (SceneStateController.CurrentSceneState == SceneState.Normal && SceneStateController.OldSceneState == SceneState.External))
            {
                currentLogic = ChangeMode;
                currentCamera.GetFinalPoint(SceneStateController.CurrentSceneState);
            }

            if(SceneStateController.CurrentSceneState == SceneState.BuildingMovement) currentLogic = NullMode;
            if (SceneStateController.OldSceneState == SceneState.BuildingMovement && SceneStateController.CurrentSceneState == SceneState.Normal) currentLogic = NormalMode;
            currentCamera.FirstTouch();
        }


        #region CameraModes

        void NormalMode()
        {
            if (touchCount != oldTouchCount && touchCount != 0) //при первом нажатии необходимо считать необходимые параметры
            {
                currentCamera.FirstTouch();
                return;
            }

            if (touchCount == 0 && currentCamera.IsResidual) //остаточное движение
            {

                currentCamera.ResidualTransform();

            }

            switch (touchCount) //Базовая логика перемещения и вращения камеры
            {
                case 1:
                    currentCamera.OneTouchTransform();
                    break;
                case 2:
                    currentCamera.TwoTouchRotate();
                    currentCamera.TwoTouchScale();
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        float localTimer;//может быть стоит вынести из этого класса
        void ExternalMode()
        {
            if (touchCount != oldTouchCount && touchCount != 0) //при первом нажатии необходимо считать необходимые параметры
            {
                currentCamera.FirstTouch();
                return;
            }

            switch (touchCount) //Базовая логика перемещения и вращения камеры. обрати внимание, что в первом случае стоит return
            {
                case 0:
                    if (currentCamera.IsResidual)
                    {
                        currentCamera.ResidualRotate();
                        break;
                    }
                    else
                    {
                        localTimer += Time.deltaTime;
                        if (localTimer >= 5f) currentCamera.CircleRotateAutomatic(localTimer - 5f); //Занести это в камера функ?
                        return;
                    }
                case 1:
                    currentCamera.OneTouchRotate();
                    break;
                case 3:
                    break;
            }
            localTimer = 0f;
        }

        void NullMode()
        {

        }

        /// <summary>
        /// При переходе между External и Normal состояниями сцены, необходимо изменить положение камеры и ее функционал
        /// </summary>
        void ChangeMode()
        {
            if (currentCamera.ChangeMode())
            {
                switch (SceneStateController.CurrentSceneState)
                {
                    case SceneState.External:
                        currentLogic = ExternalMode;
                        break;
                    case SceneState.Normal:
                        currentLogic = NormalMode;
                        break;
                }
            }
        }

        #endregion
    }
}

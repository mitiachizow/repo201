﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SceneBehavior;
//using SceneClasses;


namespace CameraBehavior
{
    public class CameraLogic : MonoBehaviour
    {
        //private int touchCount, oldTouchCount;

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
            SetCurrentLogic();
            SceneStateController.AddHandler(ChangeCamLogic);

            currentCamera = new CameraFunctional(Camera.main.transform, Camera.main.transform.Find("Camera Anchor"), GameObject.Find("Global Anchor").transform,
                minNormalCircleHeigh: 50f, midNormalCircleHeigh: 70f, maxNormalCircleHeigh: 100f, externalCircleHeigh: 160f, normalCircleRadius: 250f, externalCircleRadius: 400f);
        }

        private void SetCurrentLogic()
        {
            switch(SceneStateController.CurrentSceneState)
            {
                case SceneState.Building:
                case SceneState.Normal:
                    currentLogic = NormalMode;
                    break;
                case SceneState.External:
                    currentLogic = ExternalMode;
                    break;
                case SceneState.Global:
                    break;
                default:
                    return;
            }
        }

        void Update()
        {
            if (Input2.TouchCount != Input2.OldTouchCount && Input2.TouchCount != 0) return;
            currentLogic();
        }

        public void ChangeCamLogic()
        {
            if ((SceneStateController.CurrentSceneState == SceneState.External && (SceneStateController.OldSceneState == SceneState.Normal || SceneStateController.OldSceneState == SceneState.Building)) || 
                ((SceneStateController.OldSceneState == SceneState.Normal || SceneStateController.OldSceneState == SceneState.Building) && SceneStateController.OldSceneState == SceneState.External))
            {
                currentLogic = ChangeMode;
                currentCamera.GetFinalPoint(SceneStateController.CurrentSceneState);
            }

            //if (SceneStateController.CurrentSceneState == SceneState.BuildingTransform) currentLogic = NullMode;
            //if (SceneStateController.OldSceneState == SceneState.BuildingTransform && SceneStateController.CurrentSceneState == SceneState.Building) currentLogic = NormalMode;
        }

        private void OnEnable()
        {
            currentCamera?.SetNull();
            Debug.Log("AWAKE");
        }


        /// <summary>
        /// It can break cam logic, use it carefully. (never use it (no , rl, NEVER))
        /// </summary>
        /// <param name="state"></param>
        //public void ForcedChangeCamLogic(SceneState state)
        //{
        //    switch (state)
        //    {
        //        case SceneState.Normal:
        //        case SceneState.Building:
        //            currentLogic = NormalMode;
        //            break;
        //        case SceneState.Default:
        //            currentLogic = NullMode;
        //            break;
        //    }
        //    currentCamera.SetNull();
        //}


        #region CameraModes

        void NormalMode()
        {
            if (Input2.TouchCount == 0 && currentCamera.IsResidual) //остаточное движение
            {
                currentCamera.ResidualTransform();
            }

            switch (Input2.TouchCount) //Базовая логика перемещения и вращения камеры
            {
                case 1:
                    currentCamera.OneTouchTransform();
                    break;
                case 2:
                    currentCamera.TwoTouchRotate();
                    currentCamera.TwoTouchScale();
                    break;
                case 3:
                default:
                    break;
            }
        }

        float localTimer;//может быть стоит вынести из этого класса
        void ExternalMode()
        {
            switch (Input2.TouchCount) //Базовая логика перемещения и вращения камеры. обрати внимание, что в первом случае стоит return
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

        void NullMode() { }

        /// <summary>
        /// При переходе между External и Normal состояниями сцены, необходимо изменить положение камеры и ее функционал
        /// </summary>
        void ChangeMode()
        {
            if (currentCamera.ChangeMode())
            {
                SetCurrentLogic();
                //switch (SceneStateController.CurrentSceneState)
                //{
                //    case SceneState.External:
                //        currentLogic = ExternalMode;
                //        break;
                //    case SceneState.Building:
                //    case SceneState.Normal:
                //        currentLogic = NormalMode;
                //        break;
                //}
            }
        }

        #endregion
    }
}

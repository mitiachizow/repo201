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

        CameraFunctional currentCamera;

        delegate void CurrentLogic();
        CurrentLogic currentLogic;

        void Start()
        {
            currentLogic = NormalMode;
            sceneState = SceneState.Normal;
            GameObject.Find("Scene State Controller").GetComponent<SceneStateController>().AddHandler(ChangeCamLogic);

            currentCamera = new CameraFunctional(Camera.main.transform, Camera.main.transform.Find("Camera Anchor"), GameObject.Find("Global Anchor").transform,
                minNormalCircleHeigh: 15f, midNormalCircleHeigh: 30f, maxNormalCircleHeigh: 50f, externalCircleHeigh: 120f, normalCircleRadius: 100f, externalCircleRadius: 270f, sceneState: sceneState);
        }

        void Update()
        {
            oldTouchCount = touchCount;
            touchCount = Multiplatform.TouchCount();

            currentLogic();
        }

        bool outOfRange;

        void NormalMode()
        {
            //if (currentCamera.ForceBackToPlayground && localTimer <= 0.5f)
            //{
            //    localTimer += Time.deltaTime;
            //    currentCamera.ResidualTransform();
            //    return;
            //}
            //else
            //{
            //    currentCamera.ForceBackToPlayground = false;
            //    localTimer = 0f;
            //}

            //}
            //if (outOfRange && currentCamera.CheckOutOfRangeVertical())
            //{
            //    currentCamera.TransformVertical();
            //    return;
            //}
            //else outOfRange = false;

            if (touchCount != oldTouchCount && touchCount != 0) //при первом нажатии необходимо считать необходимые параметры
            {
                currentCamera.FirstTouch();
                return;
            }

            if (touchCount == 0 && currentCamera.IsResidual) //остаточное движение
            {
                
                currentCamera.ResidualTransform();

            }


            if (((touchCount == 0 && oldTouchCount != 0) || (touchCount == 1 && oldTouchCount == 2)) && currentCamera.CheckOutOfRangeVertical()) //При выходе за границу карты, стартуют корутины и вы возвращаемся обратно.
            {
                currentCamera.GetFinalPoint(SceneState.Default);
                outOfRange = true;
                //Debug.Log("I'm out");
                //currentCamera.StartCoroutineVertical(this);

                //if (currentCamera.CheckOutOfRangeHorizontal())
                //{
                //    //currentCamera.StartCoroutineHorizontal(this);
                //}
            }
            //else if (touchCount == 0 && (oldTouchCount != 0 || currentCamera.IsResidual) && currentCamera.CheckOutOfRangeHorizontal())
            //{
            //    currentCamera.GetFinalPoint(SceneState.Default);//?

            //    //currentCamera.StartCoroutineHorizontal(this);
            //}

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

        float localTimer;
        void ExternalMode()
        {
            switch (touchCount) //Базовая логика перемещения и вращения камеры. обрати внимание, что в первом случае стоит return
            {
                case 0:
                    if(currentCamera.IsResidual)
                    {
                        currentCamera.ResidualRotate();
                        break;
                    }
                    else
                    {
                        localTimer += Time.deltaTime;
                        if (localTimer >= 10f) currentCamera.CircleRotateAutomatic(localTimer - 10f); //Занести это в камера функ?
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


        /// <summary>
        /// При переходе между External и Normal состояниями сцены, необходимо изменить положение камеры и ее функционал
        /// </summary>
        void ChangeMode()
        {
            if (currentCamera.ChangeMode(0.01f) <= 0.00001f)
            {
                switch (sceneState)
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

        /// <summary>
        /// Содержит в себе текущее состояние сцены
        /// </summary>
        SceneState sceneState;

        public void ChangeCamLogic(SceneState sceneState)
        {
            if ((sceneState == SceneState.External && this.sceneState == SceneState.Normal) || (sceneState == SceneState.Normal && this.sceneState == SceneState.External))
            {
                this.sceneState = sceneState;
                currentLogic = ChangeMode;
                currentCamera.GetFinalPoint(sceneState);
            }
        }
    }
}

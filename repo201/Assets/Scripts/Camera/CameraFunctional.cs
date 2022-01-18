﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using System;


namespace CameraBehavior
{
    public class CameraFunctional
    {
        Transform cameraAnchor, globalAnchor, cameraTransform;
        readonly float minNormalCircleHeigh, midNormalCircleHeigh, maxNormalCircleHeigh, externalCircleHeigh, externalCircleRadius, normalCircleRadius;
        float currentRadius, localCamRadius;

        public CameraFunctional(Transform cameraTransform, Transform cameraAnchor, Transform globalAnchor, float minNormalCircleHeigh, float midNormalCircleHeigh, float maxNormalCircleHeigh,
            float externalCircleHeigh, float externalCircleRadius, float normalCircleRadius)
        {
            this.cameraTransform = cameraTransform;
            this.cameraAnchor = cameraAnchor;

            this.externalCircleHeigh = externalCircleHeigh;
            this.minNormalCircleHeigh = minNormalCircleHeigh;
            this.midNormalCircleHeigh = midNormalCircleHeigh;
            this.maxNormalCircleHeigh = maxNormalCircleHeigh;

            currentRadius = this.normalCircleRadius = normalCircleRadius;
            this.externalCircleRadius = externalCircleRadius;

            this.globalAnchor = globalAnchor;
            localCamRadius = FindRadius(cameraAnchor);
        }

        public Vector3 CameraPosition
        {
            get
            {
                return cameraTransform.position;
            }
            set
            {
                switch (SceneStateController.CurrentSceneState)
                {
                    case SceneState.Normal:
                        {
                            Vector3 newCamValue;
                            newCamValue.y = Mathf.Clamp(value.y, minNormalCircleHeigh, maxNormalCircleHeigh);
                            if (value.y >= maxNormalCircleHeigh) residualSpeedTransform = new Vector3(0f, -value.y * 0.02f, 0f);
                            else if (value.y <= minNormalCircleHeigh) residualSpeedTransform = new Vector3(0f, value.y * 0.02f, 0f);

                            if (Vector3.Distance(new Vector3(cameraTransform.position.x, 0f, cameraTransform.position.z), new Vector3(globalAnchor.position.x, 0f, globalAnchor.position.z)) <= currentRadius)
                            {

                                newCamValue.x = value.x;
                                newCamValue.z = value.z;
                            }
                            else
                            {
                                float localAngle = Vector3.SignedAngle(globalAnchor.position - cameraTransform.position, cameraTransform.parent.gameObject.transform.forward, Vector3.up); //вот тут очень много ресурсов ем, нужно вынести или не нужно?

                                newCamValue.x = Mathf.Clamp(value.x, currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x < 0f ?
                                                                     currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x :
                                                                   -(currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x),
                                                                    (currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x) > 0f ?
                                                                    (currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x) :
                                                                   -(currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x));

                                newCamValue.z = Mathf.Clamp(value.z, currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z < 0f ?
                                                                     currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z :
                                                                   -(currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z),
                                                                    (currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) > 0f ?
                                                                    (currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) :
                                                                   -(currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z));

                                residualSpeedTransform = new Vector3(-newCamValue.x, residualSpeedTransform.y, -newCamValue.z);
                            }
                            cameraTransform.position = newCamValue;
                            IsResidual = true;
                        }
                        break;
                    case SceneState.External:
                        cameraTransform.position = new Vector3(value.x, value.y, value.z);
                        break;
                }
            }
        }

        public bool IsResidual { get; private set; }

        Vector2 firstTouchPosition, secondTouchPosition, thirdTouchPosition;

        /// <summary>
        /// Остаточная скорость движения. После прекращения передвижения камеры остается инерция, которая продолжает некоторое время перемещать камеру
        /// </summary>
        Vector3 residualSpeedTransform;

        public void OneTouchTransform()
        {
            float zDeltaPosition = firstTouchPosition.y - Multiplatform.TouchPosition(0).y;
            float xDeltaPosition = firstTouchPosition.x - Multiplatform.TouchPosition(0).x;

            float zLocalPosition = zDeltaPosition;
            float xLocalPosition = xDeltaPosition;

            float rad = (cameraTransform.rotation.eulerAngles.y * Mathf.PI) / 180f;

            float speed = 2f;

            zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad)))* speed;
            xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad)))* speed;

            Vector3 newPosition;

            newPosition.x = CameraPosition.x + xDeltaPosition*Time.deltaTime;
            newPosition.y = CameraPosition.y;
            newPosition.z = CameraPosition.z + zDeltaPosition * Time.deltaTime;

            firstTouchPosition.y = Multiplatform.TouchPosition(0).y;
            firstTouchPosition.x = Multiplatform.TouchPosition(0).x;

            CameraPosition = newPosition;

            ResidualSpeedTransformCalculate();

            void ResidualSpeedTransformCalculate()//Вычисление остаточного пути
            {
                residualSpeedTransform.x = (residualSpeedTransform.x + xDeltaPosition) / 1.2f;
                residualSpeedTransform.z = (residualSpeedTransform.z + zDeltaPosition) / 1.2f;

                IsResidual = true;
            }
        }

        public void ResidualTransform()
        {
            Vector3 transformStorage;

            transformStorage.x = CameraPosition.x + residualSpeedTransform.x * Time.deltaTime;
            transformStorage.y = CameraPosition.y + residualSpeedTransform.y * Time.deltaTime;
            transformStorage.z = CameraPosition.z + residualSpeedTransform.z * Time.deltaTime;

            residualSpeedTransform.x *= 0.95f;//0.95 - поправочный коэфф, может быть стоит вынести его в отдельное место для более удобного редактирования
            residualSpeedTransform.z *= 0.95f;
            residualSpeedTransform.y *= 0.95f;

            if (Mathf.Abs(residualSpeedTransform.x) <= 0.01f) residualSpeedTransform.x = 0f;
            if (Mathf.Abs(residualSpeedTransform.z) <= 0.01f) residualSpeedTransform.z = 0f;
            if (Mathf.Abs(residualSpeedTransform.y) <= 0.01f) residualSpeedTransform.y = 0f;

            if (residualSpeedTransform.x == 0f && residualSpeedTransform.z == 0f && residualSpeedTransform.y == 0f)
            {
                IsResidual = false;
            }
            CameraPosition = transformStorage;
        }

        /// <summary>
        /// Остаточная скорость вращения камеры
        /// </summary>
        float residualSpeedRotate;

        public void ResidualRotate()
        {
            float xDeltaPosition = residualSpeedRotate;

            residualSpeedRotate *= 0.95f;

            if (Mathf.Abs(residualSpeedRotate) <= 0.01f)
            {
                residualSpeedRotate = 0f;

                IsResidual = false;
                return;
            }

            RotateLogic(xDeltaPosition, globalAnchor, currentRadius);
        }

        public void TwoTouchRotate()
        {
            if (Multiplatform.сurrentPlatform == Platform.Pc) TwoTouchRotatePc();
            else if (Multiplatform.сurrentPlatform == Platform.Android) TwoTouchRotateAndroid();

            void TwoTouchRotateAndroid()
            {
                float firstDeltaPosition = Input.GetTouch(0).deltaPosition.y;
                float secondDeltaPosition = Input.GetTouch(1).deltaPosition.y;

                if ((Input.GetTouch(0).position.x > Input.GetTouch(1).position.x) && ((firstDeltaPosition > 0 && secondDeltaPosition < 0.2f) || (firstDeltaPosition < 0 && secondDeltaPosition > -0.2f)))
                {
                    RotateLogic((firstDeltaPosition - secondDeltaPosition), cameraAnchor, localCamRadius);
                    return;
                }
                else if ((Input.GetTouch(0).position.x < Input.GetTouch(1).position.x) && ((firstDeltaPosition > 0 && secondDeltaPosition < 0.2f) || (firstDeltaPosition < 0 && secondDeltaPosition > -0.2f)))/* if (Input.GetTouch(0).position.y < Input.GetTouch(1).position.y) // не обязательное условие*/
                {

                    RotateLogic((secondDeltaPosition - firstDeltaPosition), cameraAnchor, localCamRadius);
                    return;
                }
            }
            void TwoTouchRotatePc()
            {
                float deltaPosition = Input.mousePosition.x - firstTouchPosition.x;
                RotateLogic(deltaPosition, cameraAnchor, localCamRadius);
                firstTouchPosition = Input.mousePosition;
            }
        }


        /// <summary>
        /// Угол вращения камеры вокруг оси Y
        /// </summary>
        float angleRotate;

        void RotateLogic(float xDeltaPosition, Transform anchor, float radius)
        {
            float speedRotate = 5f;

            angleRotate += xDeltaPosition * speedRotate * Time.deltaTime;

            float xPosition = radius * Mathf.Sin(angleRotate * Mathf.PI / 180f) + anchor.position.x;
            float zPosition = radius * Mathf.Cos((angleRotate - 180f) * Mathf.PI / 180f) + anchor.position.z;

            Vector3 rotateStorage;

            rotateStorage.x = xPosition;
            rotateStorage.y = cameraTransform.position.y;
            rotateStorage.z = zPosition;

            CameraPosition = rotateStorage;

            cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, -angleRotate, cameraTransform.rotation.eulerAngles.z);
        }
        public void FirstTouch()
        {
            int touchCount = Multiplatform.TouchCount;
            switch (touchCount)
            {
                case 1:
                    firstTouchPosition = Multiplatform.TouchPosition(0);
                    break;
                case 2:
                    firstTouchPosition = Multiplatform.TouchPosition(0);
                    secondTouchPosition = Multiplatform.TouchPosition(1);

                    oldScaleDistance = Vector2.Distance(secondTouchPosition, firstTouchPosition);
                    break;
                default:
                    break;
            }

            residualSpeedRotate = 0f;
        }

        float oldScaleDistance;

        float FindRadius(Transform anchor) => Vector2.Distance(new Vector2(cameraTransform.position.x, cameraTransform.position.z), new Vector2(anchor.position.x, anchor.position.z)); //Почему паблик?
        public void TwoTouchScale()
        {
            if (Multiplatform.сurrentPlatform == Platform.Pc) TwoTouchScalePC();
            else if (Multiplatform.сurrentPlatform == Platform.Android) TwoTouchScaleAndroid();

            void TwoTouchScalePC()
            {
                float deltaPosition = Input.mousePosition.y - secondTouchPosition.y;

                Vector3 scaleStorage;

                scaleStorage.y = CameraPosition.y + deltaPosition * Time.deltaTime;
                scaleStorage.x = CameraPosition.x;
                scaleStorage.z = CameraPosition.z;

                CameraPosition = scaleStorage;

                secondTouchPosition = Input.mousePosition;

            }
            void TwoTouchScaleAndroid()
            {
                firstTouchPosition = Input.GetTouch(0).position;
                secondTouchPosition = Input.GetTouch(1).position;

                float scaleDistance = Vector2.Distance(secondTouchPosition, firstTouchPosition);
                float currentChangeDistance = oldScaleDistance - scaleDistance;

                oldScaleDistance = scaleDistance;
                float yPosition = currentChangeDistance;

                Vector3 scaleStorage;

                scaleStorage.y = CameraPosition.y + yPosition * Time.deltaTime;
                scaleStorage.x = CameraPosition.x;
                scaleStorage.z = CameraPosition.z;

                CameraPosition = scaleStorage;
            }
        }
        public void OneTouchRotate()
        {
            float deltaPosition = (firstTouchPosition.x - Multiplatform.TouchPosition(0).x);

            float multiplier = 0.01f; //Change this one, to change speed of manual rotate???это тут вообще нужно?

            RotateLogic(deltaPosition * multiplier, globalAnchor, currentRadius);


            ResidualSpeedRotateCalculate();

            void ResidualSpeedRotateCalculate()
            {
                residualSpeedRotate = (residualSpeedRotate + deltaPosition * Time.deltaTime) / 2.5f;
                IsResidual = true;
            }
        }

        /// <summary>
        /// Функция GetPoint должна вызываться в том случае, если необходимо произвести переход из одного состояния сцены в другое, и данный переход
        /// подразумевает изменение положения камеры
        /// </summary>
        /// <param name="sceneState">Состояние, в которое система должна перейти</param>
        public void GetFinalPoint(SceneState sceneState)
        {
            float height = 0f;

            switch (sceneState)
            {
                case SceneState.Normal:
                    height = midNormalCircleHeigh;
                    currentRadius = normalCircleRadius;
                    break;
                case SceneState.External:
                    height = externalCircleHeigh;
                    currentRadius = externalCircleRadius;
                    break;
                case SceneState.Default:
                    height = cameraTransform.position.y;
                    currentRadius = normalCircleRadius;
                    break;
            }

            finalPoint.x = currentRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x;
            finalPoint.y = height;
            finalPoint.z = currentRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z;

            CleanParams();

            void CleanParams()
            {
                oldScaleDistance = 0f;
                residualSpeedRotate = 0f;
                firstTouchPosition = secondTouchPosition = thirdTouchPosition = new Vector3(0f, 0f);
                residualSpeedTransform = new Vector3(0f, 0f, 0f);
                IsResidual = false;
                safeChangeScale = 0f;
            }
        }

        /// <summary>
        /// finalPoint хранит предполагаемую точку, в которую будет перемещаться камера, вычисляется finalPoint в функции GetFinalPoint
        /// </summary>
        Vector3 finalPoint;
        float safeChangeScale;


        public bool ChangeMode()
        {
            float oldSafeChangeScale = safeChangeScale;

            safeChangeScale += 0.001f * Vector3.Distance(cameraTransform.position, finalPoint) * Time.deltaTime;

            Vector3 scaleChangeMode;

            scaleChangeMode.x = Mathf.SmoothStep(cameraTransform.position.x, finalPoint.x, safeChangeScale);
            scaleChangeMode.y = Mathf.SmoothStep(cameraTransform.position.y, finalPoint.y, safeChangeScale);
            scaleChangeMode.z = Mathf.SmoothStep(cameraTransform.position.z, finalPoint.z, safeChangeScale);

            cameraTransform.position = scaleChangeMode;

            if (Mathf.Abs(safeChangeScale - oldSafeChangeScale) <= 0.00001f)
            {
                return true;
            }
            else
            {
                currentRadius = FindRadius(globalAnchor);
                return false;
            }
        }

        public void CircleRotateAutomatic(float timer)
        {
            float automaticSpeed = 0.5f;
            float maxSpeed = 5f;
            float speedRotate = automaticSpeed * (Mathf.Clamp(timer, 0f, maxSpeed) / 10f);

            RotateLogic(speedRotate, globalAnchor, currentRadius);
        }
    }
}
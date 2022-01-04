using System.Collections;
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
        //SceneState sceneState;

        public CameraFunctional(Transform cameraTransform, Transform cameraAnchor, Transform globalAnchor, float minNormalCircleHeigh, float midNormalCircleHeigh, float maxNormalCircleHeigh,
            float externalCircleHeigh, float externalCircleRadius, float normalCircleRadius, SceneState sceneState)
        {
            this.cameraTransform = cameraTransform;
            this.cameraAnchor = cameraAnchor;

            this.externalCircleHeigh = externalCircleHeigh;
            this.minNormalCircleHeigh = minNormalCircleHeigh;
            this.midNormalCircleHeigh = midNormalCircleHeigh;
            this.maxNormalCircleHeigh = maxNormalCircleHeigh;

            this.normalCircleRadius = normalCircleRadius;
            this.externalCircleRadius = externalCircleRadius;

            this.globalAnchor = globalAnchor;

            this.sceneState = sceneState;

        }

        public Vector3 CameraPosition
        {
            get
            {
                return cameraTransform.position;
            }
            set
            {
                Debug.Log(sceneState);
                switch (sceneState)
                {
                    case SceneState.Normal:
                        {
                            Vector3 newCamValue;
                            newCamValue.y = Mathf.Clamp(value.y, minNormalCircleHeigh, maxNormalCircleHeigh);
                            if (value.y >= maxNormalCircleHeigh) residualSpeedTransform = new Vector3(0f, -value.y * 0.02f, 0f);
                            else if (value.y <= minNormalCircleHeigh) residualSpeedTransform = new Vector3(0f, value.y * 0.02f, 0f);

                            if (Vector3.Distance(new Vector3(cameraTransform.position.x, 0f, cameraTransform.position.z), new Vector3(globalAnchor.position.x, 0f, globalAnchor.position.z)) <= normalCircleRadius)
                            {

                                newCamValue.x = value.x;
                                newCamValue.z = value.z;
                            }
                            else
                            {
                                float localAngle = Vector3.SignedAngle(globalAnchor.position - cameraTransform.position, cameraTransform.parent.gameObject.transform.forward, Vector3.up); //вот тут очень много ресурсов ем, нужно вынести или не нужно?

                                newCamValue.x = Mathf.Clamp(value.x, normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x < 0f ?
                                                                     normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x :
                                                                   -(normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x),
                                                                    (normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x) > 0f ?
                                                                    (normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x) :
                                                                   -(normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x));

                                newCamValue.z = Mathf.Clamp(value.z, normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z < 0f ?
                                                                     normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z :
                                                                   -(normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z),
                                                                    (normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) > 0f ?
                                                                    (normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) :
                                                                   -(normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z));

                                residualSpeedTransform = new Vector3(-newCamValue.x * 0.02f, residualSpeedTransform.y, -newCamValue.z * 0.02f);
                            }
                            cameraTransform.position = newCamValue;
                            IsResidual = true;
                        }
                        break;
                    case SceneState.External:
                        cameraTransform.position = new Vector3(value.x,value.y,value.z);
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

            zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
            xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;

            Vector3 newPosition;

            newPosition.x = CameraPosition.x + xDeltaPosition;
            newPosition.y = CameraPosition.y;
            newPosition.z = CameraPosition.z + zDeltaPosition;

            firstTouchPosition.y = Multiplatform.TouchPosition(0).y;
            firstTouchPosition.x = Multiplatform.TouchPosition(0).x;

            CameraPosition = newPosition;

            ResidualSpeedTransformCalculate();

            void ResidualSpeedTransformCalculate()//Вычисление остаточного пути
            {
                //Стоит ли мне ограничивать остаточный путь?
                //residualSpeedTransform.x = Mathf.Clamp((residualSpeedTransform.x + xDeltaPosition) / 2f, -1f, 1f);
                //residualSpeedTransform.y = Mathf.Clamp((residualSpeedTransform.y + zDeltaPosition) / 2f, -1f, 1f);

                residualSpeedTransform.x = (residualSpeedTransform.x + xDeltaPosition) / 1.2f;
                residualSpeedTransform.z = (residualSpeedTransform.z + zDeltaPosition) / 1.2f;

                IsResidual = true;
            }
        }

        public void ResidualTransform()
        {
            Vector3 transformStorage;

            transformStorage.x = CameraPosition.x + residualSpeedTransform.x;
            transformStorage.y = CameraPosition.y + residualSpeedTransform.y;
            transformStorage.z = CameraPosition.z + residualSpeedTransform.z;

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
            float xDeltaPosition = residualSpeedRotate * Mathf.PI / 180f;

            residualSpeedRotate *= 0.95f;

            if (Mathf.Abs(residualSpeedRotate) <= 0.01f)
            {
                residualSpeedRotate = 0f;

                IsResidual = false;
                return;
            }

            RotateLogic(xDeltaPosition, globalAnchor, externalCircleRadius);
        }

        public void TwoTouchRotate()
        {
            float firstTouchXPos = Input.GetTouch(0).deltaPosition.y;
            float secondTouchXPos = Input.GetTouch(1).deltaPosition.y;

            if ((Input.GetTouch(0).position.x > Input.GetTouch(1).position.x) && ((firstTouchXPos > 0 && secondTouchXPos < 0.2f) || (firstTouchXPos < 0 && secondTouchXPos > -0.2f)))
            {
                RotateLogic((firstTouchXPos - secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
                return;
            }
            else if ((Input.GetTouch(0).position.x < Input.GetTouch(1).position.x) && ((firstTouchXPos > 0 && secondTouchXPos < 0.2f) || (firstTouchXPos < 0 && secondTouchXPos > -0.2f)))/* if (Input.GetTouch(0).position.y < Input.GetTouch(1).position.y) // не обязательное условие*/
            {

                RotateLogic((secondTouchXPos - firstTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
                return;
            }

            /*
             * Некоторая честь логики работы с камерой осталась нереализована и недоделана. К сожалению код, идущий ниже вызывает баг с прокруткой камеры,
             * который в данный момент я пофиксить не могу. пока оставлю так.
             */

            //firstTouchXPos = Mathf.Abs(firstTouchXPos) <= 0.2f ? 0f : firstTouchXPos;
            //secondTouchXPos = Mathf.Abs(secondTouchXPos) <= 0.2f ? 0f : secondTouchXPos;

            //if ((firstTouchXPos == 0f && secondTouchXPos < 0.2f) || (firstTouchXPos == 0f && secondTouchXPos > -0.2f) || (firstTouchXPos > -0.2f && secondTouchXPos == 0f) || (firstTouchXPos < 0.2f && secondTouchXPos == 0f))
            //{
            //    Debug.Log("SOSU");
            //    RotateLogic((firstTouchXPos + secondTouchXPos) * Mathf.PI /** 0.7f *// 180f, cameraAnchor, FindRadius(cameraAnchor));
            //    return;
            //}
        }


        /// <summary>
        /// Угол вращения камеры вокруг оси Y
        /// </summary>
        float angleRotate;

        void RotateLogic(float xDeltaPosition, Transform anchor, float radius) // Изменить dxeltaposition, я передаю сюда переменную, умноженную на пи и поделенную на 180, э то можно сделать внутри.
        {
            float speedRotate = 5f;

            angleRotate += xDeltaPosition * speedRotate;

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
            int touchCount = Multiplatform.TouchCount();
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
                case 3:
                    //firstTouchPosition = Multiplatform.TouchPosition(0);
                    //secondTouchPosition = Multiplatform.TouchPosition(1);
                    //thirdTouchPosition = Multiplatform.TouchPosition(2);

                    //float firstSide = Vector2.Distance(firstTouchPosition, secondTouchPosition);
                    //float secondSide = Vector2.Distance(secondTouchPosition, thirdTouchPosition);
                    //float thirdSide = Vector2.Distance(thirdTouchPosition, firstTouchPosition);

                    //oldPerimetr = firstSide + secondSide + thirdSide;
                    break;
                default:
                    break;
            }

            //residualSpeedTransform = new Vector2(0f, 0f);
            //changeModeCapacity = 0f;

            residualSpeedRotate = 0f;
        }

        float oldScaleDistance;

        float FindRadius(Transform anchor) => Vector2.Distance(new Vector2(cameraTransform.position.x, cameraTransform.position.z), new Vector2(anchor.position.x, anchor.position.z)); //Почему паблик?

        public void TwoTouchScale()
        {
            firstTouchPosition = Input.GetTouch(0).position;
            secondTouchPosition = Input.GetTouch(1).position;

            float scaleDistance = Vector2.Distance(secondTouchPosition, firstTouchPosition);
            float currentChangeDistance = oldScaleDistance - scaleDistance;

            oldScaleDistance = scaleDistance;
            float yPosition = currentChangeDistance / 10f * 0.7f;

            Vector3 scaleStorage;

            scaleStorage.y = CameraPosition.y + yPosition;
            scaleStorage.x = CameraPosition.x;
            scaleStorage.z = CameraPosition.z;

            CameraPosition = scaleStorage;

        }

        public void OneTouchRotate() //логика тут не очень работает по какой то причине, посмотреть этот момент
        {
            float xDeltaPosition = Input.GetTouch(0).deltaPosition.x; //Эта логика работает  только для смартфона, сделать возможность использовать это и для мыши

            float multiplier = 0.01f; //Change this one, to change speed of manual rotate???это тут вообще нужно?

            RotateLogic(xDeltaPosition * multiplier, globalAnchor, externalCircleRadius);

            ResidualSpeedRotateCalculate();

            void ResidualSpeedRotateCalculate()
            {
                residualSpeedRotate = (residualSpeedRotate + xDeltaPosition) / 2.5f;
                IsResidual = true;
            }
        }


        /// <summary>
        /// Функция GetPoint должна вызываться в том случае, если необходимо произвести переход из одного состояния сцены в другое, и данный переход
        /// подразумевает изменение положения камеры
        /// </summary>
        /// <param name="state">Состояние, в которое система должна перейти</param>
        public void GetFinalPoint(SceneState state)
        {
            float height = 0f, localRadius = 0f;

            switch (state)
            {
                case SceneState.Normal:
                    height = midNormalCircleHeigh;
                    localRadius = normalCircleRadius;
                    break;
                case SceneState.External:
                    height = externalCircleHeigh;
                    localRadius = externalCircleRadius;
                    break;
                case SceneState.Default:
                    height = cameraTransform.position.y;
                    localRadius = normalCircleRadius;
                    break;
            }

            finalPoint.x = localRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x;
            finalPoint.y = height;
            finalPoint.z = localRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z;

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


        public float ChangeMode(float currentChangeDistance)
        {
            float oldSafeChangeScale = safeChangeScale;

            safeChangeScale += currentChangeDistance * 0.01f * Vector3.Distance(cameraTransform.position, finalPoint);

            Vector3 scaleChangeMode;

            scaleChangeMode.x = Mathf.SmoothStep(cameraTransform.position.x, finalPoint.x, safeChangeScale);
            scaleChangeMode.y = Mathf.SmoothStep(cameraTransform.position.y, finalPoint.y, safeChangeScale);
            scaleChangeMode.z = Mathf.SmoothStep(cameraTransform.position.z, finalPoint.z, safeChangeScale);

            cameraTransform.position = scaleChangeMode;

            return Mathf.Abs(safeChangeScale - oldSafeChangeScale);
        }


        public void CircleRotateAutomatic(float timer)
        {
            float automaticSpeed = 0.5f;
            float maxSpeed = 5f;
            float speedRotate = automaticSpeed * (Mathf.Clamp(timer, 0f, maxSpeed) / 10f) * Mathf.PI / 180f;

            RotateLogic(speedRotate, globalAnchor, externalCircleRadius);
        }

        SceneState sceneState;
        public void ChangeMode(SceneState sceneState)
        {
            this.sceneState = sceneState;
        }
    }
}
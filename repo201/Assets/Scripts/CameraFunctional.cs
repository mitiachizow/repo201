using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using System;


namespace CameraBehavior
{
    public class CameraFunctional
    {
        Transform cameraTransform, cameraAnchor, globalAnchor;
        readonly float minNormalCircleHeigh, midNormalCircleHeigh, maxNormalCircleHeigh, externalCircleHeigh, externalCircleRadius, normalCircleRadius;


        public CameraFunctional(Transform cameraTransform, Transform cameraAnchor, Transform globalAnchor, float minNormalCircleHeigh, float midNormalCircleHeigh, float maxNormalCircleHeigh, 
            float externalCircleHeigh, float externalCircleRadius, float normalCircleRadius)
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

        }

        public bool IsResidual { get; private set; }


        Vector2 firstTouchPosition, secondTouchPosition, thirdTouchPosition;

        /// <summary>
        /// Остаточная скорость движения. После прекращения передвижения камеры остается инерция, которая продолжает некоторое время перемещать камеру
        /// </summary>
        Vector2 residualSpeedTransform;

        public void OneTouchTransform()
        {
            float zDeltaPosition = firstTouchPosition.y - Multiplatform.TouchPosition(0).y;
            float xDeltaPosition = firstTouchPosition.x - Multiplatform.TouchPosition(0).x;

            //if ((Mathf.Abs(zDeltaPosition) >= 200f) || (Mathf.Abs(xDeltaPosition) >= 200f)) //почему тут 200, не очень понятно
            //{
            //    FirstTouch();
            //    //SetResidualNull();
            //    return;
            //}

            float zLocalPosition = zDeltaPosition;
            float xLocalPosition = xDeltaPosition;

            float rad = (cameraTransform.rotation.eulerAngles.y * Mathf.PI) / 180f;

            float speed = 2f;

            zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
            xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;

            Vector3 newPosition;

            newPosition.x = cameraTransform.position.x + xDeltaPosition;
            newPosition.y = cameraTransform.position.y;
            newPosition.z = cameraTransform.position.z + zDeltaPosition;

            firstTouchPosition.y = Multiplatform.TouchPosition(0).y;
            firstTouchPosition.x = Multiplatform.TouchPosition(0).x;

            cameraTransform.position = newPosition;

            ResidualSpeedTransformCalculate();

            void ResidualSpeedTransformCalculate()//Вычисление остаточного пути
            {
                //residualSpeedTransform.x = Mathf.Clamp((residualSpeedTransform.x + xDeltaPosition) / 2f, -1f, 1f);
                //residualSpeedTransform.y = Mathf.Clamp((residualSpeedTransform.y + zDeltaPosition) / 2f, -1f, 1f);

                residualSpeedTransform.x = (residualSpeedTransform.x + xDeltaPosition) / 1.2f;
                residualSpeedTransform.y = (residualSpeedTransform.y + zDeltaPosition) / 1.2f;

                IsResidual = true;
            }
        }

        public void ResidualTransform()
        {
            Vector3 transformStorage;

            transformStorage.x = cameraTransform.position.x + residualSpeedTransform.x;
            transformStorage.y = cameraTransform.position.y;
            transformStorage.z = cameraTransform.position.z + residualSpeedTransform.y;

            residualSpeedTransform.x *= 0.95f;//0.95 - поправочный коэфф, может быть стоит вынести его в отдельное место для более удобного редактирования
            residualSpeedTransform.y *= 0.95f;

            if (Mathf.Abs(residualSpeedTransform.x) <= 0.01f) residualSpeedTransform.x = 0f;
            if (Mathf.Abs(residualSpeedTransform.y) <= 0.01f) residualSpeedTransform.y = 0f;

            if (residualSpeedTransform.x == 0f && residualSpeedTransform.y == 0f) IsResidual = false;

            cameraTransform.position = transformStorage;
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

            angleRotate += xDeltaPosition * Mathf.PI * speedRotate;

            float xPosition = radius * Mathf.Sin(angleRotate * Mathf.PI / 180f) + anchor.position.x;
            float zPosition = radius * Mathf.Cos((angleRotate - 180f) * Mathf.PI / 180f) + anchor.position.z;

            Vector3 rotateStorage;

            rotateStorage.x = xPosition;
            rotateStorage.y = cameraTransform.position.y;
            rotateStorage.z = zPosition;

            cameraTransform.position = rotateStorage;

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
            float yPosition = cameraTransform.position.y + currentChangeDistance/10f *0.7f;

            Vector3 scaleStorage;

            scaleStorage.y = yPosition;
            scaleStorage.x = cameraTransform.position.x;
            scaleStorage.z = cameraTransform.position.z;

            cameraTransform.position = scaleStorage;
        }
        
        public void OneTouchRotate() //логика тут не очень работает по какой то причине, посмотреть этот момент
        {
            float xDeltaPosition = Input.GetTouch(0).deltaPosition.x; //Эта логика работает  только для смартфона, сделать возможность использовать это и для мыши

            float multiplier = 0.01f; //Change this one, to change speed of manual rotate

            RotateLogic(xDeltaPosition * multiplier, globalAnchor, externalCircleRadius);

            ResidualSpeedRotateCalculate();

            void ResidualSpeedRotateCalculate()
            {
                residualSpeedRotate = (residualSpeedRotate + xDeltaPosition) / 2.5f;
                IsResidual = true;
            }
        }

        //public void CleanParams()
        //{
        //    oldScaleDistance = 0f;
        //    residualSpeedRotate = 0f;
        //    firstTouchPosition = secondTouchPosition = thirdTouchPosition = residualSpeedTransform = new Vector2(0f, 0f);
        //    IsResidual = false;

        //    safeChangeScale = 0f;
        //}


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
            }

            finalPoint.x = localRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x;
            finalPoint.y = height;
            finalPoint.z = localRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z;

            CleanParams();

            void CleanParams()
            {
                oldScaleDistance = 0f;
                residualSpeedRotate = 0f;
                firstTouchPosition = secondTouchPosition = thirdTouchPosition = residualSpeedTransform = new Vector2(0f, 0f);
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
    }
}

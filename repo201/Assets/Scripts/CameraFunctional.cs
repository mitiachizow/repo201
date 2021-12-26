using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using System;


namespace CameraBehavior
{
    public class CameraFunctional
    {
        Transform cameraTransform, cameraAnchor;
        readonly float minCircle, midCircle, maxCircle, externalCircle;


        public CameraFunctional(Transform cameraTransform, Transform cameraAnchor, float minCircle, float midCircle, float maxCircle, float externalCircle)
        {
            this.cameraTransform = cameraTransform;
            this.cameraAnchor = cameraAnchor;

            this.externalCircle = externalCircle;
            this.minCircle = minCircle;
            this.midCircle = midCircle;
            this.maxCircle = maxCircle;

        }

        public bool IsResidual { get; set; }


        Vector2 firstTouchPosition, secondTouchPosition, thirdTouchPosition;
        Vector2 residualSpeedTransform;
        //Остаточная скорость движения. После прекращения передвижения камеры остается инерция, которая продолжает некоторое время перемещать камеру

        public void TransformManual()
        {
            float zDeltaPosition = firstTouchPosition.y - Multiplatform.TouchPosition(0).y;
            float xDeltaPosition = firstTouchPosition.x - Multiplatform.TouchPosition(0).x;

            if ((Mathf.Abs(zDeltaPosition) >= 200f) || (Mathf.Abs(xDeltaPosition) >= 200f)) //почему тут 200, не очень понятно
            {
                FirstTouch();
                //SetResidualNull();
                return;
            }

            float zLocalPosition = zDeltaPosition;
            float xLocalPosition = xDeltaPosition;

            float rad = (cameraTransform.rotation.eulerAngles.y * Mathf.PI) / 180f;

            float speed = 1.5f;

            zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
            xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;

            Vector3 newPosition;

            newPosition.x = cameraTransform.position.x + xDeltaPosition;
            newPosition.y = cameraTransform.position.y;
            newPosition.z = cameraTransform.position.z + zDeltaPosition;

            firstTouchPosition.y = Multiplatform.TouchPosition(0).y;
            firstTouchPosition.x = Multiplatform.TouchPosition(0).x;

            cameraTransform.position = newPosition;

            //Вычисление остаточного пути
            residualSpeedTransform.x = Mathf.Clamp((residualSpeedTransform.x + xDeltaPosition) / 2f, -1f, 1f);
            residualSpeedTransform.y = Mathf.Clamp((residualSpeedTransform.y + zDeltaPosition) / 2f, -1f, 1f);

            IsResidual = true;
        }

        public void TransformResidual()
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

        public void TwoTouchRotate() //В данном месте можно использовать дельтапоз, так как на данный момент ротации мышкой не предусмотрено, и данная функция может быть вызвана только с андроида
        {
            float firstTouchXPos = Input.GetTouch(0).deltaPosition.y;
            float secondTouchXPos = Input.GetTouch(1).deltaPosition.y;

            if ((Input.GetTouch(0).position.x > Input.GetTouch(1).position.x) && ((firstTouchXPos > 0 && secondTouchXPos < 0) || (firstTouchXPos < 0 && secondTouchXPos > 0)))
            {
                RotateLogic((firstTouchXPos - secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
                return;
            }
            else if ((Input.GetTouch(0).position.x < Input.GetTouch(1).position.x) && ((firstTouchXPos > 0 && secondTouchXPos < 0) || (firstTouchXPos < 0 && secondTouchXPos > 0)))/* if (Input.GetTouch(0).position.y < Input.GetTouch(1).position.y) // не обязательное условие*/
            {

                RotateLogic((secondTouchXPos - firstTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
                return;
            }

            Debug.Log(firstTouchXPos + "/" + secondTouchXPos);

            firstTouchXPos = Mathf.Abs(firstTouchXPos) <= 0.1f ? 0f : firstTouchXPos;
            secondTouchXPos = Mathf.Abs(secondTouchXPos) <= 0.1f ? 0f : secondTouchXPos;


            if ((firstTouchXPos == 0f && secondTouchXPos < 0f) || (firstTouchXPos == 0f && secondTouchXPos > 0f) || (firstTouchXPos > 0f && secondTouchXPos == 0f) || (firstTouchXPos < 0f && secondTouchXPos == 0f))
            {
                RotateLogic((firstTouchXPos + secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
                return;
            }

            //if(FirstFingerRightPos && ((firstTouchXPos > 0 && secondTouchXPos < 0) || (firstTouchXPos < 0 && secondTouchXPos > 0)))
            //{
            //    RotateLogic((firstTouchXPos - secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
            //}

            //if (!FirstFingerRightPos && ((firstTouchXPos > 0 && secondTouchXPos < 0) || (firstTouchXPos < 0 && secondTouchXPos > 0)))
            //{
            //    RotateLogic((secondTouchXPos - firstTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
            //}



            //if ((firstTouchXPos > 10f && secondTouchXPos < -10f) || (firstTouchXPos < -10f && secondTouchXPos > 10f)
            //    || (firstTouchXPos == 0f && secondTouchXPos > 10f) || (firstTouchXPos == 0f && secondTouchXPos < -10f) || (firstTouchXPos > 10f && secondTouchXPos == 0f) || (firstTouchXPos < -10f && secondTouchXPos == 0f))
            //{
            //    RotateLogic((firstTouchXPos + secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
            //}

            //if ((firstTouchXPos == 0 && secondTouchXPos >0f) || (firstTouchXPos == 0 && secondTouchXPos < 0f)||(firstTouchXPos > 0 && secondTouchXPos == 0f) || (firstTouchXPos < 0 && secondTouchXPos == 0f)||(firstTouchXPos > 0f && secondTouchXPos < 0) || (firstTouchXPos < 0 && secondTouchXPos > 0))
            //{
            //    RotateLogic((firstTouchXPos + secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
            //}

            //if ((firstTouchXPos == 0 && secondTouchXPos > 0f) || (firstTouchXPos == 0 && secondTouchXPos < 0f) || (firstTouchXPos > 0 && secondTouchXPos == 0f) || (firstTouchXPos < 0 && secondTouchXPos == 0f))
            //{
            //    RotateLogic((firstTouchXPos + secondTouchXPos) * Mathf.PI / 180f, cameraAnchor, FindRadius(cameraAnchor));
            //}
            //else if()
            //{

            //}


            //Debug.Log(yChange /*+ "/" + yChange*/);
        }

        float angleRotate;

        private void RotateLogic(float xDeltaPosition, Transform anchor, float radius) // Изменить dxeltaposition, я передаю сюда переменную, умноженную на пи и поделенную на 180, э то можно сделать внутри.
        {
            float speedRotate = 5f;

            angleRotate += xDeltaPosition * Mathf.PI * speedRotate;

            float xPosition = radius * Mathf.Sin(angleRotate * Mathf.PI / 180f) + anchor.position.x;
            float zPosition = radius * Mathf.Cos((angleRotate - 180f) * Mathf.PI / 180f) + anchor.position.z;

            Vector3 rotateStorage;

            rotateStorage.x = xPosition;
            rotateStorage.y = cameraTransform.position.y;
            rotateStorage.z = zPosition;

            cameraTransform.position = rotateStorage;/* Debug.Log(rotateStorage);*/

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
                    firstTouchPosition = Multiplatform.TouchPosition(0);
                    secondTouchPosition = Multiplatform.TouchPosition(1);
                    thirdTouchPosition = Multiplatform.TouchPosition(2);

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
        }

        float oldScaleDistance;

        public float FindRadius(Transform anchor) => Vector2.Distance(new Vector2(cameraTransform.position.x, cameraTransform.position.z), new Vector2(anchor.position.x, anchor.position.z)); //Почему паблик?

        //void SetRotation() { cameraTransform.rotation = Quaternion.Euler(cameraTransform.position.y /*+ 11f*/, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z); } //Почему 11f


        Vector3 scaleStorage;

        public void ScaleManual()
        {
            firstTouchPosition = Input.GetTouch(0).position;
            secondTouchPosition = Input.GetTouch(1).position;

            float scaleDistance = Vector2.Distance(secondTouchPosition, firstTouchPosition);
            float currentChangeDistance = oldScaleDistance - scaleDistance;



            oldScaleDistance = scaleDistance;

            //float rad = (cameraTransform.rotation.eulerAngles.y * Mathf.PI) / 180f;

            //float zDeltaPosition = -currentChangeDistance /** Mathf.Cos(rad)*/ * Time.deltaTime;
            //float xDeltaPosition = -currentChangeDistance /** Mathf.Sin(rad)*/ * Time.deltaTime;

            //float scaleSize = 0.01f;
            float yPosition = /*Mathf.Clamp(cameraTransform.position.y + currentChangeDistance * scaleSize, minCircle, midCircle)*/ cameraTransform.position.y + currentChangeDistance/10f;
            //float stopScale = 1f;

            //if (scaleStorage.y == yPosition) stopScale = 0f;// Если мы уперлись в потолок по оси y и больше не происходит движение вверх/вниз, тогда мы остонавливаем движение и по остальным осям.
            //else stopScale = 1f;

            scaleStorage.y = yPosition;
            scaleStorage.x = cameraTransform.position.x;
            scaleStorage.z = cameraTransform.position.z;
            //scaleStorage.x = cameraTransform.position.x + xDeltaPosition /** stopScale*/;
            //scaleStorage.z = cameraTransform.position.z + zDeltaPosition /** stopScale*/;

            cameraTransform.position = scaleStorage;

            //SetRotation();
        }


    }
}

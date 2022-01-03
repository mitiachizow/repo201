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
        SceneState sceneState;

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
                switch (sceneState)
                {
                    case SceneState.Normal:
                        {
                            Vector3 newCamValue;

                            newCamValue.y = Mathf.Clamp(value.y, minNormalCircleHeigh, maxNormalCircleHeigh);

                            if (Vector3.Distance(new Vector3(cameraTransform.position.x, 0f, cameraTransform.position.z), new Vector3(globalAnchor.position.x, 0f, globalAnchor.position.z)) <= normalCircleRadius)
                            {

                                newCamValue.x = value.x;
                                newCamValue.z = value.z;
                            }
                            else
                            {
                                float localAngle = Vector3.SignedAngle(globalAnchor.position - cameraTransform.position, cameraTransform.forward, Vector3.up);

                                //ForceBackToPlayground = true;

                                newCamValue.x = Mathf.Clamp(value.x, normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) +
                                    globalAnchor.position.x < 0f ? normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x :
                                    -(normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x),
                                    (normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x) > 0f ?
                                    (normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x) :
                                    -(normalCircleRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x));

                                newCamValue.z = Mathf.Clamp(value.z, normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) +
                                globalAnchor.position.z < 0f ? normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z :
                                -(normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z),
                                 (normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) > 0f ?
                                  (normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) :
                                   -(normalCircleRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z));

                                residualSpeedTransform = new Vector2(-newCamValue.x * 0.02f, -newCamValue.z * 0.02f);
                            }
                            cameraTransform.position = newCamValue;
                            IsResidual = true;
                        }

                        break;
                    case SceneState.External:
                        break;
                }
            }
        }

        public bool IsResidual { get; private set; }
        //public bool ForceBackToPlayground;


        Vector2 firstTouchPosition, secondTouchPosition, thirdTouchPosition;

        /// <summary>
        /// Остаточная скорость движения. После прекращения передвижения камеры остается инерция, которая продолжает некоторое время перемещать камеру
        /// </summary>
        Vector2 residualSpeedTransform;

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

            transformStorage.x = CameraPosition.x + residualSpeedTransform.x;
            transformStorage.y = CameraPosition.y;
            transformStorage.z = CameraPosition.z + residualSpeedTransform.y;

            residualSpeedTransform.x *= 0.95f;//0.95 - поправочный коэфф, может быть стоит вынести его в отдельное место для более удобного редактирования
            residualSpeedTransform.y *= 0.95f;

            if (Mathf.Abs(residualSpeedTransform.x) <= 0.01f) residualSpeedTransform.x = 0f;
            if (Mathf.Abs(residualSpeedTransform.y) <= 0.01f) residualSpeedTransform.y = 0f;

            if (residualSpeedTransform.x == 0f && residualSpeedTransform.y == 0f)
            {
                //ForceBackToPlayground = false;
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

            angleRotate += xDeltaPosition /** Mathf.PI*/ * speedRotate;

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
            float yPosition = /*cameraTransform.position.y + */currentChangeDistance / 10f * 0.7f/** stopStepModify*/;

            //float stopStepModify = 1f;

            //if (cameraTransform.position.y <= minNormalCircleHeigh && (yPosition+ cameraTransform.position.y) < cameraTransform.position.y) stopStepModify = Mathf.SmoothStep(minNormalCircleHeigh / cameraTransform.position.y,0f,1f);
            ////else if (cameraTransform.position.y <= minNormalCircleHeigh && (yPosition + cameraTransform.position.y) > cameraTransform.position.y) stopStepModify = 1f;
            //else if (cameraTransform.position.y >= maxNormalCircleHeigh && (yPosition + cameraTransform.position.y) > cameraTransform.position.y) stopStepModify = Mathf.SmoothStep(minNormalCircleHeigh / cameraTransform.position.y, 0f, 1f);
            ////else if (cameraTransform.position.y >= minNormalCircleHeigh) stopStepModify = (cameraTransform.position.y - midNormalCircleHeigh) / 100f;

            Vector3 scaleStorage;

            scaleStorage.y = CameraPosition.y + yPosition/* * stopStepModify*/;
            scaleStorage.x = CameraPosition.x;
            scaleStorage.z = CameraPosition.z;

            CameraPosition = scaleStorage;

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

            CameraPosition = scaleChangeMode;

            return Mathf.Abs(safeChangeScale - oldSafeChangeScale);
        }


        public void CircleRotateAutomatic(float timer)
        {
            float automaticSpeed = 0.5f;
            float maxSpeed = 5f;
            float speedRotate = automaticSpeed * (Mathf.Clamp(timer, 0f, maxSpeed) / 10f) * Mathf.PI / 180f;

            RotateLogic(speedRotate, globalAnchor, externalCircleRadius);
        }



        //public bool CheckOutOfRangeHorizontal()
        //{
        //    if (/*cameraTransform.position.x > (-externalCircleRadius + globalAnchor.position.x) && cameraTransform.position.x < (externalCircleRadius + globalAnchor.position.x)*/)
        //    {
        //        float freeCoaf = (cameraTransform.position.x - globalAnchor.position.x) * (cameraTransform.position.x - globalAnchor.position.x)
        //            + (globalAnchor.position.z) * (globalAnchor.position.z) - externalCircleRadius * externalCircleRadius;
        //        float discriminant = (-2 * globalAnchor.position.z) * (-2 * globalAnchor.position.z) - 4 * freeCoaf;

        //        float borderOne = (2f * globalAnchor.position.z - Mathf.Sqrt(discriminant)) / 2f;
        //        float borderTwo = (2f * globalAnchor.position.z + Mathf.Sqrt(discriminant)) / 2f;

        //        if ((cameraTransform.position.z > borderOne) && (cameraTransform.position.z < borderTwo))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public bool CheckOutOfRangeVertical() => (cameraTransform.position.y >= maxNormalCircleHeigh || cameraTransform.position.y <= minNormalCircleHeigh) ? true : false;

        //public void StartCoroutineVertical(/*MonoBehaviour monoBehaviour*/)
        //{
        //    //GetFinalPoint(SceneState.Default);
        //    //CountActiveCoroutines++;
        //    //monoBehaviour.StartCoroutine(IVerticalTransform());
        //}

        //IEnumerator IVerticalTransform()
        //{
        //    float stopVerticalTransform = 1f;

        //    residualSpeedTransform = new Vector2(0f, 0f);
        //    bool orientation = false;

        //    //if (cameraTransform.position.y >= midNormalCircleHeigh)
        //    //{
        //    //    orientation = false;
        //    //}
        //    //else if (cameraTransform.position.y <= minNormalCircleHeigh)
        //    //{
        //    //    orientation = true;
        //    //}

        //    while (CheckOutOfRangeVertical())
        //    {
        //        TransformVertical(ref stopVerticalTransform, orientation);
        //        //yield return null;
        //    }

        //    //EndCoroutine();
        //    //yield break;
        //}

        public void TransformVertical()
        {
            float additionalMultiplier = cameraTransform.position.y <= midNormalCircleHeigh ? 1f : -1f;

            Vector3 storageVertical;

            storageVertical.x = cameraTransform.position.x;
            storageVertical.z = cameraTransform.position.z;
            storageVertical.y = cameraTransform.position.y + additionalMultiplier/* * 0.2f*/;

            CameraPosition = storageVertical;

            /*Есть одно замечание по этому скрипту. Скорее деже по этому региону. При отдалении в ручном режиме мы получаем другое отдаление по x и y, нежели при приближении и отдалении при помощи корутина.
             *То есть, находясь в точке (10,10,10), и отдалившить за вертикальную границу зоны, по итогу, корутин нас вернет не в эту же точку, а в точку (11,11,10). В целом, это не критично, и проверки, проводимые
             в других скриптах не должны дать возможность использовать эту неточность для выхода за пределы карты, но в целом, этот скрипт можно(на данный момент в этом нет необходимости) переделать.*/
        }

    }
}





//Vector3 newCamValue;
//float angle = Vector3.Angle(cameraTransform.position - globalAnchor.position, cameraTransform.forward);
////Debug.Log(angle);
////finalPoint.x = localRadius * Mathf.Cos(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x;
////finalPoint.y = height;
////finalPoint.z = localRadius * Mathf.Sin((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z;
////Debug.Log(value.x + "\\" + value.z);
////Debug.Log(-(normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + 0.1f + globalAnchor.position.x)+"///"+ (normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + 0.1f + globalAnchor.position.x));
////Debug.Log(-(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) + "////////" + (normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z));
//newCamValue.x = Mathf.Clamp(value.x,
//    (normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x) > 0
//    ? -(normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x)
//    : (normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x),
//    (normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x) > 0 ?
//    (normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x)
//    : -(normalCircleRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x));
//newCamValue.y = Mathf.Clamp(value.y, minNormalCircleHeigh, maxNormalCircleHeigh);
////newCamValue.x = value.x;
////newCamValue.z = value.z;
////newCamValue.z = Mathf.Clamp(value.z, -(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z), (normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z));
////newCamValue.x = value.x;
//newCamValue.z = Mathf.Clamp(value.z,
//(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) > 0
//? -(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z)
//: (normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z),
//(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z) > 0 ?
//(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z)
//: -(normalCircleRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z));


//cameraTransform.position = newCamValue;


//финальный поинт
//finalPoint.x = localRadius * Mathf.Sin(-cameraTransform.rotation.eulerAngles.y * Mathf.PI / 180f) + globalAnchor.position.x;
//finalPoint.y = height;
//finalPoint.z = localRadius * Mathf.Cos((-cameraTransform.rotation.eulerAngles.y - 180f) * Mathf.PI / 180f) + globalAnchor.position.z;

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//    !!!!!!!!!!!!!!!!!!!!!!!!!!!
//    !!!!!!!!!!!!!!!!!!!!!!!!!!!!
//    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//float rad = (cameraTransform.rotation.eulerAngles.y * Mathf.PI) / 180f;

//float speed = 2f;

//zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
//xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;


//        float freeCoaf = (cameraTransform.position.x - globalAnchor.position.x) * (cameraTransform.position.x - globalAnchor.position.x)
//            + (globalAnchor.position.z) * (globalAnchor.position.z) - externalCircleRadius * externalCircleRadius;
//        float discriminant = (-2 * globalAnchor.position.z) * (-2 * globalAnchor.position.z) - 4 * freeCoaf;

//        float borderOne = (2f * globalAnchor.position.z - Mathf.Sqrt(discriminant)) / 2f;
//        float borderTwo = (2f * globalAnchor.position.z + Mathf.Sqrt(discriminant)) / 2f;

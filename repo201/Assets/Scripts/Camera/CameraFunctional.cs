using UnityEngine;
using SceneBehavior;


namespace CameraBehavior
{
    public class CameraFunctional
    {
        Transform cameraAnchor, globalAnchor, cameraTransform;
        readonly float minNormalCircleHeigh, midNormalCircleHeigh, maxNormalCircleHeigh, externalCircleHeigh, externalCircleRadius, normalCircleRadius;
        float currentRadius, localCamRadius;

        public CameraFunctional(UnityEngine.Transform cameraTransform, UnityEngine.Transform cameraAnchor, UnityEngine.Transform globalAnchor, float minNormalCircleHeigh, float midNormalCircleHeigh, float maxNormalCircleHeigh,
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
                    case SceneState.Building:
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

                                float xPosition = currentRadius * Mathf.Sin(localAngle * Mathf.PI / 180f) + globalAnchor.position.x;
                                float zPosition = currentRadius * Mathf.Cos((localAngle - 180f) * Mathf.PI / 180f) + globalAnchor.position.z;

                                newCamValue.x = Mathf.Clamp(value.x, xPosition < 0f ? xPosition : -xPosition, xPosition > 0f ? xPosition : -xPosition);
                                newCamValue.z = Mathf.Clamp(value.z, zPosition < 0f ? zPosition : -zPosition, zPosition > 0f ? zPosition : -zPosition);

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

        /// <summary>
        /// Остаточная скорость движения. После прекращения передвижения камеры остается инерция, которая продолжает некоторое время перемещать камеру
        /// </summary>
        Vector3 residualSpeedTransform;

        public void OneTouchTransform()
        {
            float rad = (cameraTransform.rotation.eulerAngles.y * Mathf.PI) / 180f;

            float speed = 2f;

            float zDeltaPosition = ((Input2.DeltaPosition[0].y * Mathf.Cos(rad)) - (Input2.DeltaPosition[0].x * Mathf.Sin(rad))) * speed;
            float xDeltaPosition = ((Input2.DeltaPosition[0].x * Mathf.Cos(rad)) + (Input2.DeltaPosition[0].y * Mathf.Sin(rad))) * speed;

            Vector3 newPosition;

            newPosition.x = CameraPosition.x + xDeltaPosition * Time.deltaTime;
            newPosition.y = CameraPosition.y;
            newPosition.z = CameraPosition.z + zDeltaPosition * Time.deltaTime;

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
            if (Input2.CurrentPlatform == Platform.Pc) TwoTouchRotatePc();
            else if (Input2.CurrentPlatform == Platform.Android) TwoTouchRotateAndroid();

            void TwoTouchRotateAndroid()
            {
                RotateLogic((Input2.DeltaPosition[0].y - Input2.DeltaPosition[1].y), cameraAnchor, localCamRadius);
            }

            void TwoTouchRotatePc()
            {
                RotateLogic(-Input2.DeltaPosition[0].x, cameraAnchor, localCamRadius);
            }
        }


        /// <summary>
        /// Угол вращения камеры вокруг вертикальной оси
        /// </summary>
        float angleRotate;

        void RotateLogic(float xDeltaPosition, UnityEngine.Transform anchor, float radius)
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

        float FindRadius(UnityEngine.Transform anchor) => Vector2.Distance(new Vector2(cameraTransform.position.x, cameraTransform.position.z), new Vector2(anchor.position.x, anchor.position.z));
        public void TwoTouchScale()
        {
            if (Input2.CurrentPlatform == Platform.Pc) TwoTouchScalePC();
            else if (Input2.CurrentPlatform == Platform.Android) TwoTouchScaleAndroid();

            void TwoTouchScalePC()
            {
                Vector3 scaleStorage;

                scaleStorage.y = CameraPosition.y + Input2.DeltaPosition[1].y * Time.deltaTime;
                scaleStorage.x = CameraPosition.x;
                scaleStorage.z = CameraPosition.z;

                CameraPosition = scaleStorage;
            }
            void TwoTouchScaleAndroid()
            {
                float scaleDistance = (Input2.CurrentTouchPosition[0].x - Input2.CurrentTouchPosition[1].x) - (Input2.OldTouchPosition[0].x - Input2.OldTouchPosition[1].x);

                Vector3 scaleStorage;

                scaleStorage.y = CameraPosition.y + scaleDistance * Time.deltaTime;
                scaleStorage.x = CameraPosition.x;
                scaleStorage.z = CameraPosition.z;

                CameraPosition = scaleStorage;
            }
        }
        public void OneTouchRotate()
        {
            float multiplier = 0.5f;

            RotateLogic(Input2.DeltaPosition[0].x * multiplier, globalAnchor, currentRadius);

            ResidualSpeedRotateCalculate();

            void ResidualSpeedRotateCalculate()
            {
                residualSpeedRotate = (residualSpeedRotate + Input2.DeltaPosition[0].x) / 2.5f;
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
                case SceneState.Building:
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
                residualSpeedRotate = 0f;
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

        /// <summary>
        /// Set all params default(null) values
        /// </summary>
        public void SetNull()
        {
            IsResidual = false;
            residualSpeedTransform = new Vector3(0f, 0f, 0f);
            residualSpeedRotate = 0f;

        }
    }
}
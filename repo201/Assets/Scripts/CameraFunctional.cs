using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using System;


namespace CameraBehavior
{
    public class CameraFunctional
    {
        CameraParams cameraParams;
        SceneParams sceneParams;

        public CameraFunctional(CameraParams cameraParams, SceneParams sceneParams)
        {
            this.cameraParams = cameraParams;
            this.sceneParams = sceneParams;
        }

        Vector3 transformStorage;
        Vector2 residualSpeedTransform;

        #region Transform

        Vector2 firstTouchPosition, secondTouchPosition, thirdTouchPosition;

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

            float rad = (cameraParams.Position.rotation.eulerAngles.y * Mathf.PI) / 180f;

            float speed = 1.5f;

            zDeltaPosition = ((zLocalPosition * Mathf.Cos(rad)) - (xLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;
            xDeltaPosition = ((xLocalPosition * Mathf.Cos(rad)) + (zLocalPosition * Mathf.Sin(rad))) * Time.deltaTime * speed;

            transformStorage.x = cameraParams.Position.position.x + xDeltaPosition;
            transformStorage.y = cameraParams.Position.position.y;
            transformStorage.z = cameraParams.Position.position.z + zDeltaPosition;

            //residualSpeedTransform.x = Mathf.Clamp((residualSpeedTransform.x + xDeltaPosition) / 2f, -1f, 1f);
            //residualSpeedTransform.y = Mathf.Clamp((residualSpeedTransform.y + zDeltaPosition) / 2f, -1f, 1f);

            firstTouchPosition.y = Multiplatform.TouchPosition(0).y;
            firstTouchPosition.x = Multiplatform.TouchPosition(0).x;

            cameraParams.Position.position = transformStorage;

            //IsResidual = true;
        }

        #endregion


        #region Other

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

                    //oldScaleDistance = Vector2.Distance(secondTouchPosition, firstTouchPosition);
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

        #endregion
    }
}

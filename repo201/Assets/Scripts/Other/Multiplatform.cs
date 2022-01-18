using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


namespace SceneBehavior
{
    /// <summary>
    /// Класс инкапсулирует часть логики взаимодействия, позволяя работать с разными платформами как с одной
    /// </summary>
    public static class Multiplatform
    {
        static Platform сurrentPlatform { get; set; }

        static Multiplatform() => сurrentPlatform = Platform.Android;

        public static int TouchCount
        {
            get
            {
                switch (сurrentPlatform)
                {
                    case Platform.Android:
                        return Input.touchCount;
                    case Platform.Pc:
                        return Input.GetMouseButton(0) ? 1 : 0;
                    case Platform.Ios:
                    default:
                        throw new Exception("Version for IOS not ready yet.");
                }
            }
        }

        public static Vector3 TouchPosition(int i)
        {
            switch (сurrentPlatform)
            {
                case Platform.Android:
                    return GetTouchAndroid(i);
                case Platform.Pc:
                    return i == 0 ? GetTouchPc() : throw new Exception("How can you touch two places with one cursor in one time?");
                case Platform.Ios:
                default:
                    throw new Exception("Version for IOS not ready yet.");

            }

            Vector3 GetTouchPc()
            {
                return Input.mousePosition;
            }

            Vector3 GetTouchAndroid(int j)
            {
                return Input.GetTouch(j).position;
            }
        }

        /// <summary>
        /// Переопределил поведение эвента EventSystem.current.IsPointerOverGameObject для разных платформ, 
        /// логика полностью унаследована.
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverGameObject()
        {
            if (TouchCount == 0) return false;

            switch (сurrentPlatform)
            {
                case Platform.Pc:
                    return IsIsPointerOverGameObjectPC();
                case Platform.Android:
                    return IsIsPointerOverGameObjectAndroid();
                case Platform.Ios:
                default:
                    throw new Exception("Version for IOS not ready yet.");
            }

            bool IsIsPointerOverGameObjectPC()
            {
                return EventSystem.current.IsPointerOverGameObject(0);
            }

            bool IsIsPointerOverGameObjectAndroid()
            {
                if (TouchCount == 1) return EventSystem.current.IsPointerOverGameObject(0);
                else if (TouchCount == 2) return EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1);
                else return true;
            }
        }

        enum Platform
        {
            Android = 1,
            Pc = 2,
            Ios = 3
        }
    }



}
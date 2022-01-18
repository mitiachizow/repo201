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
        public static Platform сurrentPlatform { get; private set; }

        static Multiplatform() => сurrentPlatform = Platform.Pc;

        public static int TouchCount
        {
            get
            {
                switch (сurrentPlatform)
                {
                    case Platform.Android:
                        return Input.touchCount;
                    case Platform.Pc:
                        return Input.GetMouseButton(0) ? Convert.ToInt32(Input.GetMouseButton(0)) + Convert.ToInt32(Input.GetKey(KeyCode.LeftControl)) : 0;
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
                    return GetTouchPositionAndroid(i);
                case Platform.Pc:
                    return GetTouchPositionPc(i);
                case Platform.Ios:
                default:
                    throw new Exception("Version for IOS not ready yet.");

            }

            Vector3 GetTouchPositionPc(int i)
            {
                return Input.mousePosition;
            }

            Vector3 GetTouchPositionAndroid(int j)
            {
                return Input.GetTouch(j).position;
            }
        }

        /// <summary>
        /// Переопределил поведение эвента EventSystem.current.IsPointerOverGameObject для разных платформ, 
        /// логика полностью унаследована.
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUI()
        {
            if (TouchCount == 0) return false;

            switch (сurrentPlatform)
            {
                case Platform.Pc:
                    return IsPointerOverUIPC();
                case Platform.Android:
                    return IsPointerOverUIAndroid();
                case Platform.Ios:
                default:
                    throw new Exception("Version for IOS not ready yet.");
            }

            bool IsPointerOverUIPC()
            {
                return EventSystem.current.IsPointerOverGameObject(0);
            }

            bool IsPointerOverUIAndroid()
            {
                if (TouchCount == 1) return EventSystem.current.IsPointerOverGameObject(0);
                else if (TouchCount == 2) return EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1);
                else return true;
            }
        }


    }

    public enum Platform
    {
        Android = 1,
        Pc = 2,
        Ios = 3
    }

}
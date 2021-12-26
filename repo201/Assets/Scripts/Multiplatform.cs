using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SceneBehavior
{
    public static class Multiplatform
    {

        /*
         * Класс Multiplatform позволяет без проблем работать на любой платформе, просто сменив пункт CurrentPlatform
         */
        private static Platform CurrentPlatform { get; set; }

        static Multiplatform()
        {
            CurrentPlatform = Platform.Android;
        }

        public static int TouchCount()
        {
            switch (CurrentPlatform)
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

        public static Vector3 TouchPosition(int i)
        {
            switch(CurrentPlatform)
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




    }

    public enum Platform
    {
        Android = 1,
        Pc = 2,
        Ios = 3
    }

}
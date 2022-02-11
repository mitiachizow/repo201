using UnityEngine;
using System;
using UnityEngine.EventSystems;


namespace SceneBehavior
{
    /// <summary>
    /// Класс Input2 позволяет скрыть особенности работы с разными платформами, использовать во всей программе единую логику,
    /// скрывая особенности реализации тех или иных методов и свойств, вне зависимости от платформы.
    /// </summary>
    public class Input2 : MonoBehaviour
    {
        public static Platform CurrentPlatform { get; private set; } //Вынести platform в отдельный класс
        public static int OldTouchCount { get; private set; }
        public static int TouchCount { get; private set; }

        public void Start()
        {
            CurrentPlatform = Platform.Pc;

            OldTouchCount = TouchCount = 0;

            OldTouchPosition = new Vector2[2];
            CurrentTouchPosition = new Vector2[2];
            OldTouchPosition[0] = OldTouchPosition[1] = CurrentTouchPosition[0] = CurrentTouchPosition[1] = default;
        }
        public void Update()
        {
            OldTouchCount = TouchCount;
            TouchCount = GetTouchCount();

            if (OldTouchCount == 0 && TouchCount == 0) return;

            CurrentTouchPosition.CopyTo(OldTouchPosition, 0);

            switch (TouchCount)
            {
                case 0:
                    CurrentTouchPosition[0] = CurrentTouchPosition[1] = default;
                    break;
                case 1:
                    CurrentTouchPosition[0] = GetTouchPosition(0);
                    CurrentTouchPosition[1] = default;
                    break;
                case 2:
                    CurrentTouchPosition[0] = GetTouchPosition(0);
                    CurrentTouchPosition[1] = GetTouchPosition(1);

                    if (Input2.CurrentTouchPosition[0].x > Input2.CurrentTouchPosition[1].x)
                    {
                        CurrentTouchPosition[0] = GetTouchPosition(1);
                        CurrentTouchPosition[1] = GetTouchPosition(0);
                    }
                    else
                    {
                        CurrentTouchPosition[0] = GetTouchPosition(0);
                        CurrentTouchPosition[1] = GetTouchPosition(1);
                    }
                    break;
            }
        }

        int GetTouchCount()
        {
            switch (CurrentPlatform)
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

        public static Vector2[] OldTouchPosition { get; private set; }
        public static Vector2[] CurrentTouchPosition { get; private set; }
        public static Vector2[] DeltaPosition
        {
            get
            {
                switch (TouchCount)
                {
                    default:
                    case 0:
                        return new Vector2[] { default, default };
                    case 1:
                        //if (OldTouchPosition[0] == default) return default;
                        return new Vector2[] { OldTouchPosition[0] - CurrentTouchPosition[0], default };
                    case 2:
                        return new Vector2[] { OldTouchPosition[0] - CurrentTouchPosition[0], OldTouchPosition[1] - CurrentTouchPosition[1] };
                }
            }
        }

        public static Vector2 GetTouchPosition(int i)
        {
            switch (CurrentPlatform)
            {
                case Platform.Android:
                    return GetTouchPositionAndroid(i);
                case Platform.Pc:
                    return GetTouchPositionPc(i);
                case Platform.Ios:
                default:
                    throw new Exception("Version for IOS not ready yet.");

            }

            Vector2 GetTouchPositionPc(int i)
            {
                return Input.mousePosition;
            }

            Vector2 GetTouchPositionAndroid(int j)
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

            switch (CurrentPlatform)
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
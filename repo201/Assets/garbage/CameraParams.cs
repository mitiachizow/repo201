using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CameraBehavior
{
    public struct CameraParams
    {
        //public Transform Position { get; private set; }

        //public Transform GlobalAnchor { get; private set; } //Вокруг GlobalAnchor происходит вращение камеры в верхнем положении
        //public Transform LocalAnchor { get; private set; } //Вокруг LocalAnchor происходит вращение камеры в верхнем положении

        //public readonly float minCircle, midCircle, maxCircle, externalCircle;
        ///* lowCircle - минимальная высота, на которую мы можем опуститься с камерой
        // * middleCircle - высота, на которую происходит возвращение камеры из externalCircle
        // * maxCircle - высота, выше которой мы не можем подняться вручную
        // * externalCircle - высота, на которую происходит переход при смене режима игры */

        //public readonly float externalRadius, standartRadius;
        ///* standartRadius - радиус, за который не может заходить камера в обычном режиме
        // * externalRadius - радиус, по которому перемещается камера в верхнем положении
        // * в будущем, когда можно будет добавлять больше регионов в игре, необходимо будет убрать externalRadius*/

    //    public CameraParams(Camera currentCamera, GameObject globalAnchor, GameObject localAnchor, 
    //        float minCircle, float midCircle, float maxCircle, float externalCircle, float externalRadius, float standartRadius)
    //    {
    //        Position = currentCamera.transform;
    //        GlobalAnchor = globalAnchor.transform;
    //        LocalAnchor = globalAnchor.transform;

    //        this.minCircle = minCircle;
    //        this.midCircle = midCircle;
    //        this.maxCircle = maxCircle;
    //        this.externalCircle = externalCircle;

    //        this.externalRadius = externalRadius;
    //        this.standartRadius = standartRadius;
    //    }

    }
}

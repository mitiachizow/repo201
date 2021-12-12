using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneBehavior
{
    public class SceneParams
    {
        public readonly int cellSizeWidth, cellSizeHeight, sizePoint;
        /*подумать над именованием, сейчас неинтуитивное говно*/

        public SceneParams(int cellSizeWidth, int cellSizeHeight, int sizePoint)
        {
            this.cellSizeHeight = cellSizeHeight;
            this.cellSizeWidth = cellSizeWidth;
            this.sizePoint = sizePoint;
        }
    }
}
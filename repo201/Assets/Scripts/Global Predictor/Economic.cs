using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructionBehaviour;
using SceneBehavior;


namespace GlobalPredictor
{
    public class Economic
    {
        float box, boxProduction, boxConsuming;

        public Economic()
        {
            box = 100f;
            boxProduction = 9f;
            boxConsuming = 5f;
        }

        public void Calculate()
        {
            box += boxProduction - boxConsuming;
            //Debug.Log("Boxes :" + box);
        }
    }



}

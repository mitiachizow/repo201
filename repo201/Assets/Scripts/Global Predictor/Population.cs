using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GlobalPredictor
{
    public class Population
    {
        float birthRate, deathRate, population;

        public Population()
        {
            population = 100f;
            birthRate = 1f;
            deathRate = 0.99f;
        }

        public void Calculate()
        {
            //population = population * (birthRate / deathRate);
            //Debug.Log("Population :" + population);
        }
    }

}

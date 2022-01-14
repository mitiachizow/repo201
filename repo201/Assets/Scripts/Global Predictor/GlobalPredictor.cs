using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;

namespace GlobalPredictor
{
    public class GlobalPredictor : MonoBehaviour
    {
        Economic economic;
        Population population;
        //Events events;

        void Start()
        {
            timeUpdate = 0f;
            economic = new Economic();
            population = new Population();
            //events = new Events();
        }

        float timeUpdate;


        void Update()
        {
            timeUpdate += GameTime.DeltaTime;
            if (timeUpdate <= 5f) return;
            timeUpdate = 0f;

            population.Calculate();
            economic.Calculate();
            //events.Calculate();
        }
    }

}
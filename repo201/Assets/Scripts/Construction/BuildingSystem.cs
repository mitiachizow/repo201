using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using UnityEngine.EventSystems;


namespace ConstructionBehaviour
{
    public class BuildingSystem : MonoBehaviour
    {

        Building currentConstruction;


        void Start()
        {
            SceneStateController.AddHandler(Clean);
        }

        public void InstantiateConstruction(ConstructionType constructionType)
        {
            if (currentConstruction == null)
            {
                currentConstruction = new Building(constructionType);
            }
            else
            {
                currentConstruction.ChangeType(constructionType);
            }
        }
        public void CanselInstantiateConstruction()
        {
            GameObject.Find("GUI Controller").GetComponent<GUITransform>().StopTransformGUI();
            currentConstruction.Destroy();
            currentConstruction = null;
        }
        public void AddConstruction()
        {
            currentConstruction.Build();
            currentConstruction = null;
        }

        void Clean()
        {
            if(SceneStateController.CurrentSceneState == SceneState.External)
            {
                currentConstruction?.Destroy();
                currentConstruction = null;
            }
        }
    }

}
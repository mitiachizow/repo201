//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SceneBehavior;
//using UnityEngine.EventSystems;


//namespace ConstructionBehaviour
//{
//    public class BuildingSystem : MonoBehaviour
//    {

//        ConstructionType currentConstruction;
//        //GridPool grid;
//        //LandController land;

//        void Start()
//        {
//            //grid = GridPool.get
//            SceneStateController.AddHandler(Clean);
//        }

//        public void InstantiateConstruction(ConstructionType constructionType)
//        {
//            if (currentConstruction == null)
//            {
//                currentConstruction = new Building(constructionType);
//            }
//            else
//            {
//                currentConstruction.ChangeType(constructionType);
//            }
//        }
//        public void CanselInstantiateConstruction()
//        {
//            GameObject.Find("GUI Pool").GetComponent<GUITransform>().StopTransformGUI();
//            currentConstruction.Destroy();
//            currentConstruction = null;
//        }
//        public void AddConstruction()
//        {
//            currentConstruction.Build();
//            currentConstruction = null;
//        }

//        void Clean()
//        {
//            if(SceneStateController.CurrentSceneState == SceneState.External)
//            {
//                currentConstruction?.Destroy();
//                currentConstruction = null;
//            }
//        }
//    }

//}
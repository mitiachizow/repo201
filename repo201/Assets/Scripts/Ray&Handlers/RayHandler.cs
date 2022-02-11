using ConstructionBehaviour;
using System;
using UnityEngine;

namespace SceneBehavior
{
    /// <summary>
    /// RayCaster создает луч, данные с которого мы далее будем обрабатывать тут.
    /// </summary>
    public class RayHandler : MonoBehaviour
    {
        [SerializeField]
        private ConstructionSystem constructionSystem;
        [SerializeField]
        private GridSystem gridSystem;

        PlaygroundHandler playgroundHandler = PlaygroundHandler.GetPlaygroundHandler();
        UIHandler uiHandler = UIHandler.GetUIHandler();

        private RaycastHit currentHit, oldHit;

        void Update()
        {
            if (!RayCaster.isHit) return;

            if (Input2.OldTouchCount == 1 && Input2.TouchCount == 0 && currentHit.collider.gameObject.name == oldHit.collider.gameObject.name) NotifyClick.Invoke(currentHit.collider.gameObject);//тут вылезает ошибка регулярно, не очень понятно почему

            if (Input2.TouchCount != 1) return;

            if (Input2.TouchCount == 1 && Input2.OldTouchCount == 0) {oldHit = RayCaster.hit; }

            if (Input2.TouchCount == 1 && Input2.OldTouchCount == 1) currentHit = RayCaster.hit;
        }

        void BehaviourLogic()
        {
            switch (currentHit.collider.gameObject.name)
            {
                case "Change View Button":
                    if (SceneStateController.CurrentSceneState == SceneState.External) SceneStateController.ChangeSceneState(SceneState.Normal);
                    else if (SceneStateController.CurrentSceneState == SceneState.Normal) SceneStateController.ChangeSceneState(SceneState.External);
                    break;
                case "Button Confirm":
                    //GameObject.Destroy(GameObject.Find("Building Prefab").GetComponent<TransformBuilding>());
                    //GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().AddConstruction();
                    //GameObject.Find("GUI Pool").GetComponent<GUITransform>().StopTransformGUI();
                    break;
                case "Button Cansel":
                    //GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().CanselInstantiateConstruction();
                    break;
                case "Building Prefab":
                    //GameObject.Find("Building Prefab").GetComponent<TransformBuilding>().isBuildingSelected = true;

                    //currentHit.collider.gameObject.GetComponent<Outline>().enabled = true;
                    break;
                case "Factory Button":
                case "Apartment Button":
                    
                    //GameObject.Find("Construction System").GetComponent<ConstructionBehaviour.ConsructionSystem>().SpawnConstruction(StringToConstructionType(currentHit.collider.gameObject.name));
                    //ButtonPrefab(StringToConstructionType(currentHit.collider.gameObject.name));
                    break;
                case "Add Land Cell Button":
                    GameObject.Find("Land Controller").GetComponent<LandSystem>().AddCell();
                    break;
                default:
                    break;
            }

            //void ButtonPrefab(ConstructionType type)
            //{
            //    if (GameObject.Find("Building Prefab") == null)
            //    {
            //        GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().InstantiateConstruction(type);
            //        GameObject.Find("Building Prefab").AddComponent<Outline>();

            //        GameObject.Find("Building Prefab").GetComponent<TransformBuilding>().isBuildingSelected = true;

            //        GameObject.Find("Building Prefab").GetComponent<Outline>().enabled = true;
            //        GameObject.Find("Building Prefab").GetComponent<Outline>().OutlineWidth = 10;
            //        GameObject.Find("Building Prefab").GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;

            //        GameObject.Find("GUI Pool").GetComponent<GUITransform>().InstantiateButtons();
            //    }
            //    else
            //    {
            //        GameObject.Find("Building Prefab").GetComponent<TransformBuilding>().isBuildingSelected = true;
            //        GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().InstantiateConstruction(type);
            //    }

            //    GameObject.Find("Building Prefab").GetComponent<TransformBuilding>().SetValues();
            //}

            //ConstructionType StringToConstructionType(string type)
            //{
            //    switch (type)
            //    {
            //        case "Factory Button":
            //            return ConstructionType.Factory;
            //        case "Apartment Button":
            //            return ConstructionType.Apartment;
            //    }
            //    throw new Exception("There are no such a building");
            //}
        }


        public delegate void delegateHandler(GameObject gameObject);
        static event delegateHandler NotifyClick;
        public static void AddHandlerClick(delegateHandler funk)
        {
            NotifyClick += funk;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructionBehaviour;
using System;

namespace SceneBehavior
{
    /// <summary>
    /// RayCaster создает луч, данные с которого мы далее будем обрабатывать тут.
    /// </summary>
    public class RayCasterHandler : MonoBehaviour
    {
        private RaycastHit currentHit, oldHit;

        void Update()
        {
            if (!RayCaster.isHit) return;

            if (Input2.OldTouchCount == 1 && Input2.TouchCount == 0 && currentHit.collider.gameObject.name == oldHit.collider.gameObject.name) BehaviourLogic();//тут вылезает ошибка регулярно, не очень понятно почему

            if (Input2.TouchCount != 1) return;

            if (Input2.TouchCount == 1 && Input2.OldTouchCount == 0) oldHit = RayCaster.hit;

            if (Input2.TouchCount == 1 && Input2.OldTouchCount == 1) currentHit = RayCaster.hit;
        }

        void BehaviourLogic()
        {
            switch (currentHit.collider.gameObject.name)
            {
                case "Change View Button":
                    if(SceneStateController.CurrentSceneState == SceneState.External) SceneStateController.ChangeSceneState(SceneState.Normal);
                    else if (SceneStateController.CurrentSceneState == SceneState.Normal) SceneStateController.ChangeSceneState(SceneState.External);
                    break;
                case "Button Confirm":
                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().AddConstruction();
                    GameObject.Find("GUI Controller").GetComponent<GUITransform>().StopTransformGUI();
                    break;
                case "Button Cansel":
                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().CanselInstantiateConstruction();
                    break;
                case "Building Prefab":
                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().isBuildingSelected = true;

                    currentHit.collider.gameObject.GetComponent<Outline>().enabled = true;
                    break;
                case "Factory Button":
                case "Apartment Button":
                    ButtonPrefab(StringToConstructionType(currentHit.collider.gameObject.name));
                    break;
                default:
                    break;
            }

            void ButtonPrefab(ConstructionType type)
            {
                if (GameObject.Find("Building Prefab") == null)
                {
                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().InstantiateConstruction(type);
                    GameObject.Find("Building Prefab").AddComponent<Outline>();

                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().isBuildingSelected = true;

                    GameObject.Find("Building Prefab").GetComponent<Outline>().enabled = true;
                    GameObject.Find("Building Prefab").GetComponent<Outline>().OutlineWidth = 10;
                    GameObject.Find("Building Prefab").GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;

                    GameObject.Find("GUI Controller").GetComponent<GUITransform>().InstantiateButtons();
                }
                else
                {
                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().isBuildingSelected = true;

                    GameObject.Find("Construction Controller").GetComponent<BuildingSystem>().InstantiateConstruction(type);
                }
            }

            ConstructionType StringToConstructionType(string type)
            {
                switch (type)
                {
                    case "Factory Button":
                        return ConstructionType.Factory;
                    case "Apartment Button":
                        return ConstructionType.Apartment;
                }
                throw new Exception("There are no such a building");
            }
        }
    }

}
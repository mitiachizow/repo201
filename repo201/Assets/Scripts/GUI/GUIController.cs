using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructionBehaviour;

namespace SceneBehavior
{
    /// <summary>
    /// Переименовать в раукастерконтроллер?
    /// </summary>
    public class GUIController : MonoBehaviour
    {
        //GameObject confirmButton, canselButton;

        //int oldTouchCount, touchCount;
        RaycastHit currentHit, oldHit;

        void Start()
        {
            //confirmButton = GameObject.Find("");
            //oldTouchCount = touchCount = 0;
        }

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
                case "Button Yes":
                    GameObject.Find("Construction Controller").GetComponent<GridBuildingSystem>().AddNewConstruction();
                    currentHit.collider.gameObject.transform.parent.transform.parent.Find("UI").gameObject.SetActive(false);
                    break;
                case "Button No":
                    GameObject.Destroy( GameObject.Find("Building Prefab")/*gameObject.transform.parent.transform.parent.gameObject*/);
                    GameObject.Find("Construction Controller").GetComponent<GridBuildingSystem>().CanselInstantiateConstruction();
                    break;
                case "Building Prefab":
                    GameObject.Find("Construction Controller").GetComponent<GridBuildingSystem>().isBuildingSelected = true;
                    break;
            }
        }
    }

}
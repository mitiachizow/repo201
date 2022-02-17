using SceneBehavior;
using UnityEngine;
using ConstructionBehaviour;

namespace RayBehaviour
{
    public class PlaygroundHandler : MonoBehaviour
    {
        private void Start()
        {
            RayHandler.AddHandlerClick(BehaviourLogic);
        }

        private GameObject oldGameObject;

        private void BehaviourLogic(GameObject gameObject)
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.Building:
                    BuildingLogic();
                    return;
                case SceneState.Normal:
                    NormalLogic();
                    return;
                default:
                    return;
            }

            void NormalLogic()
            {

                if (oldGameObject != null && gameObject.name != oldGameObject.name)
                {
                    if (oldGameObject.HasComponent<Outline>())
                    {
                        GameObject.Destroy(oldGameObject.GetComponent<Outline>());
                    }
                }

                if (gameObject.tag != "Building" && oldGameObject != null)
                {
                    if(oldGameObject.HasComponent<Outline>())
                    {
                        GameObject.Destroy(oldGameObject.GetComponent<Outline>());
                        GameObject.Find("Info Plane").SetActive(false);//вот это потом переделать
                    }
                    return;
                }
                else if (gameObject.tag != "Building") return;

                oldGameObject = gameObject;

                if (!gameObject.HasComponent<Outline>())
                {
                    gameObject.AddComponent<Outline>().OutlineWidth = 10;
                }

                GameObject.Find("Construction System").GetComponent<ConstructionBehaviour.ConstructionSystem>().AddSelectedConstruction(gameObject); //плохой код
                GameObject.Find("Construction System").GetComponent<ConstructionBehaviour.ConstructionSystem>().GetConstrution(gameObject.transform.position).GetInfo();
            }

            void BuildingLogic()
            {
                if (gameObject.tag != "Building Preview" && oldGameObject != null)
                {
                    GameObject.Destroy(oldGameObject.GetComponent<Outline>());
                    GameObject.Destroy(oldGameObject.GetComponent<BuildingTransform>());
                    return;
                }
                else if (gameObject.tag != "Building Preview") return;

                oldGameObject = gameObject;

                if (!gameObject.HasComponent<Outline>())
                {
                    gameObject.AddComponent<Outline>().OutlineWidth = 10;
                    gameObject.AddComponent<BuildingTransform>();
                }
            }
        }
    }

}

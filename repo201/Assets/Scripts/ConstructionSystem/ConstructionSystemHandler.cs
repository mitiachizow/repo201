using SceneBehavior;
using UnityEngine;
using ConstructionBehaviour;
using UIModules;

namespace RayBehaviour
{
    public class ConstructionSystemHandler : MonoBehaviour
    {
        [SerializeField] private ConstructionSystem constructionSystem;
        [SerializeField] private CanvasController canvasController;
        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private GameObject cameraLogic;

        private void Start() => RayHandler.AddHandler(BehaviourLogic);

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
                    if (oldGameObject.HasComponent<Outline>())
                    {
                        GameObject.Destroy(oldGameObject.GetComponent<Outline>());
                        canvasController.ForceChangeCanvasState(infoTab: false);
                    }
                    return;
                }
                else if (gameObject.tag != "Building") return;

                oldGameObject = gameObject;

                if (!gameObject.HasComponent<Outline>())
                {
                    gameObject.AddComponent<Outline>().OutlineWidth = 10;
                }

                constructionSystem.AddSelectedConstruction(gameObject);
                constructionSystem.GetConstrution(gameObject.transform.position).GetInfo();
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
                    gameObject.AddComponent<BuildingTransform>().Instantiate(gridLayout,cameraLogic);
                }
            }
        }
    }

}

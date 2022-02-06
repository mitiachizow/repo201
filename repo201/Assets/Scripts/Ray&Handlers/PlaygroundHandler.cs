using SceneBehavior;
using UnityEngine;

public class PlaygroundHandler
{
    private PlaygroundHandler()
    {
        RayHandler.AddHandler(BehaviourLogic);
    }

    private static PlaygroundHandler handler;
    public static PlaygroundHandler GetPlaygroundHandler()
    {
        if (handler == null) return handler = new PlaygroundHandler();
        else return handler;
    }


    private GameObject oldGameObject;

    private void BehaviourLogic(GameObject gameObject)
    {
        switch(SceneStateController.CurrentSceneState)
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
            //Тут должно происходить тоже самое, что и в BuildingLogic, только нужно инициализировать свойства, а не играть с играть с их enabled параметрами
        }

        void BuildingLogic()
        {
            if (gameObject.tag != "Building Preview" && oldGameObject != null)
            {
                oldGameObject.GetComponent<Outline>().enabled = false;
                oldGameObject.GetComponent<BuildingTransform>().enabled = false;
                return;
            }
            else if (gameObject.tag != "Building Preview") return;

            oldGameObject = gameObject;
            //gameObject.AddComponent<Outline>().OutlineWidth = 10;
            gameObject.GetComponent<Outline>().enabled = true;
            gameObject.GetComponent<BuildingTransform>().enabled = true;
        }

        void Logic(string tagName)
        {
            if (gameObject.tag != tagName && oldGameObject != null)
            {
                GameObject.Destroy(oldGameObject.GetComponent<Outline>());
                return;
            }
            else if (gameObject.tag != tagName) return;

            oldGameObject = gameObject;
            gameObject.AddComponent<Outline>().OutlineWidth = 10;
            gameObject.AddComponent<BuildingTransform>();
        }
    }
}

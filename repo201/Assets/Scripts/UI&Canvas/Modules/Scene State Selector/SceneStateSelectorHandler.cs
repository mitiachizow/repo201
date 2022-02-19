using UnityEngine;
using RayBehaviour;
using SceneBehavior;


namespace UIModules
{
    public class SceneStateSelectorHandler : MonoBehaviour
    {
        [SerializeField] private GameObject globalButton, normalButton, externalButton, buildingButton;
        [SerializeField] private SceneStateSelector sceneStateSelector;

        private void Start() => RayHandler.AddHandler(BehaviourLogic);

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            if (gameObject.name == globalButton.name)
            {
                sceneStateSelector.SwitchSceneState(SceneState.Global);
            }
            else if(gameObject.name == externalButton.name)
            {
                sceneStateSelector.SwitchSceneState(SceneState.External);
            }
            else if(gameObject.name == normalButton.name)
            {
                sceneStateSelector.SwitchSceneState(SceneState.Normal);
            }
            else if(gameObject.name == buildingButton.name)
            {
                sceneStateSelector.SwitchSceneState(SceneState.Building);
            }
        }
    }

}

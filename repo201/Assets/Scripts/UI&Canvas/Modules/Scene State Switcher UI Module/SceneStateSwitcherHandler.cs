using UnityEngine;
using RayBehaviour;


namespace UIModules
{
    public class SceneStateSwitcherHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject ModeSwitcher;
        [SerializeField]
        private GameObject Global;
        [SerializeField]
        private GameObject Normal;
        [SerializeField]
        private GameObject External;
        [SerializeField]
        private GameObject Building;

        private void Start()
        {
            RayHandler.AddHandlerClick(BehaviourLogic);
        }

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            if(gameObject.name == Global.name || gameObject.name == External.name || gameObject.name == Normal.name || gameObject.name == Building.name)
            {
                ModeSwitcher.GetComponent<ModeSwitcher>().SwitchSceneState(gameObject.GetComponent<NodeStatus>().State);
            }
        }
    }

}

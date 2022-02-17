using UnityEngine;
using ConstructionBehaviour;
using RayBehaviour;

namespace UIModules
{
    public class ConstructionHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject Ticket;
        [SerializeField]
        private GameObject ConstructionSystem;
        [SerializeField]
        private GameObject ButtonBack;
        [SerializeField]
        private GameObject SelectorPosition;
        [SerializeField]
        private GameObject[] buttonsPool;



        private void Start()
        {
            RayHandler.AddHandlerClick(BehaviourLogic);
        }

        private void BehaviourLogic(GameObject gameObject)
        {
            if (gameObject.tag != "UI") return;

            if(gameObject.name == Ticket.name)
            {
                if (SelectorPosition.GetComponent<ConstructionSelectorPosition>().CurrentConstructionSelector == UIPlaneState.Normal)
                {
                    SelectorPosition.GetComponent<ConstructionSelectorPosition>().SetConstructionSelector(UIPlaneState.Hide);
                }
                else if (SelectorPosition.GetComponent<ConstructionSelectorPosition>().CurrentConstructionSelector == UIPlaneState.Hide)
                {
                    SelectorPosition.GetComponent<ConstructionSelectorPosition>().SetConstructionSelector(UIPlaneState.Normal);
                }
            }
            else if(gameObject.name==ButtonBack.name)
            {
                Debug.Log("Go Back");

            }
            else
            {
                foreach(GameObject obj in buttonsPool)
                {
                    if(obj.name == gameObject.name)
                    {
                        ConstructionSystem.GetComponent<ConstructionSystem>().SpawnConstruction(gameObject.GetComponent<ButtonContainer>().Construction);
                        return;
                    }
                }
            }
        }
    }

}

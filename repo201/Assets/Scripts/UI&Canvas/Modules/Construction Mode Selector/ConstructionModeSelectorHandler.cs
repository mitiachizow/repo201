using UnityEngine;
using RayBehaviour;
using UIModules;

public class ConstructionModeSelectorHandler : MonoBehaviour
{
    [SerializeField] private GameObject roadButton, baseConstructions, specialConstructions;
    [SerializeField] private CanvasController canvasController;

    void Start() => RayHandler.AddHandler(BehaviourLogic);

    private void BehaviourLogic(GameObject gameObject)
    {
        if (gameObject.tag != "UI") return;

        if(roadButton.name == gameObject.name)
        {
            Debug.Log("Road");
        }
        else if(baseConstructions.name == gameObject.name)
        {
            canvasController.ForceChangeCanvasState(constructionModeSelector:false,constructionSelector:true);
        }
        else if(specialConstructions.name == gameObject.name)
        {
            Debug.Log("Special Constructions");
        }
    }


}

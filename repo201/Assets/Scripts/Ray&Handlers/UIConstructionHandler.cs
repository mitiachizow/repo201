using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using ConstructionBehaviour;

public class UIConstructionHandler
{
    private static UIConstructionHandler handler;
    private UIConstructionHandler()
    {
        RayHandler.AddHandler(BehaviourLogic);
    }
    public static UIConstructionHandler GetUIHandler()
    {
        if (handler == null) return handler = new UIConstructionHandler();
        else return handler;
    }

    private void BehaviourLogic(GameObject gameObject)
    {
        if (gameObject.tag != "UI Construction Prefab") return;
        GameObject.Find("Construction System").GetComponent<ConstructionSystem>().InitialiseConstruction(gameObject.GetComponent<ButtonContainer>().Construction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneBehavior;
using ConstructionBehaviour;

public class UIHandler
{
    private static UIHandler handler;
    private UIHandler()
    {
        RayHandler.AddHandler(BehaviourLogic);
    }
    public static UIHandler GetUIHandler()
    {
        if (handler == null) return handler = new UIHandler();
        else return handler;
    }

    private void BehaviourLogic(GameObject gameObject)
    {
        switch (gameObject.name)
        {
            case "":
                break;
            default:
                break;

        }

    }
}

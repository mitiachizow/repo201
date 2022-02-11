//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SceneBehavior;
//using ConstructionBehaviour;

//public class ConstructionHandler
//{
//    private static ConstructionHandler handler;
//    private ConstructionHandler()
//    {
//        RayHandler.AddHandlerClick(BehaviourLogic);
//    }
//    public static ConstructionHandler GetUIHandler()
//    {
//        if (handler == null) return handler = new ConstructionHandler();
//        else return handler;
//    }

//    private void BehaviourLogic(GameObject gameObject)
//    {
//        if (gameObject.tag != "Construction Prefab") return;
//        GameObject.Find("Construction System").GetComponent<ConstructionSystem>().InitialiseConstruction(gameObject.GetComponent<ButtonContainer>().Construction);
//    }
//}

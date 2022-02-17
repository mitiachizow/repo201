using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstructionBehaviour
{
    public class ConstructionConstructor : MonoBehaviour
    {
        [SerializeField] private GridLayout gridLayout;
        [SerializeField] private Transform constructionParent;

        public Construction Create(ConstructionPrefab prefab, ConstructionLVL lvl)
        {
            switch(prefab.ConstructionType)
            {
                case ConstructionType.Apartment:
                    return new Apartment(prefab: prefab, сonstructionLvl: lvl, parent: constructionParent, grid: gridLayout, position:new Vector3(0,0,0));
                case ConstructionType.Factory:
                default:
                    return null;
            }
        }
    }
}


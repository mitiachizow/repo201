using UnityEngine;
using ConstructionBehaviour;

namespace UIModules
{
    public class PrefabContainer : MonoBehaviour
    {
        [SerializeField] private ConstructionPrefab construction;

        public ConstructionPrefab Construction
        {
            get { return construction; }
        }
    }

}


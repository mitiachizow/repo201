using UnityEngine;
using ConstructionBehaviour;

namespace UIModules
{
    public class ButtonContainer : MonoBehaviour
    {
        [SerializeField]
        private ConstructionPrefab construction;

        public ConstructionPrefab Construction
        {
            get { return construction; }
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstructionBehaviour
{
    public class ConstructionUILogic : MonoBehaviour
    {
        private void OnMouseUpAsButton()
        {
            switch (this.name)
            {
                case "Button Yes":
                    GameObject.Find("Construction Controller").GetComponent<ConstructionController>().
                        AddNewConstruction(/*gameObject.transform.parent.gameObject*/);


                    gameObject.transform.parent.transform.parent.Find("UI").gameObject.SetActive(false);
                    break;
                case "Button No":
                    GameObject.Destroy(gameObject.transform.parent.transform.parent.gameObject);
                    GameObject.Find("Construction Controller").GetComponent<ConstructionController>().currentConstruction = null ;
                    break;
            }
        }
    }

}
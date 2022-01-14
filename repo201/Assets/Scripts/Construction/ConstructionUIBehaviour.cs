using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ConstructionBehaviour
{
    public class ConstructionUIBehaviour : MonoBehaviour
    {
        GameObject ButtonYes, ButtonNo;
        int localRadius;

        public void Start()
        {
            ButtonYes = GameObject.Find("Button Yes");//поменять на confirm/cansel
            ButtonNo = GameObject.Find("Button No");
            FindRadius();
        }

        public void FindRadius()
        {
            localRadius = GameObject.Find("Construction Controller").GetComponent<ConstructionController>().currentConstruction.xSize >
    GameObject.Find("Construction Controller").GetComponent<ConstructionController>().currentConstruction.zSize ?
    GameObject.Find("Construction Controller").GetComponent<ConstructionController>().currentConstruction.xSize * ConstructionController.PointSize:
    GameObject.Find("Construction Controller").GetComponent<ConstructionController>().currentConstruction.zSize * ConstructionController.PointSize;
        }

        void Update()
        {
            ButtonNo.transform.eulerAngles = new Vector3(-80, Camera.main.transform.eulerAngles.y, 0);
            ButtonYes.transform.eulerAngles = new Vector3(-80, Camera.main.transform.eulerAngles.y, 0);
            ButtonNo.transform.position = FindButtonPosition(20f);
            ButtonYes.transform.position = FindButtonPosition(-20f);
        }


        public Vector3 FindButtonPosition(float additionalAngle)
        {
            float angleRotate = Mathf.Atan2(Camera.main.transform.position.x - gameObject.transform.parent.position.x,
                Camera.main.transform.position.z - gameObject.transform.parent.position.z) * Mathf.Rad2Deg + additionalAngle;

            float xPosition = localRadius * Mathf.Sin((angleRotate) * Mathf.PI / 180f) + gameObject.transform.parent.position.x;
            float zPosition = localRadius * Mathf.Cos((angleRotate) * Mathf.PI / 180f) + gameObject.transform.parent.position.z;

            return new Vector3(xPosition, 13f, zPosition);
        }


    }

}

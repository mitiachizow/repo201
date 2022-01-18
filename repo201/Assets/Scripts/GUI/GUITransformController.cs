using UnityEngine;


namespace SceneBehavior
{
    /// <summary>
    /// Подумать над этим классом, потом его переписать
    /// </summary>
    public class GUITransformController : MonoBehaviour
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
            localRadius = GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.x > GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.z ?
                (int)GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.x : (int)GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.z;
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

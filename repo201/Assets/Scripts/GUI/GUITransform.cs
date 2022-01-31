using UnityEngine;
using System.Collections;


namespace SceneBehavior
{
    /// <summary>
    /// Класс отвечает за transform и rotation существующих кнопок.
    /// </summary>
    public class GUITransform : MonoBehaviour
    {
        GameObject ButtonConfirm, ButtonCansel;

        Transform transformAnchor;
        float radius;

        Coroutine coroutine;

        public void Start()
        {
            SceneStateController.AddHandler(Clean);
            ButtonConfirm = GameObject.Find("Button Confirm");
            ButtonCansel = GameObject.Find("Button Cansel");

            //FindRadius();
        }

        public void InstantiateButtons()
        {
            if (coroutine != null) //а это тут вообще нужно?
            {
                FindRadius();
                return;
            }

            transformAnchor = GameObject.Find("Building Prefab").transform;
            FindRadius();
            coroutine = StartCoroutine(TransformGUI());

            void FindRadius()
            {
                radius = GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.x > GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.z ?
                    GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.x : GameObject.Find("Building Prefab").transform.Find("3D Model").transform.localScale.z;
            }
        }

        private IEnumerator TransformGUI()
        {
            while (true)
            {
                ButtonCansel.transform.eulerAngles = new Vector3(-80, Camera.main.transform.eulerAngles.y, 0);
                ButtonConfirm.transform.eulerAngles = new Vector3(-80, Camera.main.transform.eulerAngles.y, 0);
                ButtonCansel.transform.position = FindGUIButtonPosition(20f);
                ButtonConfirm.transform.position = FindGUIButtonPosition(-20f);
                yield return null;
            }


        }
        public void StopTransformGUI()
        {
            StopCoroutine(coroutine);
            ButtonCansel.transform.position = new Vector3(0f, 1000f, 0f);
            ButtonConfirm.transform.position = new Vector3(0f, 1000f, 0f);
            coroutine = null;
        }

        void Clean()
        {
            if(SceneStateController.CurrentSceneState == SceneState.External)
            {
                if (coroutine != null) StopCoroutine(coroutine);
                ButtonCansel.transform.position = new Vector3(0f, 1000f, 0f);
                ButtonConfirm.transform.position = new Vector3(0f, 1000f, 0f);
                coroutine = null;
            }
        }

        Vector3 FindGUIButtonPosition(float additionalAngle)
        {
            float angleRotate = Mathf.Atan2(Camera.main.transform.position.x - transformAnchor.position.x,
                Camera.main.transform.position.z - transformAnchor.position.z) * Mathf.Rad2Deg + additionalAngle;

            float xPosition = radius * Mathf.Sin((angleRotate) * Mathf.PI / 180f) + transformAnchor.position.x;
            float zPosition = radius * Mathf.Cos((angleRotate) * Mathf.PI / 180f) + transformAnchor.position.z;

            return new Vector3(xPosition, 13f, zPosition);
        }
    }

}

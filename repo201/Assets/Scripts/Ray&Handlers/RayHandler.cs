using UnityEngine;
using SceneBehavior;

namespace RayBehaviour
{
    /// <summary>
    /// Rayhandler является интерфейсом для взаимодействия с RayCaster.
    /// Содержит в себе логику нажатия, для получения события необходимо подписаться на event, используя AddHandler
    /// </summary>
    public class RayHandler : MonoBehaviour
    {
        private RaycastHit currentHit, oldHit;

        void Update()
        {
            if (!RayCaster.IsHit) return;

            if (Input2.OldTouchCount == 1 && Input2.TouchCount == 0 && currentHit.collider?.gameObject.name == oldHit.collider?.gameObject.name) NotifyClick.Invoke(currentHit.collider.gameObject);

            if (Input2.TouchCount != 1) return;

            if (Input2.TouchCount == 1 && Input2.OldTouchCount == 0) { oldHit = RayCaster.hit; }

            if (Input2.TouchCount == 1 && Input2.OldTouchCount == 1) currentHit = RayCaster.hit;
        }

        public delegate void delegateHandler(GameObject gameObject);
        private static event delegateHandler NotifyClick;
        public static void AddHandler(delegateHandler funk)
        {
            NotifyClick += funk;
        }
    }

}
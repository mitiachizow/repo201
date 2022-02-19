using UnityEngine;
using SceneBehavior;

namespace UIModules
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject constructionSelector, infoTab, sceneStateSelector, constructionModeSelector;

        private void Start()
        {
            ChangeCanvasState();

            SceneStateController.AddHandler(ChangeCanvasState);
        }
        /// <summary>
        /// ИЗменяет состояние Cancas согласно выбранному режиму
        /// </summary>
        private void ChangeCanvasState()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.External:
                    ForceChangeCanvasState(constructionSelector: false, infoTab: false, sceneStateSelector: true, constructionModeSelector: false);
                    break;
                case SceneState.Normal:
                    ForceChangeCanvasState(constructionSelector: false, infoTab: false, sceneStateSelector: true, constructionModeSelector: false);
                    break;
                case SceneState.Building:
                    ForceChangeCanvasState(constructionSelector: false, infoTab: false, sceneStateSelector: true, constructionModeSelector: true);
                    break;
                case SceneState.Global:
                    ForceChangeCanvasState(constructionSelector: false, infoTab: false, sceneStateSelector: true, constructionModeSelector: false);
                    break;
            }
        }
        /// <summary>
        /// Изменяет состояние отдельных элементов сцены, вне зависимости от текущего состояния остальных элементов и SceneState
        /// </summary>
        /// <param name="constructionSelector"></param>
        /// <param name="infoTab"></param>
        /// <param name="sceneStateSelector"></param>
        /// <param name="constructionModeSelector"></param>
        public void ForceChangeCanvasState(bool? constructionSelector = null, bool? infoTab = null, bool? sceneStateSelector = null, bool? constructionModeSelector = null)
        {
            if (constructionSelector != null) this.constructionSelector.SetActive((bool)constructionSelector);
            if (infoTab != null) this.infoTab.SetActive((bool)infoTab);
            if (sceneStateSelector != null) this.sceneStateSelector.SetActive((bool)sceneStateSelector);
            if (constructionModeSelector != null) this.constructionModeSelector.SetActive((bool)constructionModeSelector);
        }
    }

}
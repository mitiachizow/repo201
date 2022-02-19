using UnityEngine;
using SceneBehavior;

namespace UIModules
{
    public class SceneStateSelector : MonoBehaviour
    {
        private ModeSwitcherState switcherState;

        [SerializeField] private GameObject normalButton, buildingButton, externalButton, globalButton;

        private GameObject selectedButton;
        private GameObject[] nonSelectedButtons;

        /// <summary>
        /// Изменнеие полей selectedButton и nonSelectedButtons
        /// </summary>
        /// <param name="main"></param>
        /// <param name="other"></param>
        private void SetCurrentMain(GameObject main, params GameObject[] other)
        {
            int[] position = new int[] { -90, -180, -270 }; //Vertical button position anchors

            main.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);//Main buttom positions
            main.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

            for (int i = 0; i < other.Length; i++)//Other button positions
            {
                other[i].GetComponent<RectTransform>().sizeDelta = new Vector2(-15, -13);
                other[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, position[i]);
            }

            this.selectedButton = main;
            this.nonSelectedButtons = other;
        }
        /// <summary>
        /// Базовая инициализация полей selectedButton и nonSelectedButtons
        /// </summary>
        private void StartInitialization()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.Building:
                    selectedButton = buildingButton;
                    nonSelectedButtons = new GameObject[] { normalButton, externalButton, globalButton };
                    break;
                case SceneState.External:
                    selectedButton = externalButton;
                    nonSelectedButtons = new GameObject[] { normalButton, buildingButton, globalButton };
                    break;
                case SceneState.Normal:
                    selectedButton = normalButton;
                    nonSelectedButtons = new GameObject[] { buildingButton, externalButton, globalButton };
                    break;
                case SceneState.Global:
                    selectedButton = globalButton;
                    nonSelectedButtons = new GameObject[] { normalButton, externalButton, buildingButton };
                    break;
            }
        }
        /// <summary>
        /// Базовая логика отображения и скрытия интерфейса, не включает в себя отслеживание повторных нажатий, этим занимается CheckForChanges
        /// </summary>
        /// <param name="state"></param>
        private void ShowHideLogic(SceneState state)
        {
            switch (switcherState)
            {
                case ModeSwitcherState.Extended:
                    Hide();
                    switcherState = ModeSwitcherState.Intended;
                    break;
                case ModeSwitcherState.Intended:
                    Show();
                    switcherState = ModeSwitcherState.Extended;
                    break;
            }
        }
        /// <summary>
        /// Отображает все доступные иконки
        /// </summary>
        private void Show()
        {
            selectedButton.SetActive(true);
            foreach (GameObject item in nonSelectedButtons)
            {
                item.SetActive(true);
            }

        }
        /// <summary>
        /// Скрыть все иконки, кроме выбранной
        /// </summary>
        private void Hide()
        {
            selectedButton.SetActive(true);
            foreach (GameObject item in nonSelectedButtons)
            {
                item.SetActive(false);
            }
        }
        /// <summary>
        /// Базовая логика скрипта.
        /// 1.Проверка, 
        /// </summary>
        /// <param name="state"></param>
        public void SwitchSceneState(SceneState state)
        {
            if (SceneStateController.CurrentSceneState == state && switcherState == ModeSwitcherState.Intended)
            {
                Show();
                switcherState = ModeSwitcherState.Extended;
                return;
            }
            else if (SceneStateController.CurrentSceneState == state && switcherState == ModeSwitcherState.Extended)
            {
                Hide();
                switcherState = ModeSwitcherState.Intended;
                return;
            }

            switch (state)
            {
                case SceneState.Building:
                    SetCurrentMain(buildingButton, normalButton, externalButton, globalButton);//Вот тут может быть еще между собой поменять местами кнопки, для лучшего UX
                    break;
                case SceneState.External:
                    SetCurrentMain(externalButton, normalButton, buildingButton, globalButton);
                    break;
                case SceneState.Normal:
                    SetCurrentMain(normalButton, buildingButton, externalButton, globalButton);
                    break;
                case SceneState.Global:
                    SetCurrentMain(globalButton, normalButton, externalButton, buildingButton);
                    break;
                default:
                    return;
            }

            SceneStateController.ChangeSceneState(state);

            ShowHideLogic(state);
        }

        private void Start()
        {
            StartInitialization();

            switcherState = ModeSwitcherState.Intended;

            SetCurrentMain(selectedButton, nonSelectedButtons);

            Hide();
        }

        public enum ModeSwitcherState
        {
            Extended,
            Intended
        }
    }

}


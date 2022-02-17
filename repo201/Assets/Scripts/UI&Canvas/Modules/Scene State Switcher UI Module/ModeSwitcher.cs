using UnityEngine;
using SceneBehavior;

namespace UIModules
{
    public class ModeSwitcher : MonoBehaviour
    {
        private ModeSwitcherState switcherState;

        [Header("Scene Mode Buttons")]
        [SerializeField]
        private GameObject Normal;
        [SerializeField]
        private GameObject Building;
        [SerializeField]
        private GameObject External;
        [SerializeField]
        private GameObject Global;

        private GameObject selected;
        private GameObject[] other;

        /// <summary>
        /// Изменнеие полей selected и other
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

            this.selected = main;
            this.other = other;
        }
        /// <summary>
        /// Базовая инициализация полей selected и other
        /// </summary>
        private void GetBaseMain()
        {
            switch (SceneStateController.CurrentSceneState)
            {
                case SceneState.Building:
                    selected = Building;
                    other = new GameObject[] { Normal, External, Global };
                    break;
                case SceneState.External:
                    selected = External;
                    other = new GameObject[] { Normal, Building, Global };
                    break;
                case SceneState.Normal:
                    selected = Normal;
                    other = new GameObject[] { Building, External, Global };
                    break;
                case SceneState.Global:
                    selected = Global;
                    other = new GameObject[] { Normal, External, Building };
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
            selected.SetActive(true);
            foreach (GameObject item in other)
            {
                item.SetActive(true);
            }

        }
        /// <summary>
        /// Скрыть все иконки, кроме выбранной
        /// </summary>
        private void Hide()
        {
            selected.SetActive(true);
            foreach (GameObject item in other)
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
            if (selected.GetComponent<NodeStatus>().State == state && switcherState == ModeSwitcherState.Intended)
            {
                Show();
                switcherState = ModeSwitcherState.Extended;
                return;
            }
            else if (selected.GetComponent<NodeStatus>().State == state && switcherState == ModeSwitcherState.Extended)
            {
                Hide();
                switcherState = ModeSwitcherState.Intended;
                return;
            }

            switch (state)
            {
                case SceneState.Building:
                    SetCurrentMain(Building, Normal, External, Global);//Вот тут может быть еще между собой поменять местами кнопки, для лучшего UX
                    break;
                case SceneState.External:
                    SetCurrentMain(External, Normal, Building, Global);
                    break;
                case SceneState.Normal:
                    SetCurrentMain(Normal, Building, External, Global);
                    break;
                case SceneState.Global:
                    SetCurrentMain(Global, Normal, External, Building);
                    break;
                default:
                    return;
            }

            SceneStateController.ChangeSceneState(state);

            ShowHideLogic(state);
        }

        private void Start()
        {
            Global.AddComponent<NodeStatus>().State = SceneState.Global;
            Normal.AddComponent<NodeStatus>().State = SceneState.Normal;
            External.AddComponent<NodeStatus>().State = SceneState.External;
            Building.AddComponent<NodeStatus>().State = SceneState.Building;

            GetBaseMain();

            switcherState = ModeSwitcherState.Intended;

            SetCurrentMain(selected, other);

            Hide();
        }

        public enum ModeSwitcherState
        {
            Extended,
            Intended
        }
    }

}


namespace SceneBehavior
{
    public class SceneStateController
    {
        public static SceneState CurrentSceneState { get; private set; }
        public static SceneState OldSceneState { get; private set; }

        public delegate void ChangeStateDelegate();
        static event ChangeStateDelegate Notify;

        static SceneStateController()
        {
            CurrentSceneState = OldSceneState = SceneState.Building;
        }

        public static void ChangeSceneState(SceneState sceneState)
        {
            OldSceneState = CurrentSceneState;
            switch (sceneState)
            {
                case SceneState.External:
                    CurrentSceneState = SceneState.External;
                    Notify.Invoke();
                    break;
                case SceneState.Normal:
                    CurrentSceneState = SceneState.Normal;
                    Notify.Invoke();
                    break;
                case SceneState.Building:
                    CurrentSceneState = SceneState.Building;
                    Notify.Invoke();
                    break;
                //case SceneState.Global:
                //    CurrentSceneState = SceneState.Global;
                //    Notify.Invoke();
                //    break;
            }
        }

        public static void AddHandler(ChangeStateDelegate func)
        {
            Notify += func;
        }
    }
}
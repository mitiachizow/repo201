using UnityEngine;

namespace SceneBehavior
{
    public static class GameTime
    {
        static GameState gameState = GameState.normalMode;
        //Этот таймер используется исключитьльно для экономики
        //Узнать, можно ли переопределить Time.deltatime
        public static float DeltaTime
        {
            get
            {
                switch(gameState)
                {
                    default:
                    case GameState.pause:
                    return 0f;
                    case GameState.normalMode:
                    return Time.deltaTime;
                    case GameState.speedMode:
                    return Time.deltaTime*2f;
                    case GameState.doubleSpeedMode:
                    return Time.deltaTime*4f;
                }
            }
        }
    }

    enum GameState
    {
        pause,
        normalMode,
        speedMode,
        doubleSpeedMode
    }

}
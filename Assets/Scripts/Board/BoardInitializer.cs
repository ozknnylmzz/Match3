using DG.Tweening;
using Match3.Enums;
using Match3.Game;
using Match3.Input;
using Match3.Items;
using Match3.Level;
using Match3.Strategy;
using UnityEngine;

namespace Match3.Boards
{
    public class BoardInitializer : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private ItemGenerator _itemGenerator; 
        [SerializeField] private Match3Game _match3Game;
        [SerializeField] private BoardInputController _inputController;

        private LevelLoader _levelLoader;
        private StrategyConfig _strategyConfig;
        private GameConfig _gameConfig;

        public void Awake()
        {
            ConstructObjects();
            InitializeGame();
        }

        private void InitializeGame()
        {
            DOTween.Init().SetCapacity(500, 500);
            _gameConfig.Initialize();
            _strategyConfig.Initialize(_board, _itemGenerator);
            _board.Initialize();
            _levelGenerator.Initialize(_board, _itemGenerator, _gameConfig);
            _match3Game.Initialize(_strategyConfig, _gameConfig, _board);
            _levelLoader.Initialize(_levelGenerator);
            _inputController.Initialize(_match3Game);

            EventBus.Instance.Publish(BoardEvents.OnBoardInitialized);
        }

        private void ConstructObjects()
        {
            _strategyConfig = new StrategyConfig();
            _gameConfig = new GameConfig();
            _levelLoader = new LevelLoader();
        }
        
        private void OnDisable()
        {
            _inputController.UnsubscribeEvents();
        }

        private void OnDestroy()
        {
            EventBus.Instance.Publish(BoardEvents.OnBoardDestroyed);
        }
    }
}
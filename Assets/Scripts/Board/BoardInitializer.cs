using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
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

            _strategyConfig.Initialize(this,_board,_itemGenerator);
            _gameConfig.Initialize(_itemGenerator);
            _board.Initialize();
            _levelGenerator.Initialize(_board,_itemGenerator,_gameConfig);
            _match3Game.Initialize(this,_strategyConfig,_gameConfig,_board);
            _levelLoader.Initialize(_levelGenerator);
            _inputController.Initialize(_match3Game,_board);
         

            EventManager.Execute(BoardEvents.OnBoardInitialized);

            ItemSelectionManager.Reset(_board);
        }


        private void ConstructObjects()
        {
            _strategyConfig = new StrategyConfig();
            _gameConfig = new GameConfig();
            _levelLoader = new LevelLoader();
        }

      
        private void OnDisable()
        {
            _inputController.UnsubcribeEvents();
        }

        private void OnDestroy()
        {
            EventManager.Execute(BoardEvents.OnBoardDestroyed);
        }
    }
}
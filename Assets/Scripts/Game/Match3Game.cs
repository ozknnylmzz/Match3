using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
{
    public class Match3Game : MonoBehaviour
    {
        private bool _isSwapAllowed = true;

        private IBoard _board;
        private IMatchDataProvider _matchDataProvider;
        private BoardClearStrategy _boardClearStrategy;
        private JobsExecutor _jobsExecutor;


        public bool IsSwapAllowed => _isSwapAllowed;

        public void Initialize(BoardInitializer boardInitializer,StrategyConfig strategyConfig,GameConfig gameConfig,IBoard board)
        {
            _board = board;

            _boardClearStrategy = strategyConfig.BoardClearStrategy;

            _jobsExecutor = new JobsExecutor();

            _matchDataProvider = gameConfig.MatchDataProvider;
        }

        private void OnEnable()
        {
            EventManager.Subscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }


        public async void SwapItemsAsync(GridPosition selectedPosition, GridPosition targetPosition)
        {

            IGridSlot selectedSlot = _board[selectedPosition];
            IGridSlot targetSlot = _board[targetPosition];

          
            await DoNormalSwap(selectedSlot, targetSlot);
        }

        private async UniTask DoNormalSwap(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            // await SwapItemsAnimation(selectedSlot, targetSlot);

            if (IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition, targetSlot.GridPosition))
            {
                // EventManager.Execute(BoardEvents.OnSwapSuccess);

                ItemSelectionManager.Reset(_board);
                _matchClearStrategy.CalculateMatchStrategyJobs(_board, boardMatchData);

                StartJobs();
            }
            else
            {
                SwapItemsBack(selectedSlot, targetSlot);
            }
        }

       

        private async void StartJobs()
        {
            EventManager.Execute(BoardEvents.OnBeforeJobsStart);

            await _jobsExecutor.ExecuteJobsAsync();
            EventManager.Execute(BoardEvents.OnAfterJobsCompleted);
            EnableSwap();
        }

       

        public bool IsMatchDetected(out BoardMatchData boardMatchData, params GridPosition[] gridPositions)
        {
            boardMatchData = _matchDataProvider.GetMatchData(_board, gridPositions);

            return boardMatchData.MatchExists;
        }

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

      
        

        #region Swap

        public void EnableSwap()
        {
            _isSwapAllowed = true;
            EventManager.Execute(BoardEvents.OnSwapAllowed);
        }

        public void DisableSwap()
        {
            _isSwapAllowed = false;
        }

        public void SwapItemsData(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            _itemSwapper.SwapItemsData(selectedSlot, targetSlot);
        }

        private async void SwapItemsBack(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);

            EnableSwap();
        }

        private UniTask SwapItemsAnimation(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            return _itemSwapper.SwapItems(selectedSlot, targetSlot, this);
        }

     

        #endregion
    }
}
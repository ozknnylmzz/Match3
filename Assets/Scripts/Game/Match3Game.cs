using Cysharp.Threading.Tasks;
using Match3.Boards;
using Match3.Enums;
using Match3.Items;
using Match3.Matchs;
using Match3.Jobs;
using UnityEngine;
using Match3.Strategy;

namespace Match3.Game
{
    public class Match3Game : MonoBehaviour
    {
        private IMatchDataProvider _matchDataProvider;
        private IBoard _board;
        private MatchClearStrategy _matchClearStrategy;
        private JobsExecutor _jobsExecutor;
        private ItemSwapper _itemSwapper;
        public bool IsSwapAllowed => _isSwapAllowed;
        private bool _isSwapAllowed = true;

        public void Initialize(StrategyConfig strategyConfig, GameConfig gameConfig, IBoard board)
        {
            _board = board;

            _matchClearStrategy = strategyConfig.MatchClearStrategy;
            _jobsExecutor = new JobsExecutor();
            _itemSwapper = new ItemSwapper();
            _matchDataProvider = gameConfig.MatchDataProvider;
        }

        private void OnEnable()
        {
            EventBus.Instance.Subscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        public async void SwapItemsAsync(GridPosition selectedPosition, GridPosition targetPosition)
        {
            IGridSlot selectedSlot = _board[selectedPosition];
            IGridSlot targetSlot = _board[targetPosition];
            await DoNormalSwap(selectedSlot, targetSlot);
        }
        
        private async UniTask DoNormalSwap(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);

            if (IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition, targetSlot.GridPosition))
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(boardMatchData);

                CheckAutoMatch();
                StartJobs();
            }
            else
            {
                SwapItemsBack(selectedSlot, targetSlot);
            }
        }

        private async void StartJobs()
        {
            EventBus.Instance.Publish(BoardEvents.OnBeforeJobsStart);
            await _jobsExecutor.ExecuteJobsAsync();

            EventBus.Instance.Publish(BoardEvents.OnAfterJobsCompleted);
            EnableSwap();
        }

        private void CheckAutoMatch()
        {
            while (IsMatchDetected(out BoardMatchData boardMatchData, _board.AllGridPositions) )
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(boardMatchData);
            }
        }
        
        public bool IsMatchDetected(out BoardMatchData boardMatchData, params GridPosition[] gridPositions)
        {
            boardMatchData = _matchDataProvider.GetMatchData(_board, gridPositions);

            return boardMatchData.MatchExists;
        }   
     
        private UniTask SwapItemsAnimation(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            return _itemSwapper.SwapItems(selectedSlot, targetSlot, this);
        }

        private async void SwapItemsBack(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            await SwapItemsAnimation(selectedSlot, targetSlot);
            EnableSwap();
        }
      
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        #region Swap

        private void EnableSwap()
        {
            _isSwapAllowed = true;
        }

        public void DisableSwap()
        {
            _isSwapAllowed = false;
        }

        #endregion
    }
}
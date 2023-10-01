using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
{
    public class Match3Game : MonoBehaviour
    {
        private bool _isSwapAllowed = true;

        private IBoard _board;
        private MatchData _matchData;
        private MatchClearStrategy _matchClearStrategy;
        private BoardClearStrategy _boardClearStrategy;
        private JobsExecutor _jobsExecutor;


        public bool IsSwapAllowed => _isSwapAllowed;

        public void Initialize(StrategyConfig strategyConfig, MatchData matchData, IBoard board)
        {
            _board = board;

            _boardClearStrategy = strategyConfig.BoardClearStrategy;
            _matchClearStrategy = strategyConfig.MatchClearStrategy;
            _jobsExecutor = new JobsExecutor();

            _matchData = matchData;
        }

        private void OnEnable()
        {
            EventManager.Subscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(BoardEvents.OnBeforeJobsStart, DisableSwap);
        }


        public async void SwapItemsAsync()
        {
            await DoSwap();
        }

        private async UniTask DoSwap()
        {
            // DisableSwap();

            EnableSwap();
        }


        private async void StartJobs()
        {
            EventManager.Execute(BoardEvents.OnBeforeJobsStart);
            await _jobsExecutor.ExecuteJobsAsync();

            EventManager.Execute(BoardEvents.OnAfterJobsCompleted);
        }

        public void ClearDiagonalSlot()
        {
            _matchData.ClearDiagonal();
        }

        public void IsSameItem(GridPosition gridPosition)
        {
            IGridSlot gridSlot = _board[gridPosition];
            Debug.Log("SetMatchDatas" + gridSlot.Item.ColorType);
            _matchData.SetMatchDatas(gridSlot);
        }

        public void SetDiagonalData(GridPosition gridPosition)
        {
            IGridSlot gridSlot = _board[gridPosition];
            _matchData.SetDiagonalMoveData(BoardHelper.GetDiagonalSlots(gridSlot, _board));
        }
        
        

        public bool CheckDiagonel(GridPosition gridPosition)
        {
            HashSet<IGridSlot> once    = _matchData.GetDiagonalMoveData();
            foreach (IGridSlot slot in once)
            {
                GridPosition position = slot.GridPosition;
              
                Debug.Log($"Row: {position.RowIndex}, Column: {position.ColumnIndex}");
            }

            IGridSlot gridSlot = _board[gridPosition];
            
            Debug.Log("onceT"+gridSlot.GridPosition);
            Debug.Log("onceT"+once.Contains(gridSlot));

            return  once.Contains(gridSlot);
            
        }

        public bool IsMatchDetected()
        {
            return _matchData.CheckMatch();
        }

        public void ClearMatchData()
        {
            _matchData.ClearMatchData();
        }

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        #region Swap

        public void EnableSwap()
        {
            _isSwapAllowed = true;
            Debug.Log("CalculateMatchStrategyJobs" + _matchData.MatchedDataList.Count);

            _matchClearStrategy.CalculateMatchStrategyJobs(_board, _matchData);
            StartJobs();
            // EventManager.Execute(BoardEvents.OnSwapAllowed);
        }

        public void DisableSwap()
        {
            _isSwapAllowed = false;
        }

        #endregion
    }
}
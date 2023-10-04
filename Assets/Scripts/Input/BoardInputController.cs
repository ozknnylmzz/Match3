using System.Collections.Generic;
using UnityEngine;

namespace CasualA.Board
{
    public class BoardInputController : MonoBehaviour
    {
        private Match3Game _match3Game;
        private IBoard _board;

        private GridPosition _selectedGridPosition;
        private GridPosition _targetGridPosition;
        private bool _isDragMode;

        private int _counter = 0;
        private float dragStartTime;
        private GridPosition _lastPosition;

        private MatchData _matchData;

        private Dictionary<GridPosition, float> dragDurations = new Dictionary<GridPosition, float>();

        public void Initialize(Match3Game match3Game, IBoard board, MatchData matchData)
        {
            _match3Game = match3Game;
            _matchData = matchData;
            _board = board;
            SubcribeEvents();
        }

        public void SubcribeEvents()
        {
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Subscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        public void UnsubcribeEvents()
        {
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDown, OnPointerDown);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerUp, OnPointerUp);
            EventManager<Vector2>.Unsubscribe(BoardEvents.OnPointerDrag, OnPointerDrag);
        }

        public void OnPointerDown(Vector2 pointerWorldPos)
        {
            // _matchData.MatchedDataList.Clear();

            _counter = 0;
            _isDragMode = false;
            if (_match3Game.IsPointerOnBoard(pointerWorldPos, out _selectedGridPosition))
            {
                _isDragMode = true;
                // _match3Game.SetMatchData(_selectedGridPosition);
            }
        }

        GridPosition? _lastGridPosition = new GridPosition();

        public void OnPointerDrag(Vector2 pointerWorldPos)
        {
            if (!_isDragMode)
                return;

            #region MyRegion

            // GridPosition currentPos = _board.WorldToGridPosition(pointerWorldPos);
            // if (currentPos == _lastPosition)
            // {
            //     AddDragDuration(currentPos, Time.time - dragStartTime);
            //     dragStartTime = Time.time;
            // }
            // else
            // {
            //     dragStartTime = Time.time;
            //     _lastPosition = currentPos;
            // }

            #endregion

            if (_match3Game.IsPointerOnBoard(pointerWorldPos, out GridPosition targetGridPosition))
            {
                if (!_lastGridPosition.HasValue || !_lastGridPosition.Value.Equals(targetGridPosition))
                {
                    _lastGridPosition = targetGridPosition;


                    _match3Game.IsSameItem(targetGridPosition);

                    _counter++;
                    // Debug.Log("_counter" + _counter);
                }
            }
            else
            {
                _lastGridPosition = null;
            }
        }


        public void OnPointerUp(Vector2 pointerWorldPos)
        {
            if (_counter < 3)
            {
                return;
            }   


            if (_match3Game.CheckMove(_counter))
            {
                if (!_matchData.CheckMove)
                {
                    return;
                }
                _match3Game.SwapItemsAsync();

            }
            //
            // if (_match3Game.IsMatchDetected(_counter))
            // {
            //    
            //     
            //     if (!_match3Game.CheckMixMove(_counter))
            //     {
            //         return;
            //     }
            //     if (!_match3Game.CheckMixMove(_counter))
            //     {
            //         return;
            //     }
            //     // _match3Game.SetDiagonalData();
            //     _match3Game.CheckMove(_counter);
            //     Debug.Log("IsMatchDetected");
            //
            //     return;
            // }

            // _match3Game.ClearMatchData();
            _isDragMode = false;
        }

        void AddDragDuration(GridPosition position, float duration)
        {
            if (dragDurations.ContainsKey(position))
            {
                dragDurations[position] += duration;
            }
            else
            {
                dragDurations[position] = duration;
            }

            // Slot üzerinde ne kadar süre geçirildiğini yazdırma (isteğe bağlı)
            Debug.Log(position + " üzerinde geçirilen toplam süre: " + dragDurations[position] + " saniye");
        }
    }
}
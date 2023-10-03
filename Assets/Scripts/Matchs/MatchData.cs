using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CasualA.Board
{
    public class MatchData
    {
        public GridPosition MatchPosition;
        public int MatchItemId;
        public int DiagonelMoveCount;
        public  List<IGridSlot> MatchedDataList = new();
        public HashSet<IGridSlot> DiagonalMatchedDataList = new();

        public List<IGridSlot> SendDataList = new();

        public bool IsDiagonalMove;

        #region Variables

        private int _matchItemId
        {
            set => MatchItemId = value;
        }
        private int _diagonelMoveCount
        {
            set => DiagonelMoveCount = value;
        }

        #endregion

        public IGridSlot GetElementFromEnd(int indexFromEnd)
        {
            if (indexFromEnd <= 0 || indexFromEnd > MatchedDataList.Count)
            {
                Debug.Log("indexFromEnd" + indexFromEnd);
            }

            return MatchedDataList[MatchedDataList.Count - indexFromEnd];
        }


        public List<IGridSlot> GetLastNElements(int n)
        {
            // MatchedDataList'in null veya n'den küçük olup olmadığını kontrol eder.
            if (MatchedDataList == null || MatchedDataList.Count < n)
            {
                throw new InvalidOperationException("Insufficient elements in MatchedDataList");
            }

            // MatchedDataList'den son n elemanı alır.
            List<IGridSlot> lastNElements = MatchedDataList.GetRange(MatchedDataList.Count - n, n);
            Debug.Log("lastNElements" + lastNElements.Count);
            Debug.Log("MatchedDataList" + MatchedDataList.Count);
            for (int i = 0; i < lastNElements.Count; i++)
            {
                Debug.Log("lastNElements" + lastNElements[i].Item.ColorType);
            }

            return lastNElements;
        }

        public HashSet<IGridSlot> GetSendMatchedDataListAsSet()
        {
            return new HashSet<IGridSlot>(SendDataList);
        }

        public bool CheckOrthogonalMatch( int counter)
        {
            if (CheckSameDiagonalColors(counter))
            {
                return true;
            }

            return false;
        }


        public bool CheckSameDiagonalColors(int counter)
        {
            List<IGridSlot> lastMatch = GetLastNElements(counter);
            if (lastMatch[0].Item.ColorType == lastMatch[^1].Item.ColorType)
            {
                return lastMatch.All(x => x.Item.ColorType == lastMatch[0].Item.ColorType);
            }

            return false;
        }

        public void SetDiagonalMoveData(HashSet<IGridSlot> DiagonalMoveData)
        {
            // DiagonalMatchedDataList.Clear();
            DiagonalMatchedDataList = DiagonalMoveData;
        }

      

        public bool CheckDiagonalPosition(IBoard board)
        {
            if (MatchedDataList.Count<3)
            {
                return false;
            }
            IGridSlot lastElementPosition = MatchedDataList.LastOrDefault();
            HashSet<IGridSlot> sideDirections = BoardHelper.GetSideSlots(MatchedDataList[^2], board);
            foreach (var nearSlot in sideDirections)
            {
                if (nearSlot.GridPosition==lastElementPosition.GridPosition)
                {
                    return true;
                }
            }

            return false;
        }
        

        private List<(int, int)> GenerateCombinations(int counter)
        {
            var combinations = new List<(int, int)>();

            for (int i = counter; i >= 3; i--)
            {
                combinations.Add((i, i - 2));
            }

            return combinations;
        }
        public void SetMatchDatas(IGridSlot gridSlot)
        {
            MatchedDataList.Add(gridSlot);
        }

        public void DiagonalLogic(int counter)
        {
            if (MatchedDataList.Count > 3)
            {
                var groupedByColor = GetLastNElements(counter).GroupBy(item => item.Item.ColorType);

                var mostCommonColorGroup = groupedByColor
                    .OrderByDescending(group => group.Count())
                    .FirstOrDefault();

                if (mostCommonColorGroup != null)
                {
                    SendDataList = mostCommonColorGroup.ToList();
                }
            }
        }

        public void OrthogonalLogic(int counter)
        {
            // SendDataList = new List<IGridSlot>();

            SendDataList = GetLastNElements(counter);
        }
    }
}
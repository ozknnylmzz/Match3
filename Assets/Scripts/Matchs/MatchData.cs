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
        public List<IGridSlot> MatchedDataList=new ();
        public HashSet<IGridSlot> DiagonalMatchedDataList=new ();
        #region Variables


        private int _matchItemId
        {
            set => MatchItemId = value;
        }

        #endregion

        public IGridSlot GetElementFromEnd(int indexFromEnd)
        {
            if(indexFromEnd <= 0 || indexFromEnd > MatchedDataList.Count)
            {
                Debug.Log("indexFromEnd"+indexFromEnd);
            }
            return MatchedDataList[MatchedDataList.Count - indexFromEnd];
        }
    
        public void ClearMatchData()
        {
            MatchedDataList.Clear();
        }
        public List<IGridSlot> GetUniqueMatchedDataList()
        {
            return MatchedDataList
                .GroupBy(x => x) 
                .Where(g => g.Count() == 1) 
                .Select(g => g.Key) 
                .ToList(); 
        }
        public bool CheckMatch()
        {
            if(MatchedDataList.Count < 3)
            {
                return false;
            }

            if (!CheckMatchData())
            {
              return  false;
            }

           
            var firstColorType = (int)MatchedDataList[0].Item.ColorType;
            return MatchedDataList.All(gridSlot => (int)gridSlot.Item.ColorType == firstColorType);
        }

        private bool CheckMatchData()
        {
            IGridSlot lastElement = GetElementFromEnd(1);
            IGridSlot thirdFromLastElement = GetElementFromEnd(3);
           
            return lastElement.Item.ColorType == thirdFromLastElement.Item.ColorType;
        }

        public void SetDiagonalMoveData(HashSet<IGridSlot>DiagonalMoveData)
        {
            DiagonalMatchedDataList.Clear();
            DiagonalMatchedDataList = DiagonalMoveData;
        }

        public bool CheckDiagonalMove()
        {
            GridPosition lastElementPosition = MatchedDataList[^1].GridPosition;

            // Check if any grid position in DiagonalMatchedDataList is equal to lastElementPosition.
            foreach(var diagonalElement in DiagonalMatchedDataList)
            {
                if(diagonalElement.GridPosition.Equals(lastElementPosition))
                {
                    return true;
                }
            }

            return false; 
        }
        
        public HashSet<IGridSlot> GetDiagonalMoveData()
        {
            Debug.Log("DiagonalMatchedDataList"+DiagonalMatchedDataList.Count);
            return DiagonalMatchedDataList;
        }

        public void SetMatchDatas(IGridSlot gridSlot)
        {
            _matchItemId = (int)gridSlot.Item.ColorType;
         
            MatchedDataList.Add(gridSlot);
        }
        public void ClearDiagonal()
        {
            Debug.Log("MatchedDataList"+MatchedDataList.Count);
            if(MatchedDataList.Count > 0) 
            {
                MatchedDataList.RemoveAt(MatchedDataList.Count - 2);
            }
        }

    }
}
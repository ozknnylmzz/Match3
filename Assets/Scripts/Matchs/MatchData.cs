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
            if(MatchedDataList.Count <= 2)
            {
                return false;
            }

           
            var firstColorType = (int)MatchedDataList[0].Item.ColorType;
            return MatchedDataList.All(gridSlot => (int)gridSlot.Item.ColorType == firstColorType);
        }

        public void SetDiagonalMoveData(HashSet<IGridSlot>DiagonalMoveData)
        {
            DiagonalMatchedDataList = DiagonalMoveData;
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
                MatchedDataList.RemoveAt(MatchedDataList.Count - 1);
            }
        }

    }
}
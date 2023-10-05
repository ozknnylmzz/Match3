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
        public List<IGridSlot> MatchedDataList = new();
        public HashSet<IGridSlot> DiagonalMatchedDataList = new();

        public List<IGridSlot> SendDataList = new();

        public bool CheckMove;

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

        public List<IGridSlot> GetRemoveData(List<IGridSlot> matchdataList)
        {
            List<IGridSlot> removeMatchData = new List<IGridSlot>();
            List<IGridSlot> sendData = matchdataList;
            for (int i = 0; i < matchdataList.Count - 2; i++)
            {
                if (sendData[i].Item.ColorType == sendData[i + 1].Item.ColorType)
                {
                    CheckMove = true;
                }
                else
                {
                    if (sendData[i].Item.ColorType == sendData[i + 2].Item.ColorType)
                    {
                        removeMatchData.Add(sendData[i+1]);
                        sendData.RemoveAt(i+1);
                        // MatchedDataList.RemoveAt(i+1);
                        CheckMove = true;
                    }
                    else
                    {
                        CheckMove = false;
                        break;
                    }
                }
            }

            return removeMatchData;
        }

        public bool CheckMatchData(int counter)
        {
           List<IGridSlot> sequenceMatchData= GetLastNElements(counter);
           if (sequenceMatchData[0].Item.ColorType==sequenceMatchData[counter-1].Item.ColorType)
           {
               return true;
           }
               return false;
        }

        public void SendMatchData(int counter)
        {
            List<IGridSlot> removeMatchData = GetRemoveData(GetLastNElements(counter));

            SendDataList = GetLastNElements(counter).Where(item => !removeMatchData.Contains(item)).ToList();
        }


        public IGridSlot GetElementFromEnd(int indexFromEnd)
        {
            return MatchedDataList[MatchedDataList.Count - indexFromEnd];
        }


        public List<IGridSlot> GetLastNElements(int n)
        {
            List<IGridSlot> lastNElements = MatchedDataList.GetRange(MatchedDataList.Count - n, n);

            return lastNElements;
        }

        public HashSet<IGridSlot> GetSendMatchedDataListAsSet()
        {
            return new HashSet<IGridSlot>(SendDataList);
        }

        public void SetMatchDatas(IGridSlot gridSlot)
        {
            MatchedDataList.Add(gridSlot);
        }
    }
}
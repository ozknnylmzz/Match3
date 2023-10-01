using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CasualA.Board
{
    public class MatchDataProvider 
    {
        private readonly MatchData _matchDeta;

        public MatchDataProvider(MatchData matchDeta)
        {
            _matchDeta = matchDeta;
        }


        // public MatchData GetMatchData(IBoard board, params GridPosition[] gridPositions)
        // {
        //
        //     foreach (GridPosition gridPosition in gridPositions)
        //     {
        //         UnionSharedData(gridPosition, board, gridPositions);
        //     }
        //
        //     return _matchDeta;
        // }
        //
        // private void UnionSharedData(GridPosition gridPosition,
        //     IBoard board,
        //     params GridPosition[] gridPositions)
        // {
        //     MatchData matchData = new MatchData(gridPosition, board[gridPosition].ItemId);
        //     List<MatchData> matchDatas = new List<MatchData>() { matchData };
        //
        //     if (IsSharedMatchData(matchData, matchedDataAllSlots.MatchDataList,
        //             out List<MatchData> sharedMatchDatas))
        //     {
        //         foreach (MatchData sharedMatchData in sharedMatchDatas)
        //         {
        //             matchedDataAllSlots.MatchDataList.Remove(sharedMatchData);
        //
        //             matchSequences.UnionWith(sharedMatchData.MatchedSequences);
        //
        //             matchData.SetMatchDatas(matchSequences, gridPosition, board[gridPosition].ItemId,
        //                 gridPositions.Length > 2);
        //         }
        //     }
        // }


        // private bool IsSharedMatchData(MatchData currentMatchData, List<MatchData> matchDataList,
        //     out List<MatchData> sharedMatchData)
        // {
        //     bool isSharedFound = false;
        //     sharedMatchData = new List<MatchData>();
        //     
        //         if (currentMatchData.MatchedGridSlots.Overlaps(matchData.MatchedGridSlots))
        //         {
        //             sharedMatchData.Add(matchData);
        //             isSharedFound = true;
        //         }
        //     
        //
        //     return isSharedFound;
        // }
    }

   
}
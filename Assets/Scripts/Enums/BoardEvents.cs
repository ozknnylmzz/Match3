namespace Match3
{
    public enum BoardEvents
    {
        OnPointerDown,
        OnPointerUp,
        OnPointerDrag,
        OnPointerDownInputBlock,
        OnSwapSuccess,
        OnBeforeJobsStart,
        OnAfterJobsCompleted,
        OnSequenceDataCalculated,
        OnSlotsCalculated,
        
        OnFallDataCalculated,
       
        OnBoardInitialized,
        OnBoardDestroyed,
        OnHandShow,
        OnSwapAllowed,
        OnSwapEnd,
        OnSwapDetected,
        OnTapDetected,
    }

}
namespace CasualA.Board
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
        OnScoreCalculated,
        OnBoosterPointCalculated,
        OnPowerUpDataCalculated,
        OnFallDataCalculated,
        OnBoosterPointRequest,
        OnCloudCityExtraMoveRequest,
        OnCloudCityExtraMoveAccepted,
        OnPlayerTurnFinished,
        OnPlayerMoveEnd,
        OnUseBooster,
        OnBoardInitialized,
        OnBoardDestroyed,
        OnHandShow,
        OnSwapAllowed,
        ActivateDarkBackGrounds,
        OnCreatedPowerUps,
        OnGameFinished,
        OnPlaceMixerRequest,

        OnUsePerk,

        OnSwapDetected,
        OnTapDetected,
        OnAutoShuffle
    }

    public enum CheatEvents
    {
        OnPointerRightClick,
        OnSetCheatItem,
        OnCheckAutoMatchForBooster,
        OnUseCheatBooster
    }

    public enum SoloEvents
    {
        OnPlayerMoveEnd,
        OnClosedVSAnim
    }
}
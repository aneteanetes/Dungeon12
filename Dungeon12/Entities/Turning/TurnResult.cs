namespace Dungeon12.Entities.Turning
{
    internal enum TurnResult
    {
        /// <summary>
        /// Если в бою - "вы уверены что хотите закончить ход?"
        /// </summary>
        Awareness = 1,
        Success = 2,
        RevealedInvisible = 3,
        Failure = 4,
        NewRound = 5,
        Interrupted = 6,
        AwaitUserInput=7,
        UnknownState
    }

    internal static class TurnResultExtensions
    {
        internal static bool IsSuccess(this TurnResult turnResult)
        {
            if (turnResult is  TurnResult.Success)
                return true;

            return false;
        }

        internal static bool IsFailed(this TurnResult turnResult)
        {
            if (turnResult is  TurnResult.RevealedInvisible or TurnResult.Failure or TurnResult.Interrupted or TurnResult.UnknownState)
                return true;

            return false;
        }
    }
}

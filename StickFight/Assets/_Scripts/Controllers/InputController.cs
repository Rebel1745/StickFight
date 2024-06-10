using UnityEngine;

namespace StickFight
{
    public abstract class InputController : ScriptableObject
    {
        public abstract Vector2Int RetrieveMoveInput(bool includeMutedInput);
        public abstract bool RetrieveJumpInput(bool includeMutedInput);
        public abstract bool RetrieveWallClimbInput(bool includeMutedInput);
        public abstract bool RetrieveDashInput(bool includeMutedInput);
        public abstract int RetrieveDashDirection(bool includeMutedInput);
        public abstract void DashFinished();
        public abstract void UpdateInputMuting(bool isMuted);
        public abstract void UpdateInputMuting(bool isMuted, float duration);
        public abstract bool RetrieveIsMutedInput();
        public abstract bool RetrievePunchInput(bool includeMutedInput);
        public abstract void PunchFinished();
        public abstract bool RetrieveKickInput(bool includeMutedInput);
        public abstract void KickFinished();
    }
}

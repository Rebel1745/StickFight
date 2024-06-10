using UnityEngine;

namespace StickFight
{
    [CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
    public class EmptyController : InputController
    {
        public override void DashFinished()
        {

        }

        public override void KickFinished()
        {
            throw new System.NotImplementedException();
        }

        public override void PunchFinished()
        {
            throw new System.NotImplementedException();
        }

        public override int RetrieveDashDirection(bool includeMutedInput)
        {
            return 0;
        }

        public override bool RetrieveDashInput(bool includeMutedInput)
        {
            return false;
        }

        public override bool RetrieveIsMutedInput()
        {
            throw new System.NotImplementedException();
        }

        public override bool RetrieveJumpInput(bool includeMutedInput)
        {
            return false;
        }

        public override bool RetrieveKickInput(bool includeMutedInput)
        {
            throw new System.NotImplementedException();
        }

        public override Vector2Int RetrieveMoveInput(bool includeMutedInput)
        {
            return Vector2Int.zero;
        }

        public override bool RetrievePunchInput(bool includeMutedInput)
        {
            throw new System.NotImplementedException();
        }

        public override bool RetrieveWallClimbInput(bool includeMutedInput)
        {
            return false;
        }

        public override void UpdateInputMuting(bool isMuted)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateInputMuting(bool isMuted, float duration)
        {
            throw new System.NotImplementedException();
        }
    }
}

using UnityEngine;

namespace StickFight
{
    [CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
    public class EmptyController : InputController
    {
        public override void DashFinished()
        {
            
        }

        public override int RetrieveDashDirection(GameObject gameObject)
        {
            return 0;
        }

        public override bool RetrieveDashInput(GameObject gameObject)
        {
            return false;
        }

        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return false;
        }

        public override Vector2 RetrieveMoveInput(GameObject gameObject)
        {
            return Vector2.zero;
        }

        public override bool RetrieveWallClimbInput(GameObject gameObject)
        {
            return false;
        }
    }
}

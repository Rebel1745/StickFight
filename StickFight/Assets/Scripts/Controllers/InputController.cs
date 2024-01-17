using UnityEngine;

namespace StickFight
{
    public abstract class InputController : ScriptableObject
    {
        public abstract Vector2 RetrieveMoveInput(GameObject gameObject);
        public abstract bool RetrieveJumpInput(GameObject gameObject);
        public abstract bool RetrieveWallClimbInput(GameObject gameObject);
        public abstract bool RetrieveDashInput(GameObject gameObject);
        public abstract int RetrieveDashDirection(GameObject gameObject);
        public abstract void DashFinished();
    }
}

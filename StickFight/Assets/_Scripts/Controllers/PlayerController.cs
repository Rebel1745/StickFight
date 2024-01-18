using UnityEngine;
using UnityEngine.InputSystem;

namespace StickFight
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        private PlayerInputActions _inputActions;
        private bool _isJumping, _isWallClimb, _isDashing;
        private int _dashDirection; // -1 = left, 0 = no dash, 1 = right

        private void OnEnable()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Gameplay.Enable();

            // jump
            _inputActions.Gameplay.Jump.started += JumpStarted;
            _inputActions.Gameplay.Jump.canceled += JumpCanceled;

            // wall climb
            _inputActions.Gameplay.WallClimb.started += WallClimbStarted;
            _inputActions.Gameplay.WallClimb.canceled += WallClimbCanceled;

            // dash
            _inputActions.Gameplay.DashLeft.started += DashLeftStarted;
            _inputActions.Gameplay.DashRight.started += DashRightStarted;
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Disable();

            // jump
            _inputActions.Gameplay.Jump.started -= JumpStarted;
            _inputActions.Gameplay.Jump.canceled -= JumpCanceled;

            // wall climb
            _inputActions.Gameplay.WallClimb.started -= WallClimbStarted;
            _inputActions.Gameplay.WallClimb.canceled -= WallClimbCanceled;
            
            // dash
            _inputActions.Gameplay.DashLeft.started -= DashLeftStarted;
            _inputActions.Gameplay.DashRight.started -= DashRightStarted;
            _inputActions = null;
        }

        public override Vector2 RetrieveMoveInput(GameObject gameObject)
        {
            // we can't move if we are dashing
            if (_isDashing) return Vector2.zero;

            return _inputActions.Gameplay.Move.ReadValue<Vector2>();
        }

        private void JumpCanceled(InputAction.CallbackContext context)
        {
            _isJumping = false;
        }

        private void JumpStarted(InputAction.CallbackContext context)
        {
            _isJumping = true;
        }

        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return _isJumping;
        }

        private void WallClimbCanceled(InputAction.CallbackContext context)
        {
            _isWallClimb = false;
        }

        private void WallClimbStarted(InputAction.CallbackContext context)
        {
            _isWallClimb = true;
        }

        public override bool RetrieveWallClimbInput(GameObject gameObject)
        {
            return _isWallClimb;
        }

        private void DashLeftStarted(InputAction.CallbackContext context)
        {
            _isDashing = true;
            _dashDirection = -1;
        }

        private void DashRightStarted(InputAction.CallbackContext context)
        {
            _isDashing = true;
            _dashDirection = 1;
        }

        public override void DashFinished()
        {
            _isDashing = false;
            _dashDirection = 0;
        }

        public override bool RetrieveDashInput(GameObject gameObject)
        {
            return _isDashing;
        }

        public override int RetrieveDashDirection(GameObject gameObject)
        {
            return _dashDirection;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace CardSystem
{
    public class CardInputHandler : MonoBehaviour
    {
        private PlayerInputMap _inputActions;
        public CardManager cardManager;
        public GameObject player;

        //Trigger Thresholds for "Held" input
        private const float TriggerThreshold = 0.5f;

        private void Awake()
        {
            _inputActions = new PlayerInputMap();
        }

        private void OnEnable()
        {
            _inputActions.CardControls.Enable();

            //Slot binds. These are setup for both keyboard and controller
            _inputActions.CardControls.Slot1.performed += ctx => HandleSlotInput(0);
            _inputActions.CardControls.Slot2.performed += ctx => HandleSlotInput(1);
            _inputActions.CardControls.Slot3.performed += ctx => HandleSlotInput(2);
            _inputActions.CardControls.Slot4.performed += ctx => HandleSlotInput(3);
        }

        private void OnDisable()
        {
            _inputActions.CardControls.Disable();
        }

        private void HandleSlotInput(int slotIndex)
        {
            //Read trigger values
            var leftTriggerValue = _inputActions.CardControls.EquipMod.ReadValue<float>();
            var rightTriggerValue = _inputActions.CardControls.DiscardMod.ReadValue<float>();

            //Read keyboard values [CHANGE THIS PLEASE]
            var isLeftShiftHeld = Keyboard.current.leftShiftKey.isPressed;
            var isRightShiftHeld = Keyboard.current.rightShiftKey.isPressed;

            //Check if left modifier is held (equip)
            if (leftTriggerValue > TriggerThreshold || isLeftShiftHeld)
            {
                // Draw card into this slot
                cardManager.DrawCardToSlot(slotIndex);
            }
            //Check if right modifier is held (discard)
            else if (rightTriggerValue > TriggerThreshold || isRightShiftHeld)
            {
                //Discard card from selected slot
                cardManager.DiscardCardFromSlot(slotIndex);
            }
            else
            {
                //Draw card to selected slot
                cardManager.ActivateCardInSlot(slotIndex, player);
            }
        }
    }
}

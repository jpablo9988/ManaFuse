using UnityEngine;
using UnityEngine.InputSystem;

namespace CardSystem
{
    public class CardInputHandler : MonoBehaviour
    {
        private PlayerInputMap _inputActions;
        private CardManager cardManager;
        public DeckManager _deckManager;
        public GameObject player;

        //Trigger Thresholds for "Held" input
        private const float TriggerThreshold = 0.5f;

        private void Awake()
        {
            _inputActions = new PlayerInputMap();
            cardManager = _deckManager.CardManager;
        }

        private void OnEnable()
        {
            _inputActions.CardControls.Enable();
            //Slot binds. These are setup for both keyboard and controller
            _inputActions.CardControls.Slot1.performed += ctx => HandleSlotInput(0);
            _inputActions.CardControls.Slot2.performed += ctx => HandleSlotInput(1);
            _inputActions.CardControls.Slot3.performed += ctx => HandleSlotInput(2);
            _inputActions.CardControls.Slot4.performed += ctx => HandleSlotInput(3);
            _inputActions.CardControls.EquipMod.performed += ctx => _deckManager.ReloadSlots();
        }

        private void OnDisable()
        {
            //Slot binds. These are setup for both keyboard and controller
            _inputActions.CardControls.Slot1.performed -= ctx => HandleSlotInput(0);
            _inputActions.CardControls.Slot2.performed -= ctx => HandleSlotInput(1);
            _inputActions.CardControls.Slot3.performed -= ctx => HandleSlotInput(2);
            _inputActions.CardControls.Slot4.performed -= ctx => HandleSlotInput(3);
            _inputActions.CardControls.EquipMod.performed -= ctx => _deckManager.ReloadSlots();
            _inputActions.CardControls.Disable();
        }

        private void HandleSlotInput(int slotIndex)
        {
            //Read trigger values
            var rightTriggerValue = _inputActions.CardControls.DiscardMod.ReadValue<float>();

            //Read keyboard values 
            var isRightShiftHeld = Keyboard.current.rightShiftKey.isPressed;

            //Check if right modifier is held (discard)
            if (rightTriggerValue > TriggerThreshold || isRightShiftHeld)
            {
                //Discard card from selected slot
                cardManager.DiscardCardFromSlot(slotIndex);
            }
            else
            {
                //Activate card in selected slot
                cardManager.ActivateCardInSlot(slotIndex, player);
            }
        }
    }
}

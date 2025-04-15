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
        public bool discardHeld = false;

        //Trigger Thresholds for "Held" input
        private const float TriggerThreshold = 0.5f;

        private void Awake()
        {
            _inputActions = new PlayerInputMap();
            cardManager = _deckManager.CardManager;
            if (cardManager) return;
            _deckManager = GetComponent<DeckManager>();
            if (_deckManager)
            {
                cardManager = _deckManager.CardManager;
            }
            _deckManager = this.GetComponentInScene(false, out _deckManager);
            if (_deckManager)
            {
                cardManager = _deckManager.CardManager;
            }

            if (cardManager) return;
            //Yeet play mode if we can't find a card manager
            #if UNITY_EDITOR
            Debug.LogError("Failed to find card manager");
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        private void OnEnable() //fix all these at some point
        {
            _inputActions.CardControls.Enable();
            //Slot binds. These are setup for both keyboard and controller
            _inputActions.CardControls.Slot1.performed += ctx => HandleSlotInput(0);
            _inputActions.CardControls.Slot2.performed += ctx => HandleSlotInput(1);
            _inputActions.CardControls.Slot3.performed += ctx => HandleSlotInput(2);
            _inputActions.CardControls.Slot4.performed += ctx => HandleSlotInput(3);
            _inputActions.CardControls.EquipMod.performed += ctx => _deckManager.ReloadSlots();
            _inputActions.CardControls.DiscardMod.performed += ctx => discardHeld = true;
            _inputActions.CardControls.DiscardMod.canceled += ctx => discardHeld = false;
        }

        private void OnDisable()
        {
            //Slot binds. These are setup for both keyboard and controller
            _inputActions.CardControls.Slot1.performed -= ctx => HandleSlotInput(0);
            _inputActions.CardControls.Slot2.performed -= ctx => HandleSlotInput(1);
            _inputActions.CardControls.Slot3.performed -= ctx => HandleSlotInput(2);
            _inputActions.CardControls.Slot4.performed -= ctx => HandleSlotInput(3);
            _inputActions.CardControls.EquipMod.performed -= ctx => _deckManager.ReloadSlots();
            _inputActions.CardControls.DiscardMod.performed -= ctx => discardHeld = false;
            _inputActions.CardControls.Disable();
        }

        private void HandleSlotInput(int slotIndex)
        {
            if (discardHeld)
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

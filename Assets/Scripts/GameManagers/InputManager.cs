using UnityEngine;
using UnityEngine.EventSystems;
using CardSystem;

public class InputManager : MonoBehaviour
{
    [Header("Start Settings")]
    [SerializeField]
    private bool _activateControlsOnEnable = true;
    [SerializeField]
    private bool _activateUIInputsOnEnable = true;
    private PlayerInputHandler playerHandler;
    private CardInputHandler cardHandler;
    private UIInputHandler uiHandler;
    public bool ActivatePlayerInputs
    {
        get { if (playerHandler) return playerHandler.isActiveAndEnabled; else return false; }
        set { if (playerHandler) playerHandler.enabled = value; }
    }
    public bool ActivateCardInputs
    {
        get { if (cardHandler) return cardHandler.isActiveAndEnabled; else return false; }
        set { if (cardHandler) cardHandler.enabled = value; }
    }
    public bool ActivateUIInputs
    {
        get { if (uiHandler) return uiHandler.isActiveAndEnabled; else return false; }
        set { if (uiHandler) uiHandler.enabled = value; }
    }
    public PlayerInputHandler PlayerHandler => playerHandler;
    public CardInputHandler CardHandler => cardHandler;
    public UIInputHandler UIHandler => uiHandler;

    void Awake()
    {
        this.GetComponentInScene<PlayerInputHandler>(false, out playerHandler);
        this.GetComponentInScene<CardInputHandler>(false, out cardHandler);
        this.GetComponentInScene<UIInputHandler>(false, out uiHandler);
    }
    void OnEnable()
    {
        ActivateUIInputs = _activateUIInputsOnEnable;
        ActivatePlayerInputs = _activateControlsOnEnable;
        ActivateCardInputs = _activateControlsOnEnable;
    }
    void OnDisable()
    {
        ActivateUIInputs = false;
        ActivatePlayerInputs = false;
        ActivateCardInputs = false;
    }
}

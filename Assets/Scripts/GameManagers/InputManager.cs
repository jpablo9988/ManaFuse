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
        get { if (playerHandler != null) return playerHandler.isActiveAndEnabled; else return false; }
        set { if (playerHandler != null) playerHandler.enabled = value; }
    }
    public bool ActivateCardInputs
    {
        get { if (cardHandler != null) return cardHandler.isActiveAndEnabled; else return false; }
        set { if (cardHandler != null) cardHandler.enabled = value; }
    }
    public bool ActivateUIInputs
    {
        get { if (uiHandler != null) return uiHandler.isActiveAndEnabled; else return false; }
        set { if (uiHandler != null) uiHandler.enabled = value; }
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

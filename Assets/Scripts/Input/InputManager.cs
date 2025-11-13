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
        // register as a service in the GameContext so other systems can find us
        if (GameContext.Instance != null)
        {
            GameContext.Instance.RegisterService(this);
        }

        this.GetComponentInScene<PlayerInputHandler>(false, out playerHandler);
        this.GetComponentInScene<CardInputHandler>(false, out cardHandler);
        this.GetComponentInScene<UIInputHandler>(false, out uiHandler);

        // Register asks/acts to the GameContext so callers can use the token-based API
        var ctx = GameContext.Instance;
        if (ctx != null)
        {
            // Asks
            ctx.AddAsk(InputTokens.PlayerInputsActive, () => ActivatePlayerInputs);
            ctx.AddAsk(InputTokens.CardInputsActive, () => ActivateCardInputs);
            ctx.AddAsk(InputTokens.UIInputsActive, () => ActivateUIInputs);

            // Acts
            ctx.AddAct(InputTokens.SetPlayerInputs, (bool v) => ActivatePlayerInputs = v);
            ctx.AddAct(InputTokens.SetCardInputs, (bool v) => ActivateCardInputs = v);
            ctx.AddAct(InputTokens.SetUIInputs, (bool v) => ActivateUIInputs = v);
        }

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

    // Token container used by GameContext's Ask/Act system. Public so GameContext.InputAPI can reference it.
    public static class InputTokens
    {
        public static readonly Ask<bool> PlayerInputsActive = new Ask<bool>("Input.PlayerInputsActive");
        public static readonly Ask<bool> CardInputsActive = new Ask<bool>("Input.CardInputsActive");
        public static readonly Ask<bool> UIInputsActive = new Ask<bool>("Input.UIInputsActive");

        public static readonly Act<bool> SetPlayerInputs = new Act<bool>("Input.SetPlayerInputs");
        public static readonly Act<bool> SetCardInputs = new Act<bool>("Input.SetCardInputs");
        public static readonly Act<bool> SetUIInputs = new Act<bool>("Input.SetUIInputs");
    }

}

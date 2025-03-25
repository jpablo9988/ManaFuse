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
    private EventSystem _uiEventSystem;
    private PlayerInputHandler playerHandler;
    private CardInputHandler cardHandler;
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
        get { return _uiEventSystem.isActiveAndEnabled; }
        set
        {
            if (_uiEventSystem != null)
            {
                _uiEventSystem.enabled = value;
            }
        }
    }
    public PlayerInputHandler PlayerHandler => playerHandler;
    public CardInputHandler CardHandler => cardHandler;

    void Awake()
    {
        this.GetComponentInScene<EventSystem>(false, out _uiEventSystem);
        this.GetComponentInScene<PlayerInputHandler>(false, out playerHandler);
        this.GetComponentInScene<CardInputHandler>(false, out cardHandler);

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

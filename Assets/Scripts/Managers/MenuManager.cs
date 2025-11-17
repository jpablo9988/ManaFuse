using System;
using Unity.VisualScripting;
using UnityEngine;
using static MenuManager.MenuTokens;

    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private PauseHandler _pauseHandler;
        
        private void Awake()
        {
            if (GameContext.Instance == null)
            {
                GameContext.Instance.RegisterService(this);
            }
            _pauseHandler = GetComponent<PauseHandler>();
            
            var ctx = GameContext.Instance;
            
            //Init Asks
            
            //Init Acts
            ctx.AddAct(ChangePauseState, () => _pauseHandler.ChangePauseState());
            
        }

        public static class MenuTokens
        {
            //Define Asks
            
            
            //Define Args
            
            
            //Define Acts
            public static readonly Act<GameContext.Unit> ChangePauseState = new ("ChangePauseState");
        }
    }
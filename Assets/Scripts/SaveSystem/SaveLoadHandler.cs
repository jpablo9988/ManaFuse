using UnityEngine;
using TMPro;

namespace SaveSystem
{
    /// <summary>
    /// Handles manual save/load operations via keyboard shortcuts (F5/F9).
    /// Provides visual feedback to the player when save/load operations complete.
    /// Testing Clas.
    /// </summary>
    public class SaveLoadHandler : MonoBehaviour
    {
        [Header("Save Settings")]
        [Tooltip("The name of the save slot to use")]
        [SerializeField] private string saveSlotName = "playersave";

        [Header("UI Feedback (Optional)")]
        [Tooltip("Text component to display save/load messages")]
        [SerializeField] private TextMeshProUGUI feedbackText;

        [Tooltip("Duration in seconds to display feedback messages")]
        [SerializeField] private float feedbackDuration = 2f;

        private float _feedbackTimer = 0f;

        private void Update()
        {
            // Handle save input (F5)
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
            }

            // Handle load input (F9)
            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadGame();
            }

            // Update feedback timer
            if (_feedbackTimer > 0)
            {
                _feedbackTimer -= Time.deltaTime;
                if (_feedbackTimer <= 0 && feedbackText)
                {
                    feedbackText.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Saves the current game state to disk.
        /// </summary>
        public void SaveGame()
        {
            bool success = SaveGameManager.Instance.Save(saveSlotName);

            if (success)
            {
                Debug.Log($"Game saved successfully to {saveSlotName}!");
                ShowFeedback("Game Saved!", Color.green);
            }
            else
            {
                Debug.LogError($"Failed to save game to {saveSlotName}");
                ShowFeedback("Save Failed!", Color.red);
            }
        }

        /// <summary>
        /// Loads the saved game state from disk.
        /// </summary>
        public void LoadGame()
        {
            if (!SaveGameManager.Instance.HasSave(saveSlotName))
            {
                Debug.LogWarning($"No save file found for {saveSlotName}");
                ShowFeedback("No Save Found!", Color.yellow);
                return;
            }

            bool success = SaveGameManager.Instance.Load(saveSlotName);

            if (success)
            {
                Debug.Log($"Game loaded successfully from {saveSlotName}!");
                ShowFeedback("Game Loaded!", Color.green);
            }
            else
            {
                Debug.LogError($"Failed to load game from {saveSlotName}");
                ShowFeedback("Load Failed!", Color.red);
            }
        }

        /// <summary>
        /// Displays a feedback message to the player.
        /// </summary>
        private void ShowFeedback(string message, Color color)
        {
            if (feedbackText)
            {
                feedbackText.text = message;
                feedbackText.color = color;
                feedbackText.gameObject.SetActive(true);
                _feedbackTimer = feedbackDuration;
            }
        }
    }
}


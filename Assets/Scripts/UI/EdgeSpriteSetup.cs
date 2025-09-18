using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper script to set up an edge sprite for the ManaFuse bar.
/// This script provides a simple way to create and configure the edge sprite.
/// </summary>
public class EdgeSpriteSetup : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    [Tooltip("The ManaFuse bar component to attach the edge sprite to")]
    private ManafuseBar manafuseBar;
    
    [SerializeField]
    [Tooltip("The sprite to use for the edge indicator")]
    private Sprite edgeSprite;
    
    [SerializeField]
    [Tooltip("The color of the edge sprite")]
    private Color edgeSpriteColor = Color.white;
    
    [SerializeField]
    [Tooltip("The size of the edge sprite")]
    private Vector2 edgeSpriteSize = new Vector2(20, 30);
    
    [SerializeField]
    [Tooltip("Offset from the slider edge (in pixels)")]
    private float edgeSpriteOffset = 0f;
    
    [SerializeField]
    [Tooltip("Whether to automatically set up the edge sprite on Start")]
    private bool autoSetupOnStart = true;

    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupEdgeSprite();
        }
    }
    
    /// <summary>
    /// Sets up the edge sprite for the ManaFuse bar.
    /// Call this method to create and configure the edge sprite.
    /// </summary>
    [ContextMenu("Setup Edge Sprite")]
    public void SetupEdgeSprite()
    {
        if (manafuseBar == null)
        {
            Debug.LogError("ManaFuse bar reference is missing!");
            return;
        }
        
        if (edgeSprite == null)
        {
            Debug.LogError("Edge sprite is missing!");
            return;
        }
        
        // Create a new GameObject for the edge sprite
        GameObject edgeSpriteObject = new GameObject("EdgeSprite");
        edgeSpriteObject.transform.SetParent(manafuseBar.transform, false);
        
        // Add Image component
        Image edgeImage = edgeSpriteObject.AddComponent<Image>();
        edgeImage.sprite = edgeSprite;
        edgeImage.color = edgeSpriteColor;
        
        // Set up RectTransform
        RectTransform edgeRect = edgeSpriteObject.GetComponent<RectTransform>();
        edgeRect.sizeDelta = edgeSpriteSize;
        edgeRect.anchorMin = new Vector2(0, 0.5f);
        edgeRect.anchorMax = new Vector2(0, 0.5f);
        edgeRect.pivot = new Vector2(0.5f, 0.5f);
        edgeRect.anchoredPosition = Vector2.zero;
        
        // Set the edge sprite in the ManaFuse bar
        // Note: You'll need to manually assign this in the inspector since it's a private field
        // Or you can make the edgeSprite field public in ManafuseBar.cs
        
        Debug.Log("Edge sprite setup complete! Don't forget to assign the edgeSpriteObject to the ManaFuse bar's Edge Sprite field in the inspector.");
    }
    
    /// <summary>
    /// Updates the edge sprite properties if it's already set up.
    /// </summary>
    public void UpdateEdgeSpriteProperties()
    {
        if (manafuseBar == null) return;
        
        // Find the edge sprite in the ManaFuse bar's children
        Transform edgeSpriteTransform = manafuseBar.transform.Find("EdgeSprite");
        if (edgeSpriteTransform != null)
        {
            Image edgeImage = edgeSpriteTransform.GetComponent<Image>();
            if (edgeImage != null)
            {
                edgeImage.sprite = edgeSprite;
                edgeImage.color = edgeSpriteColor;
            }
            
            RectTransform edgeRect = edgeSpriteTransform.GetComponent<RectTransform>();
            if (edgeRect != null)
            {
                edgeRect.sizeDelta = edgeSpriteSize;
            }
        }
    }
}

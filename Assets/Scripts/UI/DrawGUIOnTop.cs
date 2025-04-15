using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///  Found on: https://discussions.unity.com/t/world-space-canvas-on-top-of-everything/128165/5
///  By User @Julien_Lynge and @Electric_Falcon
/// 
/// Overlays World UI in front of Scene Graphics.
/// </summary>
public class DrawGUIOnTop : MonoBehaviour
{

    private const string shaderTestMode = "unity_GUIZTestMode"; //The magic property we need to set
    [SerializeField] UnityEngine.Rendering.CompareFunction desiredUIComparison = UnityEngine.Rendering.CompareFunction.Always; //If you want to try out other effects
    [Tooltip("Set to blank to automatically populate from the child UI elements")]
    [SerializeField] Graphic[] uiGraphicsToApplyTo;
    [Tooltip("Set to blank to automatically populate from the child UI elements")]
    [SerializeField] TextMeshProUGUI[] uiTextsToApplyTo;
    //Allows us to reuse materials
    private Dictionary<Material, Material> materialMappings = new Dictionary<Material, Material>();
    protected virtual void Start()
    {
        if (uiGraphicsToApplyTo.Length == 0)
        {
            uiGraphicsToApplyTo = gameObject.GetComponentsInChildren<Graphic>();
        }
        if (uiTextsToApplyTo.Length == 0)
        {
            uiTextsToApplyTo = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        }
        foreach (var graphic in uiGraphicsToApplyTo)
        {
            Material material = graphic.materialForRendering;
            if (material == null)
            {
                Debug.LogError($"{nameof(DrawGUIOnTop)}: skipping target without material {graphic.name}.{graphic.GetType().Name}");
                continue;
            }
            if (!materialMappings.TryGetValue(material, out Material materialCopy))
            {
                materialCopy = new Material(material);
                materialMappings.Add(material, materialCopy);
            }
            materialCopy.SetInt(shaderTestMode, (int)desiredUIComparison);
            graphic.material = materialCopy;
        }
        foreach (var text in uiTextsToApplyTo)
        {
            Material material = text.fontMaterial;
            if (material == null)
            {
                Debug.LogError($"{nameof(DrawGUIOnTop)}: skipping target without material {text.name}.{text.GetType().Name}");
                continue;
            }
            if (!materialMappings.TryGetValue(material, out Material materialCopy))
            {
                materialCopy = new Material(material);
                materialMappings.Add(material, materialCopy);
            }
            materialCopy.SetInt(shaderTestMode, (int)desiredUIComparison);
            text.fontMaterial = materialCopy;
        }
    }
}

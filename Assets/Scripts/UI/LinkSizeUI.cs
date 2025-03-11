using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LinkSizeUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform target;

    private RectTransform localRT;
    void Awake()
    {
        localRT = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        localRT.sizeDelta = new Vector2(target.rect.width, target.rect.height);
    }
}

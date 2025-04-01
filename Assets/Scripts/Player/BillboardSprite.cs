using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private BillboardType _billboardType = BillboardType.MatchFowardWithCamera;
    void LateUpdate()
    {
        switch (_billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case BillboardType.MatchFowardWithCamera:
                transform.forward = Camera.main.transform.forward;
                break;
        }
    }
}

using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private BillboardType _billboardType = BillboardType.MatchFowardWithCamera;
    [SerializeField] private bool _reverseFoward = false;
    void LateUpdate()
    {
        switch (_billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case BillboardType.MatchFowardWithCamera:
                if (_reverseFoward) transform.forward = -Camera.main.transform.forward;
                else transform.forward = Camera.main.transform.forward;
                break;
        }
    }
}

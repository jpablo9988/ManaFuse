using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;
    //[SerializeField] private float damage = 10f;
    private Vector3 initialDirection;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(-speed * Time.deltaTime * Vector3.left);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
    }
}
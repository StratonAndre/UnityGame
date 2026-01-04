using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbodyy;

    public void SetVelocity(Vector3 velocity)
    {
        rigidbodyy.AddForce(velocity, ForceMode.VelocityChange);
    }
}

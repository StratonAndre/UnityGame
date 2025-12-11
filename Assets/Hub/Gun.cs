using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float range;
    [SerializeField] private float speed;

    [Header("Input")]
    [SerializeField] private InputActionReference fireInputAction;
    private bool fireInput;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void Update()
    {
        fireInput = fireInputAction.action.WasPressedThisFrame();

        if (fireInput) { Fire(); }
    }

    void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, range, collisionLayer))
        {
            Debug.Log("Hit object: " + hit.transform.gameObject.name);
            firePoint.LookAt(hit.point);
        }
        else
        {
            firePoint.localEulerAngles = Vector3.zero;
        }

        GameObject shootProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        shootProjectile.GetComponent<Projectile>().SetVelocity(firePoint.forward.normalized * speed);
        Destroy(shootProjectile, range / speed);
    }
}

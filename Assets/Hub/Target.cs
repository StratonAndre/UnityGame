using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform displayScreen;
    [SerializeField] private GameObject hitPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit target.");

        GameObject targetHit = Instantiate(hitPrefab, collision.transform.position, transform.rotation);
        Destroy(targetHit, 30f);

        Vector3 displayPosition = displayScreen.position +
            new Vector3((collision.transform.position.x - transform.position.x) * displayScreen.localScale.x * 2f,
                        (collision.transform.position.y - transform.position.y) * displayScreen.localScale.y * 2f,
                        0f);

        GameObject displayHit = Instantiate(hitPrefab, displayPosition, displayScreen.rotation);

        Vector3 displayScale = Vector3.Scale(displayHit.transform.localScale, displayScreen.localScale);
        displayHit.transform.localScale = displayScale;

        displayHit.transform.RotateAround(displayScreen.position, Vector3.right, displayScreen.eulerAngles.x);
        displayHit.transform.RotateAround(displayScreen.position, Vector3.up, displayScreen.eulerAngles.y);
        displayHit.transform.RotateAround(displayScreen.position, Vector3.forward, displayScreen.eulerAngles.z );

        Destroy(displayHit, 30f);
    }
}

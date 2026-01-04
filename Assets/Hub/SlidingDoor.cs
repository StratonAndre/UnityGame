using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Vector3 leftDoorClosedPosition;
    [SerializeField] private Vector3 leftDoorOpenPosition;

    [SerializeField] private Transform rightDoor;
    [SerializeField] private Vector3 rightDoorClosedPosition;
    [SerializeField] private Vector3 rightDoorOpenPosition;

    [SerializeField] private bool isLocked;
    private bool isOpen;

    [SerializeField] private float openSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        if (isOpen == true && isLocked == false)
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftDoorOpenPosition, openSpeed * Time.deltaTime);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightDoorOpenPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftDoorClosedPosition, openSpeed * Time.deltaTime);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightDoorClosedPosition, openSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isOpen = false;
        }
    }
}

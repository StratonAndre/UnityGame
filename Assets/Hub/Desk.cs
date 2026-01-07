using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Desk : MonoBehaviour
{
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private InputActionReference interactInputAction;
    private bool canInteract;

    private void Update()
    {
        if (canInteract)
        {
            if (interactInputAction.action.WasPressedThisFrame())
            {
                Debug.Log("Start mission.");
                SceneManager.LoadScene("Cargo");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = true;
            interactText.text = "Press [E] to start mission.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = false;
            interactText.text = "";
        }
    }
}

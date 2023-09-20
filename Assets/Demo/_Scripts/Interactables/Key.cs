using UnityEngine;
using TMPro;

public class Key : InteractableBase
{
    // [SerializeField]
    // // private GameObject keyPrefab;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public override void OnInteract()
    {
        gameManager.levelUnlockCounter++;
        gameManager.txtInteractMessage.text = "Collected Key!";     // TODO: make disappear after a delay.
        gameManager.txtMissionUpdate.text = "Use key to open Door.";
        gameManager.crossHairText.text = "";
        gameObject.SetActive(false);
    }

    private void GiveKey()
    {
        // lmao
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Player variables")]
    public Transform interactorSource;
    public float interactRange;

    private void Update() 
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            var hit = hitInfo.collider.gameObject.TryGetComponent(out InteractableBase interactObj);
            if (hit)
            {
                GameManager.Instance.crossHairText.text = interactObj.interactableObjName;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.OnInteract();
                }
            }
        } 
        else 
        {
            GameManager.Instance.crossHairText.text = "";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KaChow.AByteSizedMuseum
{
    public class InterpreterLine : MonoBehaviour, IDropHandler
    {
        private Image highlight;
        private Color defaultColor;

        private void Start()
        {
            highlight = GetComponent<Image>();
            defaultColor = highlight.color;
        }

        public void EnableHighlight()
        {
            highlight.color = Color.white;
        }

        public void DisableHighlight()
        {
            highlight.color = defaultColor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            // GameObject dropped = eventData.pointerDrag;
            // CodeBlock heldCodeBlock = dropped.GetComponent<CodeBlock>();
            // GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
            // Debug.Log($"attempted drop target: {dropTarget.name}");

            // if (dropTarget.TryGetComponent<InterpreterLine>(out var targetInterpreterLine))
            // {
            //     Debug.Log("here");
            //     if (targetInterpreterLine.transform.childCount >= 1)
            //     {
            //         // Debug.Log($"{heldCodeBlock.parentBeforeDrag.name}");

            //         if (heldCodeBlock.parentBeforeDrag.GetComponent<CodeBlockSlot>() != null) return;

            //         CodeBlock existingCodeBlock = targetInterpreterLine.GetComponentInChildren<CodeBlock>();

            //         // Swap the parent of the held and existing code blocks
            //         existingCodeBlock.transform.SetParent(heldCodeBlock.parentAfterDrag);
            //         heldCodeBlock.transform.SetParent(targetInterpreterLine.transform);
            //     }

            //     // heldCodeBlock.parentBeforeDrag = transform;
            //     heldCodeBlock.parentAfterDrag = dropTarget.transform;
            //     Debug.Log($"heldCodeBlock.parentAfterDrag : {heldCodeBlock.parentAfterDrag}");
            // }
            // else
            // {
            //     Debug.Log($"attempted drop target: {dropTarget.name}");
            //     heldCodeBlock.parentAfterDrag = null;

            // }

            // // if (heldCodeBlock.parentBeforeDrag.GetComponent<InterpreterLine>() != null)
            // // {
            // //     Debug.LogWarning("parentbefore drag = interpreterline");
            // // }

        }
    }
}

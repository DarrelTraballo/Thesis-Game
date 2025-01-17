using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KaChow.AByteSizedMuseum
{
    #region comments
    /* TODO: Interpreter UI: may line numbers sa left side, similar sa mga code editors (like vscode)
<- these things

       TODO: make for loop block expandable or something
             like if you try to put another code block inside the for loop, it expands (take up the bottom 1-2 InterpreterLines)
             OR


       TODO: also start working on the easy code blocks or something
             both UI and functionality
                - variable block
                    > fields for variable name and value
                    > when interpreter reads, just Debug.Log() name and value for now

                - Pickup Block
                    > picks up object directly in front of it
                        ```
                        if bot is not holding an object:
                            use raycast to check if object in front of it is pickup-able
                            if raycast.hit == pickup-able object:
                                pickup object
                            else:
                                do nothing
                        else:
                            do nothing
                        ```

                - Drop Block
                    > drops held object directly in front of it
                        ```
                        if bot is holding an object:
                            raycast to check if object in front is occupied
                            if raycast.hit == true:
                                do nothing
                            else:
                                drop object
                        else:
                            do nothing
                        ```

                - Function block
                    > Lets players define a series of commands as a function.
                    > should have another UI for telling what's inside the function block
                    > when clicked, a part of inventory side gets covered by the function block UI
                    > functionality should be very similar to how interpreter reads lines.

                - For Loop Block
                    > very similar functionality to Interpreter.ExecuteAllLines();
    */
    #region MoveBlock Done
    /*
                [Require(objects for it to control)]
                - move block
                    > no parameters
                    > simply moves the puzzle object in the room a set amount of units
                        > should be only tied to that object in the current room, it should not alter objects outside the current room

                - rotate block
                    > one parameter (clockwise / counterclockwise)
                        > drop-down maybe?
                    > simply rotates the puzzle object clock/counterclockwise

                - Lightbot puzzle
                    > establish what type of object is being controlled
                        > figure out how to make interpreter and controlled object communicate with each other

    */
    #endregion

    /* TODO: pickup-able items
                > allow players to pick up certain items and store them in their inventory
                >
       TODO:
             reset button for interpreter
    */
    #endregion

    public class Interpreter : MonoBehaviour
    {
        public static Interpreter Instance { get; private set; }
        private Interpreter() { }

        private GameManager gameManager;
        private InputManager inputManager;

        [Header("Game Events")]
        public GameEvent onInterpreterClose;
        public GameEvent onResetPuzzle;

        [Header("Left Panel")]
        public List<InterpreterLine> interpreterLines;
        public float executeTime = 0.20f;

        [Header("Right Panel")]
        [SerializeField] private GameObject codeBlockDetailsPanel;
        [SerializeField] private GameObject puzzleCameraFeed;
        [SerializeField] private GameObject deleteBlockPanel;

        [SerializeField] private GameObject closeIcon;

        [Header("Buttons")]
        [SerializeField] private Button[] interpreterButtons;

        [SerializeField] private int interpreterID;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            inputManager = InputManager.Instance;
        }

        public void ExecuteLines()
        {
            inputManager.enabled = false;
            codeBlockDetailsPanel.SetActive(false);
            puzzleCameraFeed.SetActive(true);
            StartCoroutine(ExecuteAllLines());
        }

        private IEnumerator ExecuteAllLines()
        {
            ToggleButtons(false);
            foreach (var interpreterLine in interpreterLines)
            {
                interpreterLine.EnableHighlight();

                CodeBlock codeBlock = interpreterLine.GetComponentInChildren<CodeBlock>();
                if (codeBlock == null)
                {
                    yield return new WaitForSeconds(executeTime);
                    interpreterLine.DisableHighlight();
                    continue;
                }

                yield return StartCoroutine(codeBlock.ExecuteBlock(interpreterID));

                interpreterLine.DisableHighlight();
            }
            yield return new WaitForSeconds(0.2f);

            ToggleButtons(true);

            inputManager.enabled = true;
        }

        public void ClearInterpreter()
        {
            foreach (var interpreterLine in interpreterLines)
            {
                foreach (Transform child in interpreterLine.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void ClearInterpreterCoroutine()
        {
            inputManager.enabled = false;
            StartCoroutine(ClearInterpreterLines());
        }

        public IEnumerator ClearInterpreterLines()
        {
            ToggleButtons(false);

            foreach (var interpreterLine in interpreterLines)
            {
                interpreterLine.EnableHighlight();

                foreach (Transform child in interpreterLine.transform)
                {
                    Destroy(child.gameObject);
                }

                yield return new WaitForSeconds(executeTime);

                interpreterLine.DisableHighlight();
            }
            yield return new WaitForSeconds(0.2f);

            ToggleButtons(true);

            inputManager.enabled = true;
        }

        public void CloseInterpreter()
        {
            StopExecuting();
            gameManager.SetGameState(GameState.Playing);
            onInterpreterClose.Raise(this, name);

            foreach (var interpreterLine in interpreterLines)
            {
                interpreterLine.DisableHighlight();

                foreach (Transform child in interpreterLine.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            ToggleButtons(true);
            inputManager.enabled = true;
        }

        public void ToggleButtons(bool isInteractable)
        {
            foreach (var button in interpreterButtons)
            {
                button.interactable = isInteractable;
            }
        }

        public void ShowCodeBlockDetails(Component sender, object data)
        {
            if (data is not Tuple<bool, Vector3> tupleData) return;

            bool isActive = tupleData.Item1;
            Vector3 mousePos = tupleData.Item2;

            CodeBlock codeBlock = sender.gameObject.GetComponent<CodeBlock>();
            codeBlockDetailsPanel.SetActive(isActive);

            Transform codeBlockDetailsPanelTransform = codeBlockDetailsPanel.transform;

            codeBlockDetailsPanelTransform.position = mousePos;

            TextMeshProUGUI codeBlockName = codeBlockDetailsPanelTransform.Find("Code Block Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI codeBlockDescription = codeBlockDetailsPanelTransform.Find("Code Block Description").GetComponent<TextMeshProUGUI>();

            codeBlockName.text = codeBlock.codeBlockName;
            codeBlockDescription.text = codeBlock.codeBlockDescription;

            Color codeBlockColor = codeBlock.image.color;
            codeBlockDetailsPanel.GetComponent<Image>().color = codeBlockColor;

        }

        public void ToggleDeletePanel(Component sender, object data)
        {
            if (data is not bool isActive) return;

            deleteBlockPanel.SetActive(isActive);
        }

        public void ResetPuzzle()
        {
            onResetPuzzle.Raise(this, interpreterID);
        }

        public void StopExecuting()
        {
            StopCoroutine(ExecuteAllLines());
            StopCoroutine(ClearInterpreterLines());
        }

        public void SetInterpreterID(int interpreterID)
        {
            this.interpreterID = interpreterID;
        }
    }
}

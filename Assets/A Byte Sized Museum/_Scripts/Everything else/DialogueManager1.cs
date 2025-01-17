using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KaChow.AByteSizedMuseum
{
    public class Dialogmanager : MonoBehaviour
    {

        [SerializeField] public bool helperbot_tutorial = false;
        [SerializeField] public bool unlock = false;
        public TMP_Text nameText;
        public TMP_Text dialogueText;
        public GameObject Input_manager;
        public GameObject DialogueContainer;
        public GameObject DialogueContainer2;
        public GameObject Canvas_images;
        public Button button;
        private int count = 0;
        public int counter;
        public Animator animator;
        public Queue<string> sentences = new Queue<string>();
        public int sentences_count;
        public GameObject[] images;


        private GameManager gameManager;

        int index;


        // Start is called before the first frame update
        public void Start()
        {
            gameManager = GameManager.Instance;
            DialogueContainer.SetActive(true);
            //Invoke("DelayedCode",1f);
            sentences_count = sentences.Count + 1;

            if (helperbot_tutorial == true)
            {
                index = 0;
            }
            if (unlock == true)
            {
                index = 0;
            }

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {

                if (sentences.Count != 0)
                {
                    if (button != null)
                    {
                        button.onClick.Invoke();
                    }
                }
                else
                {
                    EndDialogue();
                }
                // Invoke the button click

            }

        }

        void DelayedCode()
        {
            DialogueContainer.SetActive(true);
        }

        public void Next()
        {
            index += 1;
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
                images[index].gameObject.SetActive(true);
            }
        }

        public void StartDialogue(Dialogue dialogue)
        {
            animator.SetBool("IsOpen", false);
            animator.SetBool("IsOpen", true);
            gameManager.SetGameState(GameState.RunDialog);

            nameText.text = dialogue.name;

            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
                print(sentences.Count);
            }
            if (helperbot_tutorial == true)
            {
                images[0].gameObject.SetActive(true);
            }

            DisplayNextSentence();

        }

        public void DisplayNextSentence()
        {
            AudioManager.Instance.sfxSource.Stop();
            AudioManager.Instance.PlaySFX("HelperBot");

            if (count == counter)
            {
                Input_manager.SetActive(true);
            }

            if (helperbot_tutorial == true)
            {
                Next();
            }

            count++;

            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(String sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }

        }
        void EndDialogue()
        {
            AudioManager.Instance.sfxSource.Stop();
            ResetData();
            Canvas_images.SetActive(false);
            animator.SetBool("IsOpen", false);
            //DialogueContainer.SetActive(false);

            gameManager.SetGameState(GameState.Playing);


        }
        public void ResetData()
        {

            sentences.Clear();
            ResetUIElements();
            DialogueContainer.SetActive(false);
            DialogueContainer2.SetActive(false);
        }

        // Method to reset UI elements
        private void ResetUIElements()
        {
            // Reset UI elements to their initial state
            index = 0;
            nameText.text = "";
            dialogueText.text = "";
            //Input_manager.SetActive(true);
        }
    }
}

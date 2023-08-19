using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Yarn.Unity;

public class DialogueBubble : DialogueViewBase
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private RectTransform background; 
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Transform bubblePosition; 
    [SerializeField] private Transform bubblePositionNPC;

    private SpriteRenderer backgroundSpriteRenderer;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler<String>("isTalking", IsTalking);
        backgroundSpriteRenderer = background.gameObject.GetComponent<SpriteRenderer>();
    }

        private void Start()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        //advancing dialogue
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserRequestedViewAdvancement(); //can be called in other scipts
        }
    }

    //running dialogue
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        background.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        text.text = dialogueLine.Text.Text; //updating text.
        //advanceDialogue = requestInterrupt; //interrupting dialogue if theres a signal to advance

        text.ForceMeshUpdate(); //ensuring text is rendered instantly to avoid errors when getting tetx size

        //scaling background relative to amount of text 
        int lineCount = text.textInfo.lineCount;
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 padding = new Vector2(6f, 2f);
        backgroundSpriteRenderer.size = new Vector2(textSize.x, lineCount * 5) + padding;  
    }

    //advancing dialogue 
    public override void UserRequestedViewAdvancement()
    {
        if (background.gameObject.activeSelf && text.gameObject.activeSelf)
        {
            requestInterrupt.Invoke(); //signal to interrupt dialogue (advancing)
        }
    }

    //stopping dialogue
    public override void DismissLine(Action onDismissalComplete) //might be source of folliwng error: Can't start dialogue from node Start: the dialogue is currently in the middle of running. Stop the dialogue first.
        //might be affecting dialogue from restarting when exit trigger/talk to different charachter
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        onDismissalComplete();
    }

    //positioning speech baloon based on who's talking
    public void IsTalking(string characterName)
    {
        Transform bubblePosition = GameObject.Find(characterName).transform.Find("BubblePosition"); 

        if (characterName == "Gesi")
        {
            text.transform.position = bubblePosition.position;
            background.position = bubblePosition.position;
            backgroundSpriteRenderer.color = Color.yellow;
        }
        else
        {
            text.transform.position = bubblePosition.transform.position;
            background.position = bubblePosition.transform.position;
            backgroundSpriteRenderer.color = Color.white;
        }
        
    }


}

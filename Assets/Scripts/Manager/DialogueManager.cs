using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Button nextButton;
    public Button helpButton;
    public Button laterButton;

    private string[] currentDialogues;
    private int currentLine = 0;
    private bool isChoicePhase = false;
    private string targetMiniGameScene;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        helpButton.gameObject.SetActive(false);
        laterButton.gameObject.SetActive(false);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(NextDialogue);
        helpButton.onClick.AddListener(OnHelpSelected);
        laterButton.onClick.AddListener(OnLaterSelected);
    }

    public void ShowDialogue(string[] dialogues, string miniGameScene = "")
    {
        currentDialogues = dialogues;
        currentLine = 0;
        targetMiniGameScene = miniGameScene;

        dialoguePanel.SetActive(true);
        dialogueText.text = currentDialogues[currentLine];
    }

    public void NextDialogue()
    {
        if (isChoicePhase) return;

        currentLine++;
        if (currentLine < currentDialogues.Length)
        {
            if (currentDialogues[currentLine].Contains("You wanna help him?"))
            {
                ShowChoiceButton();
                return;
            }
            dialogueText.text = currentDialogues[currentLine];
        }
        else
            dialoguePanel.SetActive(false);
    }

    private void ShowChoiceButton()
    {
        isChoicePhase = true;
        nextButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(true);
        laterButton.gameObject.SetActive(true);
    }

    private void OnHelpSelected()
    {
        dialogueText.text = " Thanks.";
        helpButton.gameObject.SetActive(false);
        laterButton.gameObject.SetActive(false);

        nextButton.onClick.RemoveAllListeners();

        Invoke("LoadMiniGame", 2f);
    }

    private void OnLaterSelected()
    {
        dialogueText.text = "OK.. I see..";
        helpButton.gameObject.SetActive(false);
        laterButton.gameObject.SetActive(false);

        Invoke("HideDialogue", 2f);
    }

    private void LoadMiniGame()
    {
        if (!string.IsNullOrEmpty(targetMiniGameScene))
        {
            SceneManager.LoadScene(targetMiniGameScene);
        }
        else
        {
            Debug.LogError("미니게임 씬 이름이 지정되지 않았습니다!");
            dialoguePanel.SetActive(false);
        }
    }

    private void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);
        isChoicePhase = false;
    }
}

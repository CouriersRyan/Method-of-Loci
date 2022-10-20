using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InkManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJson;
    private Story _story;

    [SerializeField] private TMP_Text textField;
    [SerializeField] private GameObject dialoguePanel;

    private void StartStory(string storyText)
    {
        _story = new Story(storyText);
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
        StartStory(inkJson.text);
    }

    public void DisplayNextLine()
    {
        if (_story.canContinue)
        {
            dialoguePanel.SetActive(true);
            GameManager.Instance.TogglePlayers(Player.Primary);
            string text = _story.Continue();
            text = text?.Trim();
            textField.text = ParseForCustomTag(text)[0];
        }
        else if(_story.currentChoices.Count > 0)
        {
            GameManager.Instance.TogglePlayers(Player.Secondary);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void MakeChoice(string choiceText)
    {
        foreach (var choice in _story.currentChoices)
        {
            var choiceTagged = ParseForCustomTag(choice.text);
            if (choiceTagged.Length > 1)
            {
                if (choiceText.Equals(choiceTagged[1]))
                {
                    _story.ChooseChoiceIndex(choice.index);
                    DisplayNextLine();
                    return;
                }
            }
        }
    }

    //Splits tags from the body text using $
    private string[] ParseForCustomTag(string line)
    {
        
        var split = line.Split("$");
        for (int i = 0; i < split.Length; i++)
        {
            split[i] = split[i].Trim();
        }

        return split;
    }
}

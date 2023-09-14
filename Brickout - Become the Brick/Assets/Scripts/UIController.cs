using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject newGamePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject kidPanel;


    public List<TMP_Text> UITexts;
    public Dictionary<string, string[]>UITextsDict = new Dictionary<string, string[]>();

    private void Start()
    {
        InitializeDictionary();
    }

    void InitializeDictionary()
    {   
        for(int i = 0; i < UITexts.Count; i++)
        {
            //adding the dictionary entry, adding the value string[], [0]: original string, [1]: output value (newest)
            UITextsDict.Add(UITexts[i].name, new string[] { UITexts[i].text, "newText"});
        }
    }

    public void UpdateUIText(string TMPObjectName, string text)
    {
        
        for (int i = 0; i < UITexts.Count; i++)
        {
            if (UITexts[i].name == TMPObjectName)
            {
                //changing the the value associated with the key tmpObject.name;     
                UITextsDict[TMPObjectName][1] = text;    // value

                //formatting the text
                //read from the original string in the dictionary and output the newerone.
                UITexts[i].text = string.Format(UITextsDict[TMPObjectName][0], UITextsDict[TMPObjectName][1]);
            }
        }
    }

    public void ShowUIPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void HideUIPanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}

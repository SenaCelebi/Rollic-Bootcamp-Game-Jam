using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    private string textToWrite = "Create My Perfect Match!";
    private int characterIndex = 0;
    private float timePerCharacter = 2f;
    private float timer = 0f;

    public void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerChar)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerChar;
        characterIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (uiText != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer += timePerCharacter;
                characterIndex++;
                uiText.text = textToWrite.Substring(0, characterIndex);

                if (characterIndex >= textToWrite.Length)
                {
                    uiText = null;
                    return;
                }
            }
        }


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    [SerializeField] GameObject textEditWindow;
    
    [SerializeField] InputField textField;
    [SerializeField] Text inputText;
    [SerializeField] Text outputText;

    [SerializeField] Color whiteColor;
    [SerializeField] Color yellowColor;
    [SerializeField] Color redColor;
    [SerializeField] Color blueColor;
    [SerializeField] Color greenColor;
    private void Start()
    {
        textEditWindow.SetActive(false);
        SetWhiteColor();
    }

    public void ToggleTextWindow(bool open)
    {
        textEditWindow.SetActive(open);
        if (open)
            textField.text = outputText.text;
    }

    public void ConfirmText()
    {
        outputText.text = textField.text.ToUpper();
        textEditWindow.SetActive(false);
    }

    public void SetWhiteColor()
    {
        inputText.color = outputText.color = whiteColor;
    }

    public void SetYellowColor()
    {
        inputText.color = outputText.color = yellowColor;
    }

    public void SetRedColor()
    {
        inputText.color = outputText.color = redColor;
    }

    public void SetBlueColor()
    {
        inputText.color = outputText.color = blueColor;
    }

    public void SetGreenColor()
    {
        inputText.color = outputText.color = greenColor;
    }
}

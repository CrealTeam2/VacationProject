using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    public Text uiText;
    public string fullText;
    public float delay = 0.1f;
    private string currentText = "";

    void Start()
    {
        StartCoroutine(ShowText());
    }

    public IEnumerator ShowText()
    {

        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            uiText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }


}
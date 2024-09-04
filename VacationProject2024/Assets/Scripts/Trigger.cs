using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    public TextAnimation textAnimation;
    public Text uiText;
    public GameObject panel;
    public string message = "";
    public float displayDuration = 2.0f;
    private bool hasTriggered = false;

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (panel != null)
            {
                panel.SetActive(true);
            }
            uiText.gameObject.SetActive(true);
            textAnimation.fullText = message;
            StartCoroutine(ShowTextAndHide());
        }
    }

    IEnumerator ShowTextAndHide()
    {
        yield return StartCoroutine(textAnimation.ShowText());
        yield return new WaitForSeconds(displayDuration);
        uiText.gameObject.SetActive(false);
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}

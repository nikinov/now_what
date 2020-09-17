using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplyButton : MonoBehaviour
{
    public delegate void ClickAction(string testID);
    public event ClickAction OnClicked;
    public delegate void OrderOut(GameObject go);
    public event OrderOut MakeOrderOut;
    private bool wasClicked;
    public bool WasClicked { get { return wasClicked; } }
    public bool Deprecated;
    public float waitingTime;

    private void Start()
    {
        wasClicked = false;
        Deprecated = false;
    }

    public void Reply()
    {
        if (!wasClicked)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            wasClicked = true;
            if (!Deprecated)
            {
                if (OnClicked != null)
                    OnClicked(gameObject.GetComponent<TextMeshProUGUI>().text);
                StartCoroutine(waitForFadeOut(waitingTime));
                Deprecated = true;
            }
            else
            {
                if (OnClicked != null)
                    OnClicked(gameObject.GetComponent<TextMeshProUGUI>().text);
            }
        }
    }

    public void MakeOutOfOrder(string NameID)
    {
        if (NameID == gameObject.GetComponent<TextMeshProUGUI>().text)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            StartCoroutine(waitForFadeOutOfOrder(.5f));
            wasClicked = true;
            Deprecated = true;
        }
    }
    IEnumerator waitForFadeOut(float waiteTime)
    {
        LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0, waiteTime);
        yield return new WaitForSeconds(waiteTime);
        Destroy(gameObject);
    }
    IEnumerator waitForFadeOutOfOrder(float waiteTime)
    {
        LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0, waiteTime);
        Debug.Log("making green and fade");
        if (MakeOrderOut != null)
            MakeOrderOut(gameObject);
        yield return new WaitForSeconds(waiteTime);
        Destroy(gameObject);
    }
}

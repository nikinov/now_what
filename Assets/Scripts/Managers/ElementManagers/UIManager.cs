using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public delegate void Click(string textID);
    public event Click Clicked;
    public delegate void MakeOutOfOrder(string textID);
    public event MakeOutOfOrder makeOutOfOrder;
    [SerializeField] private GameObject ReplyButtonUI;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject PlaceholdersPanel;
    [SerializeField] private GameObject BlackPanel;
    private List<Transform> Plceholders;
    private List<int> availablePlaceholders;

    // Start is called before the first frame update
    private void Awake()
    {
        Plceholders = new List<Transform>();
        availablePlaceholders = new List<int>();
        foreach (Transform holder in PlaceholdersPanel.transform)
        {
            Plceholders.Add(holder);
            holder.gameObject.name = "PlaceHolder";
            availablePlaceholders.Add(Plceholders.IndexOf(holder));
        }
    }

    public void SpawnUI(string Content, float itWillLast)
    {
        StartCoroutine(MakeReplyTextUI(Content, itWillLast));
    }

    private void buttonClicked(string textID)
    {
        Clicked(textID);
    }

    public void fadeAll(bool IN,float TIME)
    {
        if (IN)
        {
            LeanTween.alphaCanvas(BlackPanel.GetComponent<CanvasGroup>(), 0, TIME);
            StartCoroutine(waitForBlackPanel(false, TIME));
        }
        else
        {
            BlackPanel.SetActive(true);
            LeanTween.alphaCanvas(BlackPanel.GetComponent<CanvasGroup>(), 1, TIME);
        }
    }

    public void PutButtonOutOfOrder(string textID)
    {
        makeOutOfOrder(textID);
    }
    private void FadeOut(GameObject go, float fadeSpeed)
    {
        LeanTween.alphaCanvas(go.GetComponent<CanvasGroup>(), 0, fadeSpeed);
        StartCoroutine(waitForFadeOut(go, fadeSpeed));
    }

    public void UnsubscribeToMakingOutOfOrderOnTheReplayButtonSide(GameObject go)
    {
        makeOutOfOrder -= go.GetComponent<ReplyButton>().MakeOutOfOrder;
    }

    IEnumerator MakeReplyTextUI(string content,float ItWillLast, float fadeSpeed=2)
    {
        int randomNum = Random.Range(0, availablePlaceholders.Count);
        int randomNumReal = availablePlaceholders[randomNum];
        Plceholders[randomNum].gameObject.name = "PlaceHolder occupied";
        GameObject go = Instantiate(ReplyButtonUI, Plceholders[randomNumReal].position, Quaternion.identity, MainPanel.transform);
        availablePlaceholders.Remove(randomNumReal);
        ReplyButton replyButton = go.GetComponent<ReplyButton>();
        replyButton.waitingTime = fadeSpeed;
        go.GetComponent<TextMeshProUGUI>().text = content;
        replyButton.OnClicked += buttonClicked;
        replyButton.MakeOrderOut += UnsubscribeToMakingOutOfOrderOnTheReplayButtonSide;
        makeOutOfOrder += replyButton.MakeOutOfOrder;
        LeanTween.alphaCanvas(go.GetComponent<CanvasGroup>(), 1, fadeSpeed);
        yield return new WaitForSeconds(ItWillLast);
        if (go != null)
        {
            if (!replyButton.WasClicked)
            {
                replyButton.Deprecated = true;
                FadeOut(go, fadeSpeed);
            }
        }
        yield return new WaitForSeconds(fadeSpeed);
        Plceholders[randomNum].gameObject.name = "PlaceHolder";
        availablePlaceholders.Add(randomNumReal);
    }

    IEnumerator waitForFadeOut(GameObject go,float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        Destroy(go);
        UnsubscribeToMakingOutOfOrderOnTheReplayButtonSide(go);
    }

    IEnumerator waitForBlackPanel(bool turn,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        BlackPanel.SetActive(turn);
    }
}

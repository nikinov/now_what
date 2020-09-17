using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioManager audioManager;
    public UIManager uiManager;
    private Manager manager;
    void Start()
    {
        manager = gameObject.GetComponent<Manager>();
        manager.OnOpen += OnOpen;
        manager.OnClose += OnClose;
        uiManager.Clicked += OnClick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnOpen()
    {
        manager.ClosePossible = false;
        uiManager.fadeAll(true, .5f);
        StartCoroutine(MainMenuSequence());
        StartCoroutine(makeUIInTime("start", 15, 1000));
    }

    void OnClose()
    {
        uiManager.fadeAll(false, 1);
        StartCoroutine(waitForClose(1f));
    }

    void OnClick(string textID)
    {
        switch (textID)
        {
            case "start":
                manager.OpenState("MyRoom_01");
                break;
            
            case "volume's great":
                audioManager.Stop("MainMenu001");
                audioManager.Play("MainMenu002");
                uiManager.PutButtonOutOfOrder("no give me a sec");
                StartCoroutine(makeUIInTime("done now what", 2, 1000));
                break;
            
            case "no give me a sec":
                audioManager.Stop("MainMenu001");
                audioManager.Play("MainMenu003");
                uiManager.PutButtonOutOfOrder("volume's great");
                StartCoroutine(makeUIInTime("done now what", 7, 1000));
                break;
                
            
            case "done now what":
                StartCoroutine(makeUIInTime("start", 2, 1000));
                audioManager.Play("MainMenu004");
                
                break;
            
            default:
                Debug.Log("incorrect tect ID in MainMenu.cs in function OnClick");
                break;
        }
    }

    IEnumerator waitForClose(float time)
    {
        yield return new WaitForSeconds(time);
        manager.ClosePossible = true;
    }

    IEnumerator MainMenuSequence()
    {
        audioManager.Play("MainMenu001");
        audioManager.Play("MainMenuBackgroundMusic");
        uiManager.SpawnUI("start", 4);
        yield return new WaitForSeconds(4);
        uiManager.SpawnUI("volume's great",2);
        yield return new WaitForSeconds(1);
        uiManager.SpawnUI("no give me a sec",2);
    }

    IEnumerator makeUIInTime(string buttonContent,float InWhatTimeAmount, float HowLongWillItLast)
    {
        yield return new WaitForSeconds(InWhatTimeAmount);
        uiManager.SpawnUI(buttonContent, HowLongWillItLast);
    }
}






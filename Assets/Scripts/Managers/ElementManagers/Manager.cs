using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public delegate void Close();
    public event Close OnClose;
    public delegate void Open();
    public event Open OnOpen;
    private GameManager gameManager;
    public bool ClosePossible;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        ClosePossible = true;
    }
    public bool RequestClose()
    {
        if (OnClose != null) OnClose();
        
        if (!ClosePossible)
        {
            StartCoroutine(waitForClose());
            return false;
        }
        else
        {
            gameManager.Close(gameObject.name);
            if (gameManager.debugMode)
                Debug.Log(gameObject.name+" has been Closed");
            return true;
        }
    }
    public bool RequestOpen()
    {
        if (OnOpen != null)
            OnOpen();
        return true;
    }

    public void OpenState(string stateName)
    {
        gameManager.Open_state(stateName);
    }

    private bool WaitForClose()
    {
        return ClosePossible;
    }

    IEnumerator waitForClose()
    {
        if(gameManager.debugMode)
            Debug.Log("waiting for "+gameObject.name+" to be closed");
        yield return new WaitUntil(WaitForClose);
        gameManager.Close(gameObject.name);
        if (gameManager.debugMode) 
            Debug.Log(gameObject.name+" has been Closed");
    }
}

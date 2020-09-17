using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Manager> states;
    private List<string> stateNames;
    public bool debugMode;
    [SerializeField] private GameObject LoadingState;
    private int NumOfReadyStates;

    public void Awake()
    {
        stateNames = new List<string>();
        foreach (Manager state in states)
        {
            stateNames.Add(state.gameObject.name);
        }
    }

    public void Start()
    {
        LoadingState.SetActive(false);
        Open_state("MainMenu");
    }

    public void Open_state(string stateName)
    {
        NumOfReadyStates = 0;
        if (stateName != "none")
        {
            NumOfReadyStates =+ 1;
        }
        foreach (Manager stat in states)
        {
            if (stateName != stat.gameObject.name)
            {
                if (stat.gameObject)
                {
                    bool requestOutcome = state(stat.gameObject.name).RequestClose();
                }                
            }
        }

        StartCoroutine(waitForAllToClose(stateName));
    }

    public void Close(string stateName)
    {
        state(stateName).gameObject.SetActive(false);
        NumOfReadyStates += 1;
        Debug.Log("close");
        Debug.Log(NumOfReadyStates);
    }
    public void Open(string stateName)
    {
        state(stateName).gameObject.SetActive(true);
    }
        
    private Manager state(string stateName)
    {
        return states[stateNames.IndexOf(stateName)];
    }

    private bool allStatesAreReady()
    {
        if (NumOfReadyStates == states.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator waitForAllToClose(string stateName)
    {
        yield return new WaitUntil(allStatesAreReady);

        if (stateName != "none")
        {
            state(stateName).gameObject.SetActive(true);
            Open(stateName);
            state(stateName).RequestOpen();
            if (LoadingState)
            {
                LoadingState.SetActive(false);
            }
        }
        else
        {
            LoadingState.SetActive(true);
            
        }
    }
}









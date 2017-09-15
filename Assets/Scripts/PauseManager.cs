using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    public CanvasGroup CanvasGroup;
    public GameObject LoadingCanvas;

    private int ReasonToPause = 0;

    private bool isRunning = true;

    public void AddReasonsToStop(int number)
    {
        Debug.Log(" + " + number);
        ReasonToPause += number;
        Pause();
    }

    public void AddReasonsToResume(int number)
    {
        Debug.Log(" - " + number);
        ReasonToPause -= number;
        ReStart();
    }

    private void Pause()
    {
        if (isRunning)
        {
            //CanvasGroup.interactable = false;
            Debug.Log("Show loading ...");
            LoadingCanvas.SetActive(true);

            isRunning = false;
        }
    }

    private void ReStart()
    {
        if (!isRunning)
        {
            if (ReasonToPause == 0)
            {
                //CanvasGroup.interactable = true;
                Debug.Log("Hide loading ...");
                LoadingCanvas.SetActive(false);

                isRunning = true;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidManager : MonoBehaviour
{
    public RaidGUICtrl attackRaid;
    public RaidGUICtrl defenseRaid;
    public RaidGUICtrl specialRaid;

    [SerializeField]
    private GameObject LoadingCanvas;

    public void LoadStatedParty()
    {
        StartCoroutine(Import());
    }

    private IEnumerator Import()
    {
        LoadingCanvas.SetActive(true);
        // Fix the fact that gooleSheetToUnity freeze the scene
        yield return new WaitForSeconds(0.3f);

        attackRaid.LoadStatedParties();
        defenseRaid.LoadStatedParties();
        specialRaid.LoadStatedParties();

        PlayerManager.Instance.UpdatePlayerButtonsColor();

        LoadingCanvas.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidManager : Singleton<RaidManager>
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

    internal void UnGroupPlayer(Player player)
    {
        bool found = false;

        // Look for it in the attack raid
        foreach (GameObject partyCtrlGo in attackRaid.PartyGoList)
        {
            PartyGUICtrl partyCtrl = partyCtrlGo.GetComponent<PartyGUICtrl>();
            int index = partyCtrl.Party.GetIndex(player);
            if (index != -1)
            {
                found = true;
                partyCtrl.Party.Remove(index);
                partyCtrl.playerSlots[index].Clear();
            }   
        }

        if (!found)
        {
            // Look for it in special raid
            foreach (GameObject partyCtrlGo in specialRaid.PartyGoList)
            {
                PartyGUICtrl partyCtrl = partyCtrlGo.GetComponent<PartyGUICtrl>();
                int index = partyCtrl.Party.GetIndex(player);
                if (index != -1)
                {
                    found = true;
                    partyCtrl.Party.Remove(index);
                    partyCtrl.playerSlots[index].Clear();
                }
            }
        }

        if (!found)
        {
            // Look for it in special raid
            foreach (GameObject partyCtrlGo in defenseRaid.PartyGoList)
            {
                PartyGUICtrl partyCtrl = partyCtrlGo.GetComponent<PartyGUICtrl>();
                int index = partyCtrl.Party.GetIndex(player);
                if (index != -1)
                {
                    found = true;
                    partyCtrl.Party.Remove(index);
                    partyCtrl.playerSlots[index].Clear();
                }
            }
        }
    }
}

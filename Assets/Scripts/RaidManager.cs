using GoogleSheetsToUnity;
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

    public void SaveOnSheets()
    {
        StartCoroutine(SaveCoroutine());
    }

    private IEnumerator SaveCoroutine()
    {
        LoadingCanvas.SetActive(true);
        // Fix the fact that gooleSheetToUnity freeze the scene
        yield return new WaitForSeconds(0.3f);

        Save();

        LoadingCanvas.SetActive(false);
    }

    private void Save()
    {
        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Test Raid Maker").LoadWorkSheet("Save");

        //worksheet.SetWorksheetSize(8, 20);

        List<string> s = worksheet.GetRowTitles();
        List<string> s1 = worksheet.GetColumnTitles();

        //Dictionary<string, string> dico = new Dictionary<string, string>();
        //dico.Add("P1", "coucouuuu");
        //worksheet.ModifyRowData("Attaque 1", dico);

        //SaveOneGroup(attackRaid.PartyGoList[0].GetComponent<PartyGUICtrl>().Party, "attaque", worksheet);

        foreach (GameObject partyGo in attackRaid.PartyGoList)
        {
            Party party = partyGo.GetComponent<PartyGUICtrl>().Party;
            SaveOneGroup(party, attackRaid.Name, worksheet);
        }

        foreach (GameObject partyGo in specialRaid.PartyGoList)
        {
            Party party = partyGo.GetComponent<PartyGUICtrl>().Party;
            SaveOneGroup(party, specialRaid.Name, worksheet);
        }

        foreach (GameObject partyGo in defenseRaid.PartyGoList)
        {
            Party party = partyGo.GetComponent<PartyGUICtrl>().Party;
            SaveOneGroup(party, defenseRaid.Name, worksheet);
        }
    }

    private void SaveOneGroup(Party party, string raid, GS2U_Worksheet worksheet)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();

        // Add the party name
        row.Add("Name", party.PartyName);

        // Add the player name
        for(int i = 1; i < 6; i++)
        {
            if (party.Players[i - 1] != null) { row.Add("P" + i.ToString(), party.Players[i - 1].Name); }
            else { row.Add("P" + i.ToString(), ""); }
        }

        string partyRowId = raid + " " + party.Id;

        // Add the row
        worksheet.ModifyRowData(partyRowId, row);
    }
}

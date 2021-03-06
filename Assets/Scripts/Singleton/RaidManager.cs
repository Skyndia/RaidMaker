﻿using GoogleSheetsToUnity;
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

        PlayerManager.Instance.UngroupAllPlayers();

        attackRaid.LoadStatedParties();
        defenseRaid.LoadStatedParties();
        specialRaid.LoadStatedParties();

        PlayerManager.Instance.UpdatePlayerButtonsColor();

        LoadingCanvas.SetActive(false);
    }

    internal PartyGUICtrl FindPlayer(Player player)
    {
        PartyGUICtrl result = null;
        bool found = false;

        // Look for it in the attack raid
        foreach (GameObject partyCtrlGo in attackRaid.PartyGoList)
        {
            PartyGUICtrl partyCtrl = partyCtrlGo.GetComponent<PartyGUICtrl>();
            int index = partyCtrl.Party.GetIndex(player);
            if (index != -1)
            {
                found = true;
                result = partyCtrl;
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
                    result = partyCtrl;
                }
            }
        }

        if (!found)
        {
            // Look for it in defense raid
            foreach (GameObject partyCtrlGo in defenseRaid.PartyGoList)
            {
                PartyGUICtrl partyCtrl = partyCtrlGo.GetComponent<PartyGUICtrl>();
                int index = partyCtrl.Party.GetIndex(player);
                if (index != -1)
                {
                    found = true;
                    result = partyCtrl;
                }
            }
        }

        return result;
    }

    internal void UnGroupPlayer(Player player)
    {
        PartyGUICtrl partyCtrl = FindPlayer(player);
        if (partyCtrl != null)
        {
            int index = partyCtrl.Party.GetIndex(player);
            partyCtrl.Party.Remove(index);
            partyCtrl.playerSlots[index].Clear();
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
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Raid Maker").LoadWorkSheet("Save");

        //worksheet.SetWorksheetSize(8, 20);

        //Dictionary<string, string> dico = new Dictionary<string, string>();
        //dico.Add("P1", "coucouuuu");
        //worksheet.ModifyRowData("Attaque 1", dico);

        //SaveOneGroup(attackRaid.PartyGoList[0].GetComponent<PartyGUICtrl>().Party, "attaque", worksheet);

        // Clean
        for(int i = 0; i < 5; i++)
        {
            CleanOneGroup(attackRaid.Name, i, worksheet);
            CleanOneGroup(specialRaid.Name, i, worksheet);
            CleanOneGroup(defenseRaid.Name, i, worksheet);
        }

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

    private void CleanOneGroup(string raid, int index, GS2U_Worksheet worksheet)
    {
        Dictionary<string, string> row = new Dictionary<string, string>();

        row.Add("Name", "");
        for(int i = 1; i < 6; i++)
        {
            row.Add("P" + i.ToString(), "");
        }

        string partyRowId = raid + " " + index;

        // Add the (empty) row
        worksheet.ModifyRowData(partyRowId, row);
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

    public void LoadFromSheet()
    {
        // Clear each raid
        ClearRaids();

        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Raid Maker").LoadWorkSheet("Save");
        WorksheetData data = worksheet.LoadAllWorksheetInformation();

        foreach (RowData row in data.rows)
        {
            LoadOneGroup(row);
        }
    }

    private void LoadOneGroup(RowData row)
    {
        Party party = new Party();

        string raid = row.rowTitle.Split(' ')[0];
        int index = int.Parse(row.rowTitle.Split(' ')[1]);

        // Create the party
        string name = row.cells[1].value;
        party.PartyName = name;
        party.Id = index;

        for(int i = 0; i < 5; i++)
        {
            string pName = row.cells[i + 2].value;
            if (PlayerManager.Instance.playerLibrary.ContainsKey(pName))
            {
                Player player = PlayerManager.Instance.playerLibrary[pName];
                party.Players[i] = player;
            }
        }

        // Create a party game object
        if (party.PartyName != "")
        {
            switch (raid)
            {
                case "Attaque":
                    attackRaid.CreatePartyGo(party);
                    break;

                case "Special":
                    specialRaid.CreatePartyGo(party);
                    break;

                case "Defense":
                    defenseRaid.CreatePartyGo(party);
                    break;
            }
        }
    }

    private void ClearRaids()
    {
        PlayerManager.Instance.UngroupAllPlayers();

        foreach (GameObject party in attackRaid.PartyGoList)
        {
            Destroy(party);
        }
        attackRaid.PartyGoList = new List<GameObject>();

        foreach (GameObject party in specialRaid.PartyGoList)
        {
            Destroy(party);
        }
        specialRaid.PartyGoList = new List<GameObject>();

        foreach (GameObject party in defenseRaid.PartyGoList)
        {
            Destroy(party);
        }
        defenseRaid.PartyGoList = new List<GameObject>();
    }

    public string GetAttackRaidName()
    {
        return attackRaid.Name;
    }

    public string GetSpecialRaidName()
    {
        return specialRaid.Name;
    }

    public string GetDefenseRaidName()
    {
        return defenseRaid.Name;
    }

    public void MoveParty(RaidGUICtrl oldRaid, RaidGUICtrl newRaid, PartyGUICtrl partyCtrl)
    {
        oldRaid.PartyGoList.Remove(partyCtrl.gameObject);
        partyCtrl.transform.SetParent(newRaid.transform);
        partyCtrl.RaidName = newRaid.Name;
        newRaid.PartyGoList.Add(partyCtrl.gameObject);
    }
}

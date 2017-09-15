using GoogleSheetsToUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RaidGUICtrl : MonoBehaviour, IPointerClickHandler
{
    public string Name;
    public List<GameObject> PartyGoList;
    private bool statedPartiesLoaded = false;

    [SerializeField]
    private GameObject partyGoPrefab;

    void Start()
    {
        // Create a party list
        PartyGoList = new List<GameObject>();
    }

    public void LoadStatedParties()
    {
        // If the stated parties were already loaded, then delete all the parties
        if (statedPartiesLoaded)
        {
            foreach (GameObject party in PartyGoList)
            {
                Destroy(party);
            }
            PartyGoList = new List<GameObject>();
        }

        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Test Raid Maker").LoadWorkSheet(Name);
        WorksheetData data = worksheet.LoadAllWorksheetInformation();
        
        int partyId = 0;
        
        // The firt row contains the parties names.
        // For each party declared
        foreach (CellData cell in data.rows[0].cells)
        {
            Party party = new Party(cell.value, Name);

            // Retrieve all the players of a party
            for(int i = 1; i < 6; i++)
            {
                string playerName = data.rows[i].cells[partyId * 4].value;
                if (PlayerManager.Instance.playerLibrary.ContainsKey(playerName))
                {
                    Player player = PlayerManager.Instance.playerLibrary[playerName];
                    //mark the player as grouped
                    player.grouped = true;
                    party.AddPlayer(player);
                }
            }

            // Instanciate a gameObject for the party
            GameObject partyGo = GameObject.Instantiate(partyGoPrefab);
            partyGo.transform.SetParent(transform, false);

            PartyGUICtrl partyGuiCtrl = partyGo.GetComponent<PartyGUICtrl>();
            partyGuiCtrl.Party = party;

            // save the party gameObject
            PartyGoList.Add(partyGo);

            partyId++;
        }
        statedPartiesLoaded = true; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerManager.Instance.EndDragPlayer();
    }
}

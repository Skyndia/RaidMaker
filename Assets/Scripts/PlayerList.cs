using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    public GameObject registeredPlayerPrefab;
    public GameObject registeredPlayerList;

    public RaidGUICtrl attaqueGUICtrl;
    public RaidGUICtrl defenseGUICtrl;
    public RaidGUICtrl specialGUICtrl;

    public List<PlayerButton> playerGos;

    // Use this for initialization
    void Start ()
    {
        RetrievePlayerList();
	}

    private void RetrievePlayerList()
    {
        SpreadSheetManager manager = new SpreadSheetManager();
        GS2U_Worksheet worksheet = manager.LoadSpreadSheet("Test Raid Maker").LoadWorkSheet("Groupes NW");

        WorksheetData data = worksheet.LoadAllWorksheetInformation();

        int cpt = 1;
        int security = 0;
        while (data.rows[cpt].cells.Count > 1 && data.rows[cpt].cells[1].value != "" && security < 100)
        {
            RowData row = data.rows[cpt];

            // instanciate the prefab to create a player button
            GameObject registeredPlayerObj = GameObject.Instantiate(registeredPlayerPrefab);
            Text registeredPlayerText = registeredPlayerObj.GetComponentInChildren<Text>();

            // set the button parent
            registeredPlayerObj.transform.SetParent(registeredPlayerList.transform, false);
            // set the button text
            registeredPlayerText.text = data.rows[cpt].cells[1].value;

            // create a player object
            Player player = new Player(row);
            // Add it to the player dictionnary
            if (!PlayerManager.Instance.playerLibrary.ContainsKey(player.Name))
            {
                PlayerManager.Instance.playerLibrary.Add(player.Name, player);
            }

            // Initialize the details of the player button info
            PlayerButton button = registeredPlayerObj.GetComponent<PlayerButton>();
            button.Initialize(player);


            // Add the playerButton to the dictionnary
            playerGos.Add(button);

            cpt++;
            security++;
        }
    }

    public void UpdatePlayerColors()
    {
        foreach(PlayerButton button in playerGos)
        {
            button.SetAsGrouped(PlayerManager.Instance.playerLibrary[button.playerName.text].grouped);
        }
    }
}

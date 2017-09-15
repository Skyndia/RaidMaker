using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyGUICtrl : MonoBehaviour
{
    public GameObject ContentPanel;
    public Toggle Toggle;
    public PlayerSlot[] playerSlots;
    public Text partyName;

    private Party _party;
    public Party Party
    {
        get
        {
            return _party;
        }

        set
        {
            _party = value;
            SetFieldsValues();
        }
    }

    public void SetFieldsValues()
    {
        partyName.text = _party.PartyName;

        for (int i = 0; i < 5; i++)
        {
            PlayerSlot playerSlot = playerSlots[i];
            Player player = _party.Players[i];
            
            if (player != null)
            {
                playerSlot.Fill(player);
            }
            else
            {
                playerSlot.Clear();
            }
        }
    }

    public void TogglePressed()
    {
        ContentPanel.SetActive(Toggle.isOn);
    }
}

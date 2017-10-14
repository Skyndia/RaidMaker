using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyGUICtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject ContentPanel;
    public Toggle Toggle;
    public PlayerSlot[] playerSlots;
    public Text partyName;
    public string RaidName;

    private Vector3 startDragPosition;
    
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        string attackRaidName = RaidManager.Instance.GetAttackRaidName();
        string specialRaidName = RaidManager.Instance.GetSpecialRaidName();
        string defenseRaidName = RaidManager.Instance.GetDefenseRaidName();
        RaidManager raidManager = RaidManager.Instance;

        // Drag to right
        if (startDragPosition.x < transform.position.x)
        {
            
            // Current raid : attack
            if (RaidName == RaidManager.Instance.GetAttackRaidName())
            {
                // to special raid
                raidManager.MoveParty(raidManager.attackRaid, raidManager.specialRaid, this);
            }
            // Current raid : special
            else if (RaidName == RaidManager.Instance.GetSpecialRaidName())
            {
                // to defense raid
                raidManager.MoveParty(raidManager.specialRaid, raidManager.defenseRaid, this);
            }
            // Current raid : defense
            else if (RaidName == RaidManager.Instance.GetDefenseRaidName())
            {
                // don't move
                transform.SetParent(raidManager.defenseRaid.transform);
            }
        }
        // Drag to left
        else
        {
            // Current raid : attack
            if (RaidName == RaidManager.Instance.GetAttackRaidName())
            {
                // don't move
                transform.SetParent(raidManager.attackRaid.transform);
            }
            // Current raid : special
            else if (RaidName == RaidManager.Instance.GetSpecialRaidName())
            {
                // to attack raid
                raidManager.MoveParty(raidManager.specialRaid, raidManager.attackRaid, this);
            }
            // Current raid : defense
            else if (RaidName == RaidManager.Instance.GetDefenseRaidName())
            {
                // to special raid
                raidManager.MoveParty(raidManager.defenseRaid, raidManager.specialRaid, this);
            }
        }
    }

}

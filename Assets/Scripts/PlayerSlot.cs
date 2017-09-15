using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour, IPointerClickHandler
{
    public Text NameText;
    public Text StatsText;
    public Image ClassImage;
    public int slotId;
    public PartyGUICtrl PartyCtrl;
    public Player Player;

    private bool _filled = false;

	// Use this for initialization
	void Start ()
    {
        // Hide the image
        if (ClassImage.sprite == null)
        {
            HideImage();
        }
    }

    internal void HideImage()
    {
        ClassImage.color = new Color(1, 1, 1, 0);
    }

    internal void ShowImage(Sprite classIcon)
    {
        ClassImage.sprite = classIcon;
        ClassImage.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If we are already dragging, paste the player here
        if(PlayerManager.Instance.IsDragging)
        {
            if (_filled)
            {
                PlayerSlot origin = PlayerManager.Instance.OriginSlot;
                if (origin != null)
                {
                    // Stop the dragging
                    Player originPlayer = PlayerManager.Instance.EndDragPlayer();

                    // Fill the origin slot
                    origin.Fill(Player);

                    // Fill this slot
                    Fill(originPlayer);
                }
            }
            else
            {
                // Stop the dragging
                Player player = PlayerManager.Instance.EndDragPlayer();

                // Group the slot
                Fill(player);

                PlayerManager.Instance.UpdatePlayerButtonsColor();
            }
            
        }
        // If we are not already dragging a player, start to drag this one
        else if (_filled)
        {
            PlayerManager.Instance.DragPlayer(Player, this);
            Clear();
        }
    }

    public void Fill(Player player)
    {
        Player = player;
        NameText.text = player.Name;
        StatsText.text = player.GearScore.ToString();
        ShowImage(player.ClassIcon);
        // Notify the player as grouped
        player.grouped = true;


        PartyCtrl.Party.Players[slotId] = player;
        _filled = true;
    }

    public void Clear()
    {
        Player.grouped = false;
        Player = null;
        NameText.text = "";
        StatsText.text = "";
        HideImage();

        PartyCtrl.Party.Players[slotId] = null;
        _filled = false;
    }
}

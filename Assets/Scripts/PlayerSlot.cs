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

    private bool _filled = false;

	// Use this for initialization
	void Start ()
    {
        // Hide the image
        if (ClassImage.sprite == null)
        {
            ClassImage.color = new Color(1, 1, 1, 0);
        }
    }

    internal void ShowImage(Sprite classIcon)
    {
        ClassImage.sprite = classIcon;
        ClassImage.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerManager.Instance.IsDragging)
        {
            // Stop the dragging
            Player player = PlayerManager.Instance.EndDragPlayer();

            // Notify the player as grouped
            player.grouped = true;

            // Group the player
            PartyCtrl.Party.Players[slotId] = player;
            Fill(player);

            PlayerManager.Instance.UpdatePlayerButtonsColor();
        }
    }

    public void Fill(Player player)
    {
        NameText.text = player.Name;
        StatsText.text = player.GearScore.ToString();
        ShowImage(player.ClassIcon);

        _filled = true;
    }
}

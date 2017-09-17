using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Toggle toggle;
    public Text playerName;
    public Image classIcon;
    public Text ApText;
    public Text DpText;
    public Image Background;
    public Player Player;
    // If you are a ghost, there are things you're not supposed to do
    public bool IsGhost = false;

    public void Initialize(Player player)
    {
        playerName.text = player.Name;
        classIcon.sprite = player.ClassIcon;
        ApText.text = player.Ap.ToString();
        DpText.text = player.Dp.ToString();
        SetAsGrouped(false);
        Player = player;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsGhost)
        {
            toggle.isOn = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsGhost)
        {
            toggle.isOn = false;
        }
    }

    public void SetAsGrouped(bool grouped)
    {
        if (grouped)
        {
            Background.color = new Color(1, 1, 1, 1);
        }
        else
        {
            Background.color = new Color(197.0f / 255.0f, 197.0f / 255.0f, 197.0f/255.0f, 1.0f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsGhost)
        {
            //If we are already dragging a player
            if (PlayerManager.Instance.IsDragging)
            {
                // Stop the current draging operation
                PlayerManager.Instance.EndDragPlayer();
            }

            // Inform the manager that we are dragging this player and create a ghost
            PlayerManager.Instance.DragPlayer(Player);
        }
            
    }

    void Update()
    {
        if (IsGhost)
        {
            transform.position = Input.mousePosition;
        }
    }
}

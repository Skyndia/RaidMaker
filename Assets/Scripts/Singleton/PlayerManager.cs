using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    // Player dictionnary. By names
    public Dictionary<string, Player> playerLibrary = new Dictionary<string, Player>();
    public PlayerList PlayerList;
    public GameObject GhostPrefab;
    public GameObject GhostCanvas;
    public GameObject Ghost = null;
    public PlayerSlot OriginSlot = null;

    public bool IsDragging = false;
    private Player DragAndDropPlayer;

    public void UpdatePlayerButtonsColor()
    {
        PlayerList.UpdatePlayerColors();
    }

    public GameObject DragPlayer(Player player, PlayerSlot slot = null)
    {
        DragAndDropPlayer = player;
        IsDragging = true;
        Ghost = CreateGhost();

        PlayerButton ghostButton = Ghost.GetComponent<PlayerButton>();

        ghostButton.IsGhost = true;
        ghostButton.Initialize(player);

        Ghost.transform.position = transform.position;

        if (slot != null)
        {
            OriginSlot = slot;
        }

        return Ghost;
    }

    internal void UngroupAllPlayers()
    {
        foreach (KeyValuePair<string, Player> player in playerLibrary)
        {
            player.Value.grouped = false;
        }
    }

    public Player EndDragPlayer()
    {
        OriginSlot = null;
        IsDragging = false;
        if (Ghost != null) { Destroy(Ghost); }
        Ghost = null;
        return DragAndDropPlayer;
    }

    private GameObject CreateGhost()
    {
        GameObject ghost = GameObject.Instantiate(GhostPrefab);
        ghost.transform.SetParent(GhostCanvas.transform, false);

        return ghost;
    }


}

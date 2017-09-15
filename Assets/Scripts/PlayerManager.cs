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

    public bool IsDragging = false;
    private Player DragAndDropPlayer;

    public void UpdatePlayerButtonsColor()
    {
        PlayerList.UpdatePlayerColors();
    }

    public GameObject DragPlayer(Player player)
    {
        DragAndDropPlayer = player;
        IsDragging = true;
        Ghost = CreateGhost();
        return Ghost;
    }

    public Player EndDragPlayer()
    {
        IsDragging = false;
        Destroy(Ghost);
        Ghost = null;
        return DragAndDropPlayer;
    }

    public GameObject CreateGhost()
    {
        GameObject ghost = GameObject.Instantiate(GhostPrefab);
        ghost.transform.SetParent(GhostCanvas.transform, false);

        return ghost;
    }


}

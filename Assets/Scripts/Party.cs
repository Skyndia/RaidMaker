using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party
{
    public string PartyName;
    public Player[] Players;
    public int playerNb;

    public Party()
    {
        Players = new Player[5];
        playerNb = 0;
    }
    
    public Party(string name)
    {
        Players = new Player[5];
        PartyName = name;
        playerNb = 0;
    }

    public void AddPlayer(Player player)
    {
        if (playerNb < 5)
        {
            Players[playerNb] = player;
            playerNb++;
        }
        else
        {
            Debug.LogError("No more space in the party " + PartyName);
            // Should trigger an alert
        }
    }
}

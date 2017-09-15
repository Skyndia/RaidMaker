using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party
{
    public string PartyName;
    public string BelongsTo;
    public Player[] Players;
    public int playerNb;

    public Party()
    {
        Players = new Player[5];
        playerNb = 0;
    }
    
    public Party(string name, string raid)
    {
        Players = new Player[5];
        PartyName = name;
        BelongsTo = raid;
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

    public int GetIndex(Player player)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i] == player)
            {
                return i;
            }
        }

        return -1;
    }

    public void Remove(int i)
    {
        Players[i] = null;
    }
}

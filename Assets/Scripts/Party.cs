using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party
{
    public string PartyName;
    public string BelongsTo;
    public Player[] Players;
    public int Id;

    public Party()
    {
        Players = new Player[5];
    }
    
    public Party(string name, string raid, int id)
    {
        Id = id;
        Players = new Player[5];
        PartyName = name;
        BelongsTo = raid;
    }

    public void AddPlayer(Player player)
    {
        int cpt = 0;
        while (Players[cpt] != null && cpt < 5) { cpt++; }
        Players[cpt] = player;
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

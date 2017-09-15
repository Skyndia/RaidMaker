using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;

public class Player
{
    public Sprite ClassIcon;
    public string ClassName;
    // Id
    public string Name;
    public int Level;
    public int Ap;
    public int Dp;
    public int GearScore;

    public bool grouped = false;

    public Player()
    {

    }

    public Player(RowData row)
    {
        Name = row.cells[1].value;

        #region switch classes
        ClassName = row.cells[2].value;
        switch (row.cells[2].value)
        {
            case "Magicien":
                ClassIcon = ClassesManager.Instance.wizIcon;
                break;
            case "Magicienne":
                ClassIcon = ClassesManager.Instance.witchIcon;
                break;
            case "Lame Sombre" :
                ClassIcon = ClassesManager.Instance.dkIcon;
                break;
               case "Sorcière" :
                ClassIcon = ClassesManager.Instance.sorcIcon;
                break;
            case "Musa":
                ClassIcon = ClassesManager.Instance.musaIcon;
                break;
            case "Maewha":
                ClassIcon = ClassesManager.Instance.maehwaIcon;
                break;
            case "Guerrier":
                ClassIcon = ClassesManager.Instance.warIcon;
                break;
            case "Valkyrie":
                ClassIcon = ClassesManager.Instance.valkIcon;
                break;
            case "Berserker":
                ClassIcon = ClassesManager.Instance.zerkIcon;
                break;
            case "Kunoichi":
                ClassIcon = ClassesManager.Instance.KunoIcon;
                break;
            case "Ninja":
                ClassIcon = ClassesManager.Instance.ninjaIcon;
                break;
            case "Dompteuse":
                ClassIcon = ClassesManager.Instance.tamerIcon;
                break;
            case "Ranger":
                ClassIcon = ClassesManager.Instance.rangerIcon;
                break;
            case "Striker":
                ClassIcon = ClassesManager.Instance.strikerIcon;
                break;
            default:
                break;
        }
        #endregion

        int.TryParse(row.cells[3].value, out Ap);
        int.TryParse(row.cells[4].value, out Dp);
        int.TryParse(row.cells[5].value, out GearScore);
    }
}

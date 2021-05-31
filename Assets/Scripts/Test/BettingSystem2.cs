using UnityEngine;

public class BettingSystem2
{
    public int GetMaxAmountOfCellsVertically()
    {
        return (Screen.height - 200) / 50;
    }

    public int GetMaxAmountOfCellsHorizontally()
    {
        return Screen.width / 50;
    }
}
using UnityEngine;

public class Dice
{
    public int GetRandomDotsAmount()
    {
        return Random.Range(0, 6);
    }
}
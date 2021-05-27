using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] public int AmountOfDices = 1;
    [SerializeField] public int BetBase = 1;

    public readonly int DiceSidesAmount = 6;
    
    public readonly int MinAmountOfDices = 1;
    public readonly int MaxAmountOfDices = 3;
    
    public readonly int WinMultiplierForNoSelection = 0;
    public readonly int WinMultiplierForOneSelected = 3;
    public readonly int WinMultiplierForTwoSelected = 2;
    public readonly int WinMultiplierForThreeSelected = 1;

    public readonly int MaxSelectedCellsAmount = 3;
    
    public float GetMaxSliderValue => AmountOfDices * DiceSidesAmount;
    public float GetLowestValueForCurrentDicesAmount => AmountOfDices;
    public float GetHighestValueForCurrentDicesAmount => AmountOfDices * DiceSidesAmount;

}
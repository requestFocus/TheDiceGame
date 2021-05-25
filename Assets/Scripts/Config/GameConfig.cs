using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] public int AmountOfDices = 2;

    public int DiceSidesAmount = 6;
    
    public int MinAmountOfDices = 1;
    public int MaxAmountOfDices = 3;

    public int BetBase = 1;
    
    public int WinMultiplierForNoSelection = 0;
    public int WinMultiplierForOneSelected = 3;
    public int WinMultiplierForTwoSelected = 2;
    public int WinMultiplierForThreeSelected = 1;

    public int MaxSelectedCellsAmount = 3;
    
    public float GetMaxSliderValue => AmountOfDices * DiceSidesAmount;
    public float GetLowestValueForCurrentDicesAmount => AmountOfDices;
    public float GetHighestValueForCurrentDicesAmount => AmountOfDices * DiceSidesAmount;

}
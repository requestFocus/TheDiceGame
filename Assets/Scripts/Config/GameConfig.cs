using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] public int AmountOfDices = 2;

    private int DiceSidesAmount = 6;
    public float GetMaxSliderValue => AmountOfDices * DiceSidesAmount;

    public int MinAmountOfDices = 1;
    public int MaxAmountOfDices = 3;
}
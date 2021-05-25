using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DicesManager
{
    private DicePresenter.Factory _dicePresenterFactory;
    private GameConfig _gameConfig;

    private List<Vector2> _occupied = new List<Vector2>();
    private List<DicePresenter> _dicePresenters = new List<DicePresenter>();

    private DicesManager(DicePresenter.Factory factory, GameConfig gameConfig)
    {
        _dicePresenterFactory = factory;
        _gameConfig = gameConfig;
    }

    public Vector2 GetUniqueRandomPosition(float contentWidth, float contentHeight, float[] dimensions)
    {
        float x;
        float y;
        
        do
        {
            x = Random.Range(-contentWidth/2 + dimensions[0], contentWidth/2 - dimensions[0]);
            y = Random.Range(-contentHeight/2 + dimensions[1], contentHeight/2 - dimensions[1]);
        }
        while (IsPositionUnavailable(x, y, dimensions));
        
        _occupied.Add(new Vector2(x, y));

        return new Vector2(x, y);
    }

    private bool IsPositionUnavailable(float x, float y, float[] dimensions)
    {
        var offsetX = dimensions[0] /2;
        var offsetY = dimensions[1] /2;

        bool insideX = false;
        bool insideY = false;

        List<bool> occupationStatus = new List<bool>();
        
        foreach (var vector2 in _occupied)
        {
            if (x - offsetX > vector2.x - offsetX && x - offsetX < vector2.x + offsetX 
                || x + offsetX > vector2.x - offsetX && x + offsetX < vector2.x + offsetX)
            {
                insideX = true;
            }
            
            if (y - offsetY > vector2.y - offsetY && y - offsetY < vector2.y + offsetY
                || y + offsetY > vector2.y - offsetY && y + offsetY < vector2.y + offsetY)
            {
                insideY = true;
            }

            occupationStatus.Add(insideX || insideY);
        }

        return occupationStatus.Exists(status => status);
    }

    public void CreateDices()
    {
        for (int i = 0; i < _gameConfig.AmountOfDices; i++)
        {
            DicePresenter dice = _dicePresenterFactory.Create();
            _dicePresenters.Add(dice);
        }
    }

    public void RemoveDices()
    {
        _dicePresenters.Clear();
    }

    public List<DicePresenter> GetDicesPresenters()
    {
        return _dicePresenters;
    }

    public void ClearOccupiedPositions()
    {
        _occupied.Clear();
    }
}

using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class DicesManager
{
    [Inject] private DicePresenter.Factory _dicePresenterFactory;

    private List<Vector2> _occupied = new List<Vector2>();

    private float _x;
    private float _y;

    public Vector2 GetUniqueRandomPosition(float contentWidth, float contentHeight, float[] dimensions)
    {
        do
        {
            _x = Random.Range(-contentWidth/2 + dimensions[0], contentWidth/2 - dimensions[0]);
            _y = Random.Range(-contentHeight/2 + dimensions[1], contentHeight/2 - dimensions[1]);
        }
        while (IsPositionUnavailable(_x, _y, dimensions));
        
        _occupied.Add(new Vector2(_x, _y));

        return new Vector2(_x, _y);
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

    public List<DicePresenter> CreateDices()
    {
        List<DicePresenter> dicePresenters = new List<DicePresenter>();

        for (int i = 0; i < 1; i++)
        {
            DicePresenter dice = _dicePresenterFactory.Create();
            dicePresenters.Add(dice);
        }
        
        return dicePresenters;
    }

    public void ClearOccupiedPositions()
    {
        _occupied.Clear();
    }
}

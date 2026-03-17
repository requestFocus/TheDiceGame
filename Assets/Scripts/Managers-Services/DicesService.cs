using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DicesService
{
    private readonly DicePresenter.Factory _dicePresenterFactory;
    private readonly GameConfig _gameConfig;

    private readonly List<Vector2> _occupied = new List<Vector2>();
    private readonly List<DicePresenter> _dicePresenters = new List<DicePresenter>();

    private DicesService(DicePresenter.Factory factory, GameConfig gameConfig)
    {
        _dicePresenterFactory = factory;
        _gameConfig = gameConfig;
    }

    public int GetDicesSum()
    {
        int sum = 0;
        foreach (var dice in _dicePresenters)
        {
            sum += dice.GetDiceValue();
        }

        return sum;
    }

    private Vector2 GetUniqueRandomPosition(float contentWidth, float contentHeight, float[] dimensions)
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
    
    public async UniTask DistributeDices(RectTransform contentTransform)
    {
        Rect contentRect = contentTransform.rect;
        Sequence sequence = DOTween.Sequence();
        
        foreach (DicePresenter dicePresenter in GetDicesPresenters())
        {
            var targetDotsAmount = dicePresenter.GetGeneratedRandomId();
            Transform diceTransform = dicePresenter.transform;

            Vector2 commonStartingPosition = new Vector3(contentRect.width, -contentRect.height);
            Vector2 uniqueRandomLandingPosition = GetUniqueRandomPosition(contentRect.width,
                contentRect.height, dicePresenter.GetDimensions());
            
            diceTransform.SetParent(contentTransform, true);
            diceTransform.localScale = Vector3.one;
            diceTransform.localPosition = commonStartingPosition;

            float angle = Mathf.Atan2(commonStartingPosition.y - uniqueRandomLandingPosition.y, commonStartingPosition.x - uniqueRandomLandingPosition.x) * Mathf.Rad2Deg;
            dicePresenter.GetDiceImage().transform.rotation = Quaternion.Euler (new Vector3(0f,0f,angle));
            dicePresenter.GetAnimatedSidesContainerTransform().transform.rotation = Quaternion.Euler (new Vector3(0f,0f,angle));

            sequence = DOTween.Sequence()
                .Append(diceTransform.DOLocalMove(uniqueRandomLandingPosition, 1.2f)) // SET EASE
                .InsertCallback(0f, () =>
                {
                    dicePresenter.AnimateDiceMovement(targetDotsAmount);
                });
        }

        await sequence.Play().AsyncWaitForCompletion();
    }

    public void RemoveDices()
    {
        _dicePresenters.Clear();
    }

    private List<DicePresenter> GetDicesPresenters()
    {
        return _dicePresenters;
    }

    public void ClearOccupiedPositions()
    {
        _occupied.Clear();
    }
}

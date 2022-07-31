using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayPanel
{
    private readonly UiManager _uiManager;
    private readonly DicesManager _dicesManager;
    private readonly BettingCellsManager _bettingCellsManager;
    private readonly GameplayManager _gameplayManager;

    private const string BALANCE = "Balance";
    
    public GameplayPanel(UiManager uiManager, 
        DicesManager dicesManager,
        BettingCellsManager bettingCellsManager,
        GameplayManager gameplayManager)
    {
        _uiManager = uiManager;
        _dicesManager = dicesManager;
        _bettingCellsManager = bettingCellsManager;
        _gameplayManager = gameplayManager;
    }
    
    public void GoBack()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }

    public void ShowHelp()
    {
        _uiManager.ShowWindow<HowToPlayWindow>();
    }
    
    public void SetupDices()
    {
        _dicesManager.RemoveDices();
        ClearPreviouslyOccupiedPositions();
        _dicesManager.CreateDices();
    }
    
    private void ClearPreviouslyOccupiedPositions()
    {
        _dicesManager.ClearOccupiedPositions();
    }

    public async UniTask DistributeDices(RectTransform contentTransform)
    {
        await _dicesManager.DistributeDices(contentTransform);
    }
    
    public bool CanContinue()
    {
        return PlayerPrefs.GetInt(BALANCE) > 0;
    }
    
    public List<BettingCellPresenter> GetSelectedCellsPresenters()
    {
        return _bettingCellsManager.GetSelectedCellsPresenters();
    }

    public void OnDicesDistributed()
    {
        _gameplayManager.OnDicesDistributed();
    }
}
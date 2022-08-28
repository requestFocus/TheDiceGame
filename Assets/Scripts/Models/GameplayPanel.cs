using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameplayPanel : IInitializable, IDisposable
{
    private readonly UiManager _uiManager;
    private readonly DicesManager _dicesManager;
    private readonly DiceAmountChange _diceAmountChange;
    private readonly BettingSystem _bettingSystem;
    private readonly Bank _bank;
    private readonly WinScoreIndicator _winScoreIndicator;

    private const string BALANCE = "Balance";
    
    public event Action DiceAmountChangedRedirect;
    public event Action<int, List<BettingCellPresenter>> DicesDistributed;

    public GameplayPanel(UiManager uiManager,
        DicesManager dicesManager,
        DiceAmountChange diceAmountChange,
        BettingSystem bettingSystem,
        Bank bank,
        WinScoreIndicator winScoreIndicator)
    {
        _uiManager = uiManager;
        _dicesManager = dicesManager;
        _diceAmountChange = diceAmountChange;
        _bettingSystem = bettingSystem;
        _bank = bank;
        _winScoreIndicator = winScoreIndicator;
    }

    public void Initialize()
    {
        _diceAmountChange.DiceAmountChanged += UpdateDiceAmountDisplay;
        
        _bettingSystem.CellTapped += UpdatePotentialWin;
        _bettingSystem.CellTapped += UpdateMultiplier;

        DicesDistributed += MarkWinningCell;
        DicesDistributed += UpdateWinScoreIndicator;
        DicesDistributed += UpdatePostTossOutcome;

        _bank.SliderUpdated += GetSelectedCellsPresenters;
    }

    private void UpdateDiceAmountDisplay()
    {
        _bank.DiceAmountChanged(_bettingSystem.GetSelectedCellsPresenters());
    }
    
    private void UpdatePotentialWin(List<BettingCellPresenter> selectedCellPresenters)
    {
        _bank.UpdatePotentialWin(selectedCellPresenters);
    }
    
    private void UpdateMultiplier(List<BettingCellPresenter> selectedCellPresenters)
    {
        _bank.UpdatePotentialWin(selectedCellPresenters);
    }

    private void UpdatePostTossOutcome(int totalScore, List<BettingCellPresenter> selectedCellPresenter)
    {
        _bank.UpdatePostTossOutcome(totalScore, selectedCellPresenter);
    }

    private void MarkWinningCell(int totalScore, List<BettingCellPresenter> selectedCellPresenters)
    {
        _bettingSystem.MarkWinningCell(totalScore, selectedCellPresenters);
    }

    private void UpdateWinScoreIndicator(int totalScore, List<BettingCellPresenter> selectedCellPresenters)
    {
        _winScoreIndicator.UpdateWinScoreIndicator(totalScore, selectedCellPresenters);
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
        return _bettingSystem.GetSelectedCellsPresenters();
    }

    public void OnDicesDistributed()
    {
        DicesDistributed?.Invoke(_dicesManager.GetDicesSum(), _bettingSystem.GetSelectedCellsPresenters());
    }

    public void Dispose()
    {
        _diceAmountChange.DiceAmountChanged -= UpdateDiceAmountDisplay;
        
        _bettingSystem.CellTapped -= UpdatePotentialWin;
        _bettingSystem.CellTapped -= UpdateMultiplier;

        DicesDistributed -= MarkWinningCell;
        DicesDistributed -= UpdateWinScoreIndicator;
        DicesDistributed -= UpdatePostTossOutcome;
        
        _bank.SliderUpdated -= GetSelectedCellsPresenters;
    }
}
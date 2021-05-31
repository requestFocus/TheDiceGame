public class Cell2
{
    private bool _isSelected;
    private int _cellId;

    public void SelectCell()
    {
        _isSelected = true;
    }

    public void DeselectCell()
    {
        _isSelected = false;
    }

    public bool GetSelectedState()
    {
        return _isSelected;
    }

    public int GetCellId()
    {
        return _cellId;
    }

    public void SetCellId(int value)
    {
        _cellId = value;
    }
}
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericButton : Selectable, IPointerClickHandler
{
    private float holdTime = 0.5f;
 
    public UnityEvent onLongPress = new UnityEvent();
    public UnityEvent onClick = new UnityEvent();

    private bool _longPressInvoked;
 
    public override void OnPointerDown(PointerEventData eventData)
    {
        /* if pointer's down then it will invoke in holdTime seconds */
        Invoke(nameof(OnLongPressAction), holdTime);
    }
 
    public override void OnPointerUp(PointerEventData eventData)
    {
        /* if pointer's up then cancel invoking long press action */
        CancelInvoke(nameof(OnLongPressAction));
    }
 
    public override void OnPointerExit(PointerEventData eventData)
    {
        /* if pointer's exit then cancel invoking long press action */
        CancelInvoke(nameof(OnLongPressAction));
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        /* standard click */
        Invoke(nameof(OnClickAction), 0);
    }
 
    private void OnClickAction()
    {
        if (interactable && !_longPressInvoked)
        {
            onClick?.Invoke();
        }

        _longPressInvoked = false;
    }
    
    private void OnLongPressAction()
    {
        onLongPress?.Invoke();
        _longPressInvoked = true;
    }
}

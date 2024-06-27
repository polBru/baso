using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ButtonBehaviour : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    public float timer;
    public float actualTimmer;
    private bool isPressed = false;
    private bool hasPressed = false;

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private UnityEvent m_OnClick = new UnityEvent();

    [FormerlySerializedAs("onHold")]
    [SerializeField]
    private UnityEvent m_OnHold = new UnityEvent();

    public UnityEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    public UnityEvent onHold
    {
        get { return m_OnHold; }
        set { m_OnHold = value; }
    }

    private void Start()
    {
        actualTimmer = timer;
    }

    public void OnUpdateSelected(BaseEventData data)
    {
        if (isPressed)
        {
            actualTimmer -= Time.deltaTime;
            if (actualTimmer <= 0)
            {
                m_OnHold.Invoke();
                hasPressed = false;
                isPressed = false;
                return;
            }
        } else if (hasPressed)
        {
            hasPressed = false;
            if (actualTimmer > 0) {
                m_OnClick.Invoke();
                hasPressed = false;
            }
            actualTimmer = timer;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
        hasPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
        ResetTimer();
    }

    private void ResetTimer()
    {
        isPressed = false;
        actualTimmer = timer;
    }
}

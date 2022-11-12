using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    [SerializeField]
    private IntEvent gameEvent;
    [SerializeField]
    private UnityEvent<int> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised(int value)
    {
        response.Invoke(value);
    }
}

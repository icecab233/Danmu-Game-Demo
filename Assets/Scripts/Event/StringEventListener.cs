using UnityEngine;
using UnityEngine.Events;

public class StringEventListener : MonoBehaviour
{
    [SerializeField]
    private StringEvent gameEvent;
    [SerializeField]
    private UnityEvent<string> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised(string value)
    {
        response.Invoke(value);
    }
}

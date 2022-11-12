using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListener : MonoBehaviour
{
    [SerializeField]
    private PlayerEvent gameEvent;
    [SerializeField]
    private UnityEvent<Player> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

    public void OnEventRaised(Player value)
    {
        response.Invoke(value);
    }
}

using UnityEngine;

namespace DanmuGame.events
{
    [CreateAssetMenu(fileName = "New Player Event", menuName = "Game Events/Player Event")]
    public class PlayerEvent : BaseGameEvent<Player> { }
}
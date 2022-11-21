using UnityEngine;

namespace DanmuGame.events
{
    [CreateAssetMenu(fileName = "New GameObject Event", menuName = "Game Events/GameObject Event")]
    public class GameObjectEvent : BaseGameEvent<GameObject> { }
}
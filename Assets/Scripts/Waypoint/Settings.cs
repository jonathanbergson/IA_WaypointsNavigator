using UnityEngine;

namespace Waypoint
{
    [CreateAssetMenu(fileName = "Waypoint Settings", menuName = "Waypoint System/Settings")]
    public class Settings : ScriptableObject
    {
        [Header("Materials")]
        public Material standart;
        public Material selected;
        public Material headGraph;
        public Material tailGraph;

        [Header("Prefab")]
        public Waypoint prefab;
    }
}

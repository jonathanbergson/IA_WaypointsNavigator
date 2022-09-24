using System.Collections.Generic;
using UnityEngine;

namespace Waypoint
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Waypoint : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        [SerializeField] private Material defaultMaterial;

        [SerializeField] private Type type;
        [SerializeField] private List<Waypoint> waypoints = new();

        public enum Type
        {
            Head,
            Tail,
            Standart,
        }

        private void Awake()
        {
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void OnMouseDown()
        {
            if (type == Type.Tail) return;

            Manager.Instance.SetSelected(this);
            SetSelectedMaterial();
        }

        public void SetType(Type waypointType)
        {
            type = waypointType switch
            {
                Type.Head => Type.Head,
                Type.Tail => Type.Tail,
                _ => Type.Standart
            };

            DefineDefaultMaterial();
        }

        #region Define Material
        private void DefineDefaultMaterial()
        {
            Settings settings = Manager.Instance.settings;

            defaultMaterial = type switch
            {
                Type.Head => settings.headGraph,
                Type.Tail => settings.tailGraph,
                _ => settings.standart
            };

            ResetMaterial();
        }

        public void ResetMaterial()
        {
            _meshRenderer.material = defaultMaterial;
        }

        public void SetSelectedMaterial()
        {
            _meshRenderer.material = Manager.Instance.settings.selected;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Waypoint
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Waypoint : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        [SerializeField] private Material defaultMaterial;

        public Types Type { get; private set; }
        [SerializeField] private List<Waypoint> waypoints = new();

        public enum Types
        {
            Head,
            Tail,
            Standart,
        }

        private void Awake()
        {
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            Color color = Manager.Instance.settings.standart.color;
            foreach (var waypoint in waypoints)
            {
                Debug.DrawLine(transform.position, waypoint.transform.position, color);
            }
        }

        private void OnMouseDown()
        {
            if (Type == Types.Tail)
            {
                Manager.Instance.AddConnection(this);
                return;
            }
            Manager.Instance.SetSelected(this);
        }

        public void SetType(Types waypointTypes)
        {
            Type = waypointTypes switch
            {
                Types.Head => Types.Head,
                Types.Tail => Types.Tail,
                _ => Types.Standart
            };

            SetDefaultMaterial();
        }

        public void AddConnection(Waypoint connection)
        {
            waypoints.Add(connection);
        }

        #region Define Material
        private void SetDefaultMaterial()
        {
            Settings settings = Manager.Instance.settings;

            defaultMaterial = Type switch
            {
                Types.Head => settings.headGraph,
                Types.Tail => settings.tailGraph,
                _ => settings.standart
            };

            ResetMaterial();
        }

        public void SetSelectedMaterial()
        {
            _meshRenderer.material = Manager.Instance.settings.selected;
        }

        public void ResetMaterial()
        {
            _meshRenderer.material = defaultMaterial;
        }
        #endregion
    }
}

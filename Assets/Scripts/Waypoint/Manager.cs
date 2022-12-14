using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Waypoint
{
    public class Manager : MonoBehaviour
    {
        private Waypoint _head;
        private Waypoint _tail;
        private Waypoint _selected;

        public Settings settings;
        public static Manager Instance { get; private set; }

        public Transform npc;
        [SerializeField] private bool _isMoving;
        [SerializeField] private Waypoint _npcTarget;

        private void Awake()
        {
            if (Instance) Destroy(this);
            else Instance = this;
        }

        private void Start()
        {
            if (!settings.prefab) return;

            CreateHeadWaypoint();
            CreateTailWaypoint();
        }

        private void Update()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            HandleClick();
            HandleMove();
            if (_isMoving) HandleMoveNpc();
        }

        private void HandleClick()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 3000)) return;

            Waypoint waypoint = CreateStandardWaypoint(ray.origin.x, ray.origin.y);
            AddConnection(waypoint);
            SetSelected(waypoint);
        }

        private void HandleMove()
        {
            if (!Input.GetKeyDown(KeyCode.M)) return;

            CalcWeightPath();

            _npcTarget = _head;
            _isMoving = true;
        }

        private void CreateHeadWaypoint()
        {
            _head = Instantiate(settings.prefab, null);
            _head.SetType(Waypoint.Types.Head);
            _head.gameObject.transform.position = new Vector3(-12, 6);

            SetSelected(_head);
        }

        private void CreateTailWaypoint()
        {
            _tail = Instantiate(settings.prefab, null);
            _tail.SetType(Waypoint.Types.Tail);
            _tail.gameObject.transform.position = new Vector3(12, -6);
        }

        private Waypoint CreateStandardWaypoint(float x, float y)
        {
            Waypoint waypoint = Instantiate(settings.prefab, null);
            waypoint.SetType(Waypoint.Types.Standart);
            waypoint.gameObject.transform.position = new Vector3(x, y);

            return waypoint;
        }

        public void AddConnection(Waypoint connection)
        {
            if(_selected) _selected.AddConnection(connection);
        }

        public void SetSelected(Waypoint selected)
        {
            if(_selected) _selected.ResetMaterial();

            _selected = selected;

            if (_selected.Type == Waypoint.Types.Standart)
                _selected.SetSelectedMaterial();
        }

        private void CalcWeightPath()
        {
            Queue<Waypoint> queueWaypoints = new Queue<Waypoint>();
            List<Waypoint> visitedWaypoints = new List<Waypoint>();
            queueWaypoints.Enqueue(_head);

            while (queueWaypoints.Count > 0)
            {
                Waypoint wp = queueWaypoints.Dequeue();
                visitedWaypoints.Add(wp);
                wp.Distance =
                    Vector3.Distance(wp.transform.position, _tail.transform.position);

                foreach (Waypoint waypoint in wp.waypoints)
                {
                    if (visitedWaypoints.Contains(waypoint) == false)
                        queueWaypoints.Enqueue(waypoint);
                }
            }
        }

        private void HandleMoveNpc()
        {


            Vector3 direction = _npcTarget.transform.position - npc.position;
            if (direction.magnitude < 0.1)
            {
                _npcTarget = _npcTarget.GetNextWaypoint();
            }

            npc.transform.position += direction.normalized * 4 * Time.deltaTime;
        }
    }
}

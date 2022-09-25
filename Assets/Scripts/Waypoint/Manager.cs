using UnityEngine;

namespace Waypoint
{
    public class Manager : MonoBehaviour
    {
        private Waypoint _head;
        private Waypoint _tail;
        private Waypoint _selected;

        public Settings settings;
        public static Manager Instance { get; private set; }

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
    }
}

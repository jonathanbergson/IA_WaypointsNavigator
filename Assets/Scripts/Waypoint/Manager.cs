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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _, 1000) == false)
                CreateStandardWaypoint(ray.origin.x, ray.origin.y);
        }

        private void CreateHeadWaypoint()
        {
            _head = Instantiate(settings.prefab, null);
            _head.SetType(Waypoint.Type.Head);
            _head.gameObject.transform.position = new Vector3(-5, -5);
        }

        private void CreateTailWaypoint()
        {
            _tail = Instantiate(settings.prefab, null);
            _tail.SetType(Waypoint.Type.Tail);
            _tail.gameObject.transform.position = new Vector3(5, 5);
        }

        private void CreateStandardWaypoint(float x, float y)
        {
            Waypoint waypoint = Instantiate(settings.prefab, null);
            waypoint.SetType(Waypoint.Type.Standart);
            waypoint.gameObject.transform.position = new Vector3(x, y);

            SetSelected(waypoint);
            waypoint.SetSelectedMaterial();
        }

        public void SetSelected(Waypoint selected)
        {
            if(_selected) _selected.ResetMaterial();
            _selected = selected;
        }
    }
}

using UnityEngine;

namespace WoW
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCam;
        [SerializeField] private LayerMask layerMask;

        private void Awake() => _mainCam = Camera.main;

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 10, layerMask)) return;
            if (!hit.transform.TryGetComponent(out IClickable clickable)) return;
            clickable.Click();
        }
    }
}

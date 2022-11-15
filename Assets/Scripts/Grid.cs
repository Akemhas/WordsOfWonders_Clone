using UnityEngine;
using UnityEngine.UI;

namespace WoW
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private Vector2Int gridSize;

        private void Awake()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            RectTransform rect = transform as RectTransform;
            Vector2 cellSizeDelta = (cellPrefab.transform as RectTransform).sizeDelta;
            rect.sizeDelta = new Vector2(gridSize.y * cellSizeDelta.x, gridSize.y * cellSizeDelta.y);
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = gridSize.x;

            for (int i = 0; i < gridSize.x * gridSize.y; i++)
            {
                Instantiate(cellPrefab, transform);
            }
        }

        private void DestroyGrid()
        {
            while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}

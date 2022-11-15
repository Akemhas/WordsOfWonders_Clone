using UnityEngine;
using UnityEngine.UI;

namespace WoW
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        // [SerializeField] private Vector2Int gridSize;

        public Cell[][] cells;

        [ContextMenu("Initialize")]
        public void InitializeGrid(Vector2Int gridSize, Word[] words)
        {
            gridSize.Clamp(new Vector2Int(5, 5), new Vector2Int(int.MaxValue, int.MaxValue));
            Vector2 cellSizeDelta = cellPrefab.rectTransform.sizeDelta;
            var maxSize = new Vector2(gridSize.y * cellSizeDelta.x, gridSize.y * cellSizeDelta.y);

            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = gridSize.x;

            gridLayoutGroup.cellSize = (rectTransform.sizeDelta / gridSize) - gridLayoutGroup.spacing;

            cells = new Cell[gridSize.x][];
            for (int i = 0; i < gridSize.x; i++)
            {
                cells[i] = new Cell[gridSize.y];
                for (int j = 0; j < gridSize.y; j++)
                {
                    Cell cell = Instantiate(cellPrefab, transform);
                    cell.cellPos = new Vector2Int(i, j);
                    cells[i][j] = cell;
                    cell.modelHolder.SetActive(false);
                }
            }

            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];

                for (int j = 0; j < word.Letters.Length; j++)
                {
                    if (word.H)
                    {
                        Cell cell = cells[word.SRow][j];
                        cell.modelHolder.SetActive(true);
                        cell.Letter = word.Letters[j];
                    }
                    else
                    {
                        Cell cell = cells[word.Letters.Length - 1 - j][word.SColumn];
                        cell.modelHolder.SetActive(true);
                        cell.Letter = word.Letters[j];
                    }
                }
            }
        }
    }
}

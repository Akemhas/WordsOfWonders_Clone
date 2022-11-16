using UnityEngine;
using UnityEngine.UI;

namespace WoW
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private Vector2Int gridSize;

        public Cell[][] cells;

        [ContextMenu("Initialize")]
        public void InitializeGrid(Vector2Int size, Word[] words)
        {
            gridSize = size;

            Vector2 cellSizeDelta = cellPrefab.rectTransform.sizeDelta;
            var maxSize = new Vector2(gridSize.y * cellSizeDelta.x, gridSize.y * cellSizeDelta.y);

            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = gridSize.x;

            gridLayoutGroup.cellSize = (rectTransform.sizeDelta / gridSize) - gridLayoutGroup.spacing;
            var cellSize = gridLayoutGroup.cellSize;
            if (gridLayoutGroup.cellSize.x > gridLayoutGroup.cellSize.y) cellSize.x = gridLayoutGroup.cellSize.y;
            else cellSize.y = gridLayoutGroup.cellSize.x;
            gridLayoutGroup.cellSize = cellSize;

            cells = new Cell[gridSize.x][];
            for (int i = 0; i < gridSize.y; i++)
            {
                cells[i] = new Cell[gridSize.x];
                for (int j = 0; j < gridSize.x; j++)
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

                word.cells = new Cell[word.Letters.Length];

                for (int j = 0; j < word.Letters.Length; j++)
                {
                    Cell cell;
                    if (word.H)
                    {
                        int index = word.SColumn + j;
                        cell = cells[word.SRow][index];
                        cell.letter = word.Letters[j];
                    }
                    else
                    {
                        int index = word.Letters.Length - 1 - j;
                        cell = cells[index + word.SRow][word.SColumn];
                        cell.letter = word.Letters[index];
                    }

                    cell.modelHolder.SetActive(true);
                    word.cells[j] = cell;
                }
            }
        }
    }
}

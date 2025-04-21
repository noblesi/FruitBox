using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public GameObject fruitPrefab;
    public Transform fruitParent;
    public LineRenderer lineRenderer;
    public TextMeshProUGUI scoreText;

    [SerializeField] private int rows = 9;
    [SerializeField] private int cols = 16;
    [SerializeField] private float spacing = 0.9f;

    private List<Fruit> selectedFruits = new List<Fruit>();
    private List<Fruit> allFruits = new List<Fruit>();
    private bool isDragging = false;

    private int score = 0;
    

    private void Start()
    {
        GenerateFruits();
        UpdateScoreUI();
        
        if(lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }

    private void Update()
    {
        HandleDragSelection();
    }

    private void HandleDragSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            ClearAllSelections();
            if(lineRenderer != null)
                lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Debug.Log($"선택 완료! 총 {selectedFruits.Count}개 선택됨.");

            if(selectedFruits.Count > 0)
            {
                int sum = 0;
                foreach(var fruit in selectedFruits)
                {
                    sum += fruit.number;
                }

                if (sum == 10)
                {
                    Debug.Log("합이 10입니다! 과일 제거!");
                    RemoveSelectedFruits();
                }
                else
                {
                    Debug.Log($"합이 {sum}입니다. 다시 시도하세요!");
                    ClearAllSelections();
                }
            }
            if(lineRenderer != null)
                lineRenderer.positionCount = 0;
        }

        if (isDragging)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if(hit != null)
            {
                Fruit fruit = hit.GetComponent<Fruit>();
                if(fruit != null && !selectedFruits.Contains(fruit))
                {
                    if (selectedFruits.Count == 0 || IsAdjacent(fruit, selectedFruits[selectedFruits.Count - 1]))
                    {
                        fruit.ToggleSelection();
                        selectedFruits.Add(fruit);
                        if(isDragging)
                            UpdateLineRenderer();
                    }
                }
            }
        }
    }

    private void GenerateFruits()
    {
        float offsetX = (cols - 1) * spacing / 2f;
        float offsetY = (rows - 1) * spacing / 2f;

        for (int y = 0; y < rows; y++)
        {
            for(int x = 0; x < cols; x++)
            {
                Vector2 position = new Vector2((x * spacing) - offsetX, -(y * spacing) + offsetY);
                GameObject fruitObj = Instantiate(fruitPrefab, position, Quaternion.identity, fruitParent);
                Fruit fruit = fruitObj.GetComponent<Fruit>();

                fruit.SetGridPosition(x, y);
                fruit.SetNumber(Random.Range(1, 10));

                allFruits.Add(fruit);
            }
        }
    }

    private bool IsAdjacent(Fruit a, Fruit b)
    {
        int dx = a.gridX - b.gridX;
        int dy = a.gridY - b.gridY;

        if(dx == 0 ||dy == 0 || Mathf.Abs(dx) == Mathf.Abs(dy))
        {
            return IsPathClear(a, b);
        }
        return false;
    }

    private bool IsPathClear(Fruit a, Fruit b)
    {
        int dx = System.Math.Sign(b.gridX - a.gridX);
        int dy = System.Math.Sign(b.gridY - a.gridY);

        int x = a.gridX + dx;
        int y = a.gridY + dy;

        while(x != b.gridX || y != b.gridY)
        {
            if (FindFruitAt(x, y) != null) 
                return false;
            x += dx;
            y += dy;
        }
        return true;
    }

    private Fruit FindFruitAt(int x, int y)
    {
        foreach(Fruit fruit in allFruits)
        {
            if(fruit != null && fruit.gridX == x && fruit.gridY == y) 
                return fruit;
        }
        return null;
    }

    private void ClearAllSelections()
    {
        foreach(Fruit fruit in selectedFruits)
        {
            if(fruit != null)
            {
                fruit.ResetSelection();
            }
        }
        selectedFruits.Clear();
        if(lineRenderer != null)        
            lineRenderer.positionCount = 0;
    }

    private void RemoveSelectedFruits()
    {
        score += selectedFruits.Count;
        UpdateScoreUI();

        foreach(Fruit fruit in selectedFruits)
        {
            if (fruit != null)
            {
                allFruits.Remove(fruit);
                Destroy(fruit.gameObject);
            }
        }
        selectedFruits.Clear();
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = selectedFruits.Count;
        for(int i = 0; i < selectedFruits.Count; i++)
        {
            lineRenderer.SetPosition(i, selectedFruits[i].transform.position);
        }
    }

    private void UpdateScoreUI()
    {
        if(scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }
}

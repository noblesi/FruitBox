using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public GameObject fruitPrefab;
    public Transform fruitParent;
    public LineRenderer lineRenderer;

    public int rows = 5;
    public int cols = 5;
    public float spacing = 1.2f;

    private List<Fruit> selectedFruits = new List<Fruit>();
    private bool isDragging = false;

    private void Start()
    {
        GenerateFruits();
        
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
                int randomNumber = Random.Range(1, 10);
                fruit.SetNumber(randomNumber);
            }
        }
    }

    private bool IsAdjacent(Fruit a, Fruit b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    private void ClearAllSelections()
    {
        foreach(Fruit fruit in selectedFruits)
        {
            fruit.ResetSelection();
        }
        selectedFruits.Clear();
    }

    private void RemoveSelectedFruits()
    {
        foreach(Fruit fruit in selectedFruits)
        {
            Destroy(fruit.gameObject);
        }
        selectedFruits.Clear();
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = selectedFruits.Count;
        for(int i = 0; i < selectedFruits.Count; i++)
        {
            lineRenderer.SetPosition(i, selectedFruits[i].transform.position);
        }
    }
}

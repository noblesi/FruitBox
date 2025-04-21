using TMPro;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int number;
    public TextMeshProUGUI numberText;

    public int gridX;
    public int gridY;

    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(numberText == null)
        {
            numberText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void SetNumber(int value)
    {
        number = value;
        if(numberText != null)
        {
            numberText.text = number.ToString();
        }
    }

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        spriteRenderer.color = isSelected ? Color.yellow : Color.white;
        transform.localScale = isSelected ? Vector3.one * 1.2f : Vector3.one;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void ResetSelection()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        transform.localScale = Vector3.one;
    }
}

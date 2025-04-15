using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public GameObject fruitPrefab;
    public Transform fruitParent;

    public int rows = 5;
    public int cols = 5;
    public float spacing = 1.2f;

    private void Start()
    {
        GenerateFruits();
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
                GameObject fruit = Instantiate(fruitPrefab, position, Quaternion.identity, fruitParent);

                int randomNumber = Random.Range(1, 10);
                fruit.GetComponent<Fruit>().SetNumber(randomNumber);
            }
        }
    }
}

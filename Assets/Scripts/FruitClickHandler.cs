using UnityEngine;

public class FruitClickHandler : MonoBehaviour
{
    private Fruit fruit;
    private FruitManager manager;

    public void Init(Fruit fruit, FruitManager manager)
    {
        this.fruit = fruit;
        this.manager = manager;
    }

    private void OnMouseDown()
    {
        fruit.ToggleSelection();
        manager.ToggleFruitSelection(fruit);
    }
}

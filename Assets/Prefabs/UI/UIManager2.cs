using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : MonoBehaviour
{
    public Camera slotCamera;
    private Vector3 zeroPos;
    private Vector3 nextPos;
    public int rowsAndColumns = 20;
    private int column = 1;
    private int row = 1;
    private float size;
    private float scale;
    public Dictionary<string, Rect> PrefabRect { get; private set; } = new();

    private void Awake()
    {
        size = slotCamera.orthographicSize / (rowsAndColumns / 2f);
        scale = size * 0.65f;
        nextPos = zeroPos = new(-size * (rowsAndColumns / 2f - 0.5f), size * (rowsAndColumns / 2f - 0.85f), 1f);
    }

    public Rect AddSlotRenderers(GameObject prefab)
    {
        if (PrefabRect.ContainsKey(prefab.name))
            return PrefabRect[prefab.name];

        if (row > rowsAndColumns)
            return Rect.zero;

        var item = Instantiate(prefab, slotCamera.transform);
        item.transform.localScale *= scale;
        item.transform.localPosition = nextPos;
        Rect rect = new((column - 1) / (float)rowsAndColumns, 1f - row / (float)rowsAndColumns,
            1f / rowsAndColumns, 1f / rowsAndColumns);
        PrefabRect.Add(prefab.name, rect);

        if (column < rowsAndColumns)
        {
            column++;
            nextPos += Vector3.right * size;
        }
        else if (row <= rowsAndColumns)
        {
            column = 1;
            row++;
            nextPos.x = zeroPos.x;
            nextPos += Vector3.down * size;
        }

        return rect;
    }
}

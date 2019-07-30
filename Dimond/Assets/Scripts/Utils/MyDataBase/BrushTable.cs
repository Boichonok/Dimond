using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BrushTable
{
    [System.Serializable]
    public class BrushProxy
    {
        public int brushId;
        public bool isFreeBrush;
        public BrushesType material;

        public BrushProxy(int brushId, bool isFreeBrush, BrushesType material)
        {
            this.brushId = brushId;
            this.isFreeBrush = isFreeBrush;
            this.material = material;
        }
    }

    public List<BrushProxy> rows;

    public bool InitTable(List<BrushProxy> rows)
    {
        this.rows = rows;
        return true;
    }

    public void AddRow(int id, bool isFreeBrush, BrushesType material)
    {
        var isExistRow = rows.Exists((BrushProxy obj) => obj.brushId == id);
        if (!isExistRow)
        {
            rows.Add(new BrushProxy(id, isFreeBrush, material));
        }
        else
        {
            var row = rows.Find((BrushProxy obj) => obj.brushId == id);
            row.brushId = id;
            row.isFreeBrush = isFreeBrush;
            row.material = material;
        }
    }

    public BrushProxy GetRowById(int id)
    {
        Debug.Log("Row: " + rows);
        var TableLength = rows.Count;
        for (int i = 0; i < TableLength; i++)
        {
            if (rows[i].brushId == id)
            {
                return rows[i];
            }
        }
        return null;
    }

    public int TableSize()
    {
        if (rows != null)
            return rows.Count;
        return 0;
    }
}

using Newtonsoft.Json;
using System;

public enum CellState { 
    None,
    Ground,
    Digged,
    Watered,
    Carrot,
    Corn,
    Rice
}

[Serializable]
public class CellData
{
    public int x { get; set; }
    public int y { get; set; }
    public CellState cellState { get; set; }
    public DateTime dateTime { get; set; }

    public CellData()
    { 
        this.x = 0;
        this.y = 0;
        this.cellState = CellState.None;
        dateTime = DateTime.Now;
    }

    public CellData(int x, int y, CellState state, DateTime dateTime)
    { 
        this.x = x;
        this.y = y;
        this.cellState = state;
        this.dateTime = dateTime;
    }
    public CellData(int x, int y, CellState state)
    {
        this.x = x;
        this.y = y;
        this.cellState = state;
        this.dateTime = DateTime.Now;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static CellState StringToCellState(string state)
    { 
        CellState cellState = (CellState)Enum.Parse(typeof(CellState), state);
        return cellState;
    }
}

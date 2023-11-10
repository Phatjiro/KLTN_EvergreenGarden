using System;

public enum CellState { 
    None,
    Ground,
    Digged,
    Watered,
    Carrot
}

[Serializable]
public class CellData
{
    public int x;
    public int y;
    public CellState cellState;

    public CellData()
    { 
        this.x = 0;
        this.y = 0;
        this.cellState = CellState.None;
    }

    public CellData(int x, int y, CellState state)
    { 
        this.x = x;
        this.y = y;
        this.cellState = state;
    }

    public override string ToString()
    {
        return $"{x},{y},{cellState}";
    }

    public static CellState StringToCellState(string state)
    { 
        CellState cellState = (CellState)Enum.Parse(typeof(CellState), state);
        return cellState;
    }
}

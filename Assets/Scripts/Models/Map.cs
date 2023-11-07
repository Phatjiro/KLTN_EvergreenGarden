using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Map
{
    public List<CellData> lstCell;

    public Map()
    {
        lstCell = new List<CellData>();
    }

    public Map(List<CellData> lstCell)
    {
        this.lstCell = lstCell;
    }

    public void AddCell(CellData cell)
    {
        if (lstCell == null)
        {
            lstCell = new List<CellData>();
        }
        for (int i = 0; i < lstCell.Count; i++)
        {
            if (lstCell[i].x == cell.x && lstCell[i].y == cell.y)
            {
                lstCell[i] = cell;
                return;
            }
        }
        lstCell.Add(cell);
        return;
    }

    public void ShowMap()
    {
        for (int i = 0; i < lstCell.Count; i++)
        {
            Debug.Log(lstCell[i].ToString());
        }
    }

    public void ExportFileTxt()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Data");
        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log("Create folder Data successful!");
            }
        }
        catch (System.Exception)
        {
            throw;
        }
        string filePath = Path.Combine(folderPath, "map.txt");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < lstCell.Count; i++)
            {
                writer.WriteLine(lstCell[i].ToString());
            }
        }
        Debug.Log("Export file successful to: " + filePath);
    }

    public void ReadFileTxt()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Data");
        string filePath = Path.Combine(folderPath, "map.txt");
        lstCell = new List<CellData>();
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                string[] lstLineElement;
                while ((line = reader.ReadLine()) != null)
                {
                    lstLineElement = line.Split(",");
                    int x = int.Parse(lstLineElement[0]);
                    int y = int.Parse(lstLineElement[1]);
                    CellState state = CellData.StringToCellState(lstLineElement[2]);
                    CellData data = new CellData(x, y, state);
                    lstCell.Add(data);
                }
            }
        }
        else
        {
            Debug.Log("map.txt does not exist!");
        }
    }

    public int GetLength()
    {
        return lstCell.Count;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

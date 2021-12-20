using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private string[,] gridArray;
    private TextMesh[,] debugTextArray;

    private List<string> gridStr;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new string[width, height];
        debugTextArray = new TextMesh[width, height];

        gridStr = new List<string>();

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x,y] = CodeJieun.Utility.CreateWorldText(gridArray[x, y], null, GetWorldPosition(x , y)+new Vector3(cellSize, cellSize)*.5f, 30, Color.white, TextAnchor.MiddleCenter);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        var cameraPos = Camera.main.transform.position;

        return new Vector3(x,y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        var cameraPos = Camera.main.transform.position;

        x = Mathf.FloorToInt((worldPosition - originPosition ).x/ cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition ).y / cellSize);
    }

    public void SetValue(int x, int y, string value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (value.Equals("Remove") && gridStr.Contains($"{x},{y}"))
            {
                gridArray[x, y] = default(string);
                gridStr.Remove($"{x},{y}");
                debugTextArray[x, y].text = gridArray[x, y];
            }
            else if(!value.Equals("Remove") && !gridStr.Contains($"{x},{y}"))
            {
                gridArray[x, y] = value;
                gridStr.Add($"{x},{y}");
                debugTextArray[x, y].text = gridArray[x, y];
            }
            else if (!value.Equals("Remove") && gridStr.Contains($"{x},{y}"))
            {
                gridStr.Remove($"{x},{y}");
                gridArray[x, y] = value;
                gridStr.Add($"{x},{y}");
                debugTextArray[x, y].text = gridArray[x, y];
            }
        }
    }

    public void SetValue(Vector3 worldPosition, string value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void FindValue(string value)
    {
        if (gridStr.Count == 0) return;

        Debug.Log(gridStr[0]);

        foreach (var element in gridStr)
        {
            var array = element.Split(",");
            int x = int.Parse(array[0]);
            int y = int.Parse(array[1]);

            if (value.Equals("All"))
            {
                debugTextArray[x, y].text = gridArray[x, y];
            }
            else
            {
                if (gridArray[x, y].Equals(value)) debugTextArray[x, y].text = value;
                else debugTextArray[x, y].text = null;
            }
        }
    }

    public void save()
    {
        Debug.Log("Save!");
        foreach (var element in gridStr)
            Debug.Log($"{element}");
    }

    public void load()
    {
        List<string> loadData = new List<string>();
        loadData.Add("7,6");
        loadData.Add("7,5");
        loadData.Add("3,5");

        List<string> loadNumber = new List<string>();
        loadNumber.Add("56");
        loadNumber.Add("56");
        loadNumber.Add("56");

        gridStr = loadData;

        int index = 0;
        foreach (var element in loadData)
        {
            var array = element.Split(",");

            int x = int.Parse(array[0]);
            int y = int.Parse(array[1]);

            gridArray[x, y] = loadNumber[index];
            index++;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        GameObject myLine = new GameObject("Line");
        myLine.transform.position = start;

        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Standard Unlit"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }


}


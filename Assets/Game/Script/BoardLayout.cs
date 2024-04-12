using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayout : MonoBehaviour
{

    public LayoutRow[] allRows; 

    public Leaf[,] GetLayout()
    {
        Leaf[,] theLayout = new Leaf[allRows[0].leafsInRow.Length, allRows.Length];

        for (int y = 0; y < allRows.Length; y++)
        {
            for (int x = 0; x < allRows[y].leafsInRow.Length; x++)
            {
                if (x < theLayout.GetLength(0))
                {
                    if (allRows[y].leafsInRow[x] != null)
                    {
                        theLayout[x, allRows.Length - 1 - y] = allRows[y].leafsInRow[x];
                    }
                }
            }
        }


        return theLayout;
    }

}

[System.Serializable]
public class LayoutRow 
{
    public Leaf[] leafsInRow;

}
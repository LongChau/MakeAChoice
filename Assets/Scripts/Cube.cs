//#define USE_RANDOM_COLOR
#define PAINT_THE_FRONT

using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

    public Cell[] cells;

    private Cell[,,] matrix;

    public void Init()
    {
        //cells = new Transform[transform.childCount];
        matrix = new Cell[3, 3, 3];
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i, j, k] = cells[count];
                    matrix[i, j, k].Init(count, i, j, k);
                    count++;
                }
            }
        }

#if USE_RANDOM_COLOR
        // Randomize color
        for (int i = 0; i < cells.Length; i++)
        {
            //cells[i] = transform.GetChild(i);
            cells[i].GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
#endif // USE_RANDOM_COLOR

#if PAINT_THE_FRONT
        PainTheFront();
#endif // PAINT_THE_FRONT
    }

    // Use this for initialization
    void Start () {
        Init();
	}

    //// Update is called once per frame
    //void Update () {

    //}

    public void UpdateRelativeLocations()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    matrix[i, j, k].SetRelativeLocation(i, j, k);
                }
            }
        }
    }

#if PAINT_THE_FRONT
    public void PainTheFront()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                matrix[i, 2, k].GetComponent<Renderer>().material.color = Color.red;

                for (int j = 0; j < 2; j++)
                {
                    matrix[i, j, k].GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }
#endif // PAINT_THE_FRONT

    public void RotateRow(int row, RotateDirection direction)
    {
        switch (direction)
        {
            case RotateDirection.ROTATE_LEFT:
            {
                Cell temp = matrix[row, 0, 0];
                matrix[row, 0, 0] = matrix[row, 2, 0];
                matrix[row, 2, 0] = matrix[row, 2, 2];
                matrix[row, 2, 2] = matrix[row, 0, 2];
                matrix[row, 0, 2] = temp;

                temp = matrix[row, 0, 1];
                matrix[row, 0, 1] = matrix[row, 1, 0];
                matrix[row, 1, 0] = matrix[row, 2, 1];
                matrix[row, 2, 1] = matrix[row, 1, 2];
                matrix[row, 1, 2] = temp;
                break;
            }

            case RotateDirection.ROTATE_RIGHT:
            {
                Cell temp = matrix[row, 0, 0];
                matrix[row, 0, 0] = matrix[row, 0, 2];
                matrix[row, 0, 2] = matrix[row, 2, 2];
                matrix[row, 2, 2] = matrix[row, 2, 0];
                matrix[row, 2, 0] = temp;

                temp = matrix[row, 0, 1];
                matrix[row, 0, 1] = matrix[row, 1, 2];
                matrix[row, 1, 2] = matrix[row, 2, 1];
                matrix[row, 2, 1] = matrix[row, 1, 0];
                matrix[row, 1, 0] = temp;
                break;
            }
        }

#if PAINT_THE_FRONT
        PainTheFront();
#endif // PAINT_THE_FRONT
    }

    public void RotateCol(int col, RotateDirection direction)
    {
        switch (direction)
        {
            case RotateDirection.ROTATE_DOWN:
                {
                    Cell temp = matrix[0, 0, col];
                    matrix[0, 0, col] = matrix[2, 0, col];
                    matrix[2, 0, col] = matrix[2, 2, col];
                    matrix[2, 2, col] = matrix[0, 2, col];
                    matrix[0, 2, col] = temp;

                    temp = matrix[0, 1, col];
                    matrix[0, 1, col] = matrix[1, 0, col];
                    matrix[1, 0, col] = matrix[2, 1, col];
                    matrix[2, 1, col] = matrix[1, 2, col];
                    matrix[1, 2, col] = temp;
                    break;
                }

            case RotateDirection.ROTATE_UP:
                {
                    Cell temp = matrix[0, 0, col];
                    matrix[0, 0, col] = matrix[0, 2, col];
                    matrix[0, 2, col] = matrix[2, 2, col];
                    matrix[2, 2, col] = matrix[2, 0, col];
                    matrix[2, 0, col] = temp;

                    temp = matrix[0, 1, col];
                    matrix[0, 1, col] = matrix[1, 2, col];
                    matrix[1, 2, col] = matrix[2, 1, col];
                    matrix[2, 1, col] = matrix[1, 0, col];
                    matrix[1, 0, col] = temp;
                    break;
                }
        }

#if PAINT_THE_FRONT
        PainTheFront();
#endif // PAINT_THE_FRONT
    }

    public void RotateMatrix(RotateDirection direction)
    {
        switch (direction)
        { 
            case RotateDirection.ROTATE_LEFT:
            case RotateDirection.ROTATE_RIGHT:
            {
                for (int i = 0; i < 3; i++)
                {
                    RotateRow(i, direction);
                }
                break;
            }

            case RotateDirection.ROTATE_UP:
            case RotateDirection.ROTATE_DOWN:
            {
                for (int k = 0; k < 3; k++)
                {
                    RotateCol(k, direction);
                }
                break;
            }
        }
    }
}

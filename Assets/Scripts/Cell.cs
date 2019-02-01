using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

    int _index;
    int _row;
    int _col;
    int _layer;

	//// Use this for initialization
	//void Start () {
	
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    public void Init(int index, int row, int layer, int col)
    {
        _index = index;
        SetRelativeLocation(row, layer, col);
    }

    public void SetRelativeLocation(int row, int layer, int col)
    {
        _row = row;
        _layer = layer;
        _col = col;
    }
}

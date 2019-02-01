using UnityEngine;
using System.Collections;

public class Global {

    public const int LAYER_CUBE = 8;
    public const int MASK_CUBE = 1 << LAYER_CUBE;
}

public enum RotateDirection
{
    NO_ROTATION = -1,
    ROTATE_LEFT = 0,
    ROTATE_RIGHT = 1,
    ROTATE_UP = 2,
    ROTATE_DOWN = 3,
}
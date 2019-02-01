using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour {

    public MapController mapController;
    public CubeController rubikController;

	// Use this for initialization
	void Start () {
        rubikController.Deactivate();
        mapController.Activate();
	}
	
	//// Update is called once per frame
	//void Update () {
	
	//}
}

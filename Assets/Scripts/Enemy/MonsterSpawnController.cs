using UnityEngine;
using System.Collections;

public class MonsterSpawnController : MonoBehaviour
{

    public GameObject monster;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", 5, 5);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void Spawn()
    {
        //do spawn stuff...
        Instantiate(monster);
    }
}

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    public Terrain terrain;

    public int hp;

    private float targetDistance;
    private bool isDead;

    //// Use this for initialization
    //void Start () {

        
    //}
	
	// Update is called once per frame
	void Update ()
	{
	    if (isDead)
	        return;
        Vector3 targetPos = target.position;

        targetDistance = Vector3.Distance(transform.position, targetPos);
	    if (targetDistance > agent.stoppingDistance)
	    {
	        agent.SetDestination(target.position);
	    }
	}
}

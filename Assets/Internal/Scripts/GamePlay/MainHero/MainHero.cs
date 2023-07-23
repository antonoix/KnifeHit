using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainHero : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        navAgent.SetDestination(new Vector3(1, 0, 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

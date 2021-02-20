using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float chaseRange;
    [SerializeField] private int FOV;
    [SerializeField] private int viewDistance;
    [SerializeField] private Transform[] playerTransforms;
    [SerializeField] private Transform[] patrolLocs;

    private NavMeshAgent agent;
    private Selector topNode;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ConstructBehaviourTree();
    }

    private void Update()
    {
        topNode.Evaluate();
    }

    private void ConstructBehaviourTree()
    {
        InRangeNode inRange = new InRangeNode(chaseRange, playerTransforms, transform);
        IsVisibleNode isVisible = new IsVisibleNode(FOV, viewDistance, playerTransforms, transform);
        WonderNode wonderNode = new WonderNode(patrolLocs, agent);
        ChaseNode chaseNode = new ChaseNode(playerTransforms, agent);

       
        Sequence noEnemySequence = new Sequence(new List<Node> { inRange, isVisible});
        Inverter noEnemyInverter = new Inverter(noEnemySequence);
        Sequence chaseSequence = new Sequence(new List<Node> { inRange, isVisible, chaseNode });
        Sequence wonderSequence = new Sequence(new List<Node> { noEnemyInverter, wonderNode});

        topNode = new Selector(new List<Node> { wonderSequence, chaseSequence });
    }
}

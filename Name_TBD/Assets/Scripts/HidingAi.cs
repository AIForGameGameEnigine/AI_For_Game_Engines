using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HidingAi : MonoBehaviour
{
    [SerializeField] private float chaseRange;
    [SerializeField] private int FOV;
    [SerializeField] private int viewDistance;
    [SerializeField] private Transform[] playerTransform;

    private NavMeshAgent agent;
    private Selector topNode;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ConstructBehaviourTree();
    }

    // Update is called once per frame
    void Update()
    {
        topNode.Evaluate();
    }

    private void ConstructBehaviourTree()
    {
        GameObject[] covers = GameObject.FindGameObjectsWithTag("Cover");
        InRangeNode inRange = new InRangeNode(chaseRange, playerTransform, transform);
        IsVisibleNode isVisible = new IsVisibleNode(FOV, viewDistance, playerTransform, transform);
        Inverter invertIsVisible = new Inverter(isVisible);
        HidingNode hindingNode = new HidingNode(covers, transform, playerTransform, agent);
        RunNode runNode = new RunNode(covers, playerTransform, agent);

        Sequence hideSequence = new Sequence(new List<Node> { inRange, invertIsVisible, hindingNode });
        Sequence runSequence = new Sequence(new List<Node> { inRange, runNode });

        topNode = new Selector(new List<Node> { hideSequence, runSequence });
    }
}

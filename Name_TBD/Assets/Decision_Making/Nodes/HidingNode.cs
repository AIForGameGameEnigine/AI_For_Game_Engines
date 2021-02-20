﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HidingNode : Node
{
    

    struct BestCover
    {

        public GameObject cover;
        public float dist_origin;
        public float dist_target;
    }


    private GameObject[] covers;
    private Transform origin;
    private Transform[] target;
    private NavMeshAgent agent;

    public HidingNode(GameObject[] covers, Transform origin, Transform[] target, NavMeshAgent agent)
    {
        this.covers = covers;
        this.origin = origin;
        this.target = target;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        BestCover cover = FindBestCover();

        Transform coverTransfom = cover.cover.transform;

        float distance = Vector3.Distance(coverTransfom.position, agent.transform.position);

        if (distance > 0.2)
        {
            agent.isStopped = false;
            agent.SetDestination(coverTransfom.position);
            _nodeState = NodeState.RUNNING;
            return _nodeState;
        }
        else
        {
            agent.isStopped = true;
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }
    }

    private BestCover FindBestCover()
    {
        BestCover bestCover = new BestCover();

        foreach (var tar in target)
        {
            bestCover.cover = covers[0];
            Transform coverTransfom = covers[0].transform;
            bestCover.dist_origin = Vector3.Distance(origin.position, coverTransfom.position);
            bestCover.dist_target = Vector3.Distance(tar.position, coverTransfom.position);

            for (int i = 1; i < covers.Length; i++)
            {
                Transform newCoverTransfom = covers[i].transform;
                float dist_origin = Vector3.Distance(origin.position, newCoverTransfom.position);
                float dist_target = Vector3.Distance(tar.position, newCoverTransfom.position);

                if (dist_target > bestCover.dist_target)
                {
                    bestCover.cover = covers[i];

                    bestCover.dist_origin = dist_origin;
                    bestCover.dist_target = dist_target;
                }

            }
        }

        return bestCover;
    }

}

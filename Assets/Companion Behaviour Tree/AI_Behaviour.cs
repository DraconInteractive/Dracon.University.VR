using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AI_Behaviour : MonoBehaviour
{
    //Behaviour tree integration for AI
    public RootNode rootNode;
    Companion companion;
    public string current;

    public float followDist;

    public bool disabled;
    private void Start()
    {
        companion = GetComponent<Companion>();
        StartCoroutine(TimedEvaluation());
    } 

    IEnumerator TimedEvaluation ()
    {
        while (true)
        {
            if (!disabled)
            {
                EvaluateTree();
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }

    public void EvaluateTree()
    {
        if (companion == null)
        {
            companion = GetComponent<Companion>();
        }
        BehaviourNode currentNode = rootNode.childNode;
        while (currentNode is ControlNode)
        {
            ControlNode c = currentNode as ControlNode;

            MethodInfo method = this.GetType().GetMethod(c.methodName);
            bool result = (bool)method.Invoke(this, null);

            if (result)
            {
                currentNode = c.positive;
            }
            else
            {
                currentNode = c.negative;
            }
        }
        if (currentNode is ExecutionNode)
        {
            ExecutionNode e = currentNode as ExecutionNode;

            if (e.methodName != current)
            {
                Invoke(e.methodName, 0);
                current = e.methodName;
            }
        }
    }

    public bool WithinFollowDist ()
    {
        Transform cam = Camera.main.transform;
        float dist = Vector3.Distance(transform.position, cam.position);
        if (dist < followDist)
        {
            return true;
        }
        return false;
    }

    public bool CheckStay ()
    {
        return companion.stay;
    }

    public bool CheckStare ()
    {
        return (companion.stareCounter > 0.75f);
    }
    public void MoveToPlayer ()
    {
        companion.ClearAction();
        companion.actionRoutine = companion.StartCoroutine(companion.MoveToPlayer());
    }

    public void Attend ()
    {
        companion.ClearAction();
        companion.actionRoutine = companion.StartCoroutine(companion.Attend());
    }

    public void Wait ()
    {
        companion.ClearAction();
    }
}


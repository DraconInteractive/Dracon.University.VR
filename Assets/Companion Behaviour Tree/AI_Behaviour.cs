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

    public string current;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EvaluateTree();
        }
    }

    public void EvaluateTree()
    {

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

    
}


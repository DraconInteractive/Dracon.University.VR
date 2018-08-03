using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node", menuName = "Behaiour/Control Node")]

public class ControlNode : BehaviourNode {

    public string methodName;
    public BehaviourNode positive, negative;
}

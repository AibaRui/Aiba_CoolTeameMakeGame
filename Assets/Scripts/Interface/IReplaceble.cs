using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReplaceble
{

    void EnterReplaceZone(ReplceType replceType);

    void ExitReplaceZone(ReplceType replceType);
}
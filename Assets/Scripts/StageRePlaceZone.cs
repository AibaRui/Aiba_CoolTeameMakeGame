using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRePlaceZone : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private ReplceType _replceType = ReplceType.Up;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            other.gameObject.GetComponent<IReplaceble>().EnterReplaceZone(_replceType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<IReplaceble>().ExitReplaceZone(_replceType);
        }
    }
}

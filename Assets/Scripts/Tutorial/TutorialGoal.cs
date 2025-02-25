using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    [Header("チュートリアルのタイプ")]
    [SerializeField] private TutorialType _type;


    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IEnterTutorialGoalble>(out IEnterTutorialGoalble t);

        if (t == null)
        {
            Debug.Log(other.gameObject.name + ":Null:" + t);
        }
        else
        {
            t.EnterZone(_type);
        }

        Destroy(gameObject);
    }

}


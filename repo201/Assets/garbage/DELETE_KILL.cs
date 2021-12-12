using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_KILL : MonoBehaviour
{
    public void KillTRANSITIONOBJ()
    {
        Destroy(GameObject.Find("TransitionObject"));
    }
}

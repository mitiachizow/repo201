using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainer : MonoBehaviour
{
    [SerializeField]
    private ConstructionPrefab construction;

    public ConstructionPrefab Construction
    {
        get { return construction; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDefinition : ScriptableObject
{
    [SerializeField]
    internal GameObject _pickUpPrefab;

    [SerializeField]
    internal Brick _brickPrefab;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    [SerializeField]
    private GameObject _trunkRenderer;

    [SerializeField]
    private Transform _segmentRoot;

    private Brick _grabbedBrick;
    private MainCharacterController _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<MainCharacterController>();
        StopTrunk();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_controller == collision.gameObject.GetComponent<MainCharacterController>())
        {
            StartTrunk(_controller.CurrentItem);
        }
    }


    private void StartTrunk(ItemConfig itemConfig)
    {
        _trunkRenderer.SetActive(true);

        if (itemConfig != null)
            _grabbedBrick = Instantiate(itemConfig._brickPrefab, _segmentRoot);

        // character
    }

    public void StopTrunk()
    {
        _trunkRenderer.SetActive(false);

        if (_grabbedBrick == null) { return; }

        if(_grabbedBrick._inGrid)
        {
            _controller.OnDrop();
        }


    }
}

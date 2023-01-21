using UnityEngine;

public class Script : MonoBehaviour
{
    public GameObject _gameobject; // public , we can see this field in Inspector!!

    private void Start() // on start game
    {
        _gameobject.SetActive(false); // disabled
    }

    private void Update() // why in update???
    {
        if(Input.GetKeyDown(KeyCode.Space)) // detect space button on keyboard when it pressed
        {
            _gameobject.SetActive(true); // enable
            GetComponent<MeshRenderer>().enabled = false; // disable meshRenderer component
        }

        float x = Input.GetAxis("Horizontal"); // -1 to 1 // press a => -1// press d => 1// nothing => 0
        transform.position += new Vector3(x, 0f, 0f) * Time.fixedDeltaTime; // each frame // smooth transition
        // deltaTime => time between frames
    }

}

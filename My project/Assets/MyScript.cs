using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour // Unity => 
{
    private BoxCollider colllider = null; // field, empty

    //to initalize components
    private void Awake() // => before Start
    {
        colllider = GetComponent<BoxCollider>(); // get BoxCollider component
        print(colllider.name); // print the name of parent;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverboardAttributes : MonoBehaviour
{
    [SerializeField]
    public char speed; // Speed attribute

    [SerializeField]
    public char boost; // Boost attribute

    [SerializeField]
    public char turning; // Turning attribute

    // Properties to access the attributes
    public char Speed { get { return speed; } }
    public char Boost { get { return boost; } }
    public char Turning { get { return turning; } }
}


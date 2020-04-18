using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayDirectionChanger : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    private Heading _firstHeading;

    [SerializeField]
    private Heading _secondHeading;
    #pragma warning restore 0649

    public Heading GetOtherDirection(Heading currentDirection)
    {
        if (currentDirection.Opposite() == _firstHeading)
        {
            return _secondHeading;
        }
        else if (currentDirection.Opposite() == _secondHeading)
        {
            return _firstHeading;
        }

        Debug.LogError("Invalid direction!" + "\n" + currentDirection + "\n" + currentDirection.Opposite());
        return Heading.Max;
    }
}

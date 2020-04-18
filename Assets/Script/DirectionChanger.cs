// -----------------------------------------------
// Filename: DirectionChanger.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    public bool LeftTurn
    {
        get { return _leftTurn; }
        set
        {
            _leftTurn = value;
            UpdateSprite();
            UpdateDirection();
        }
    }

    public Heading NewDirection
    {
        get { return _heading; }
        set
        {
            _heading = value;
            UpdateDirection();
        }
    }

    public Heading FromDirection
    {
        get
        {
            if (_leftTurn)
            {
                switch (_heading)
                {
                    case Heading.Up:
                        return Heading.Right;
                    case Heading.Down:
                        return Heading.Left;
                    case Heading.Left:
                        return Heading.Up;
                    case Heading.Right:
                        return Heading.Down;
                }
            }
            else
            {
                switch (_heading)
                {
                    case Heading.Up:
                        return Heading.Left;
                    case Heading.Down:
                        return Heading.Right;
                    case Heading.Left:
                        return Heading.Down;
                    case Heading.Right:
                        return Heading.Up;
                }
            }

            Debug.LogError("Invalid FromDirection");
            return Heading.Max;
        }
    }

    [SerializeField]
    private Heading _heading;

    [SerializeField]
    private bool _leftTurn;

    #pragma warning disable 0649
    [SerializeField]
    private Sprite _leftTurnSprite;

    [SerializeField]
    private Sprite _rightTurnSprite;
    #pragma warning restore 0649

    private void Start()
    {
        UpdateSprite();
        UpdateDirection();
    }

    private void UpdateSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (_leftTurn)
        {
            spriteRenderer.sprite = _leftTurnSprite;
        }
        else
        {
            spriteRenderer.sprite = _rightTurnSprite;
        }
    }

    private void UpdateDirection()
    {
        transform.rotation = _heading.ToQuaternion();
    }
}

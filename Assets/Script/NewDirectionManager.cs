// -----------------------------------------------
// Filename: NewDirectionManager.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using UnityEngine;

public class NewDirectionManager : MonoBehaviour
{
    [System.Serializable]
    public struct LegalPosition
    {
        public int x;
        public int y;
        public DirectionChanger currentDirectionChanger;
    }

    public LegalPosition[] _legalPositions;
}

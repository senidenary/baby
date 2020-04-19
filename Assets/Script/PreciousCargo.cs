// -----------------------------------------------
// Filename: PreciousCargo.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciousCargo : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    private ScoreManager _scoreManager;
    #pragma warning restore 0649

    public void Die(string killer)
    {
        _scoreManager.GameOver(killer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Car" || other.tag == "Fire")
        {
            Die(other.tag);
        }
    }
}

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
    private const float KillDist = 0.8f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Hazard")
        {
            float dist = Vector3.Distance(other.transform.position, transform.position);
            if (dist < KillDist)
            {
                Debug.Log("Hazard");
            }
        }
    }
}

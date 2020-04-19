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

    [SerializeField]
    private AudioClip _hitCarAudioClip;

    [SerializeField]
    private AudioClip _onFireAudioClip;

    [SerializeField]
    private AudioClip _babyLeftAudioClip;
    #pragma warning restore 0649

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Die(string killer)
    {
        if (!_scoreManager.GameActive)
        {
            return;
        }

        if (killer == "Car")
        {
            _audioSource.clip = _hitCarAudioClip;
            _audioSource.Play();
        }
        else if (killer == "Fire")
        {
            _audioSource.clip = _onFireAudioClip;
            _audioSource.Play();
        }
        else if (killer == "Border")
        {
            _audioSource.clip = _babyLeftAudioClip;
            _audioSource.Play();
        }

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

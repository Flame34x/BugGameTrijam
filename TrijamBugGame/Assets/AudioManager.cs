using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] musicTracks;
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    [SerializeField] private AudioSource musicFX;

    private void Start()
    {
        currentTrackIndex = Random.Range(0, musicTracks.Length);
        PlayTrack(currentTrackIndex);
    }

    private void Update()
    {
        // Check if the current track has finished playing
        if (!musicFX.isPlaying)
        {
            // Move to the next track in the array
            currentTrackIndex++;
            if (currentTrackIndex >= musicTracks.Length)
            {
                // If we've reached the end, loop back to the beginning
                currentTrackIndex = 0;
            }

            // Play the next track
            PlayTrack(currentTrackIndex);
        }
    }

    private void PlayTrack(int trackIndex)
    {
        // Make sure the track index is valid
        if (trackIndex >= 0 && trackIndex < musicTracks.Length)
        {
            musicFX.clip = musicTracks[trackIndex];
            musicFX.Play();
        }
    }
}

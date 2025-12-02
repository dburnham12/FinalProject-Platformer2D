using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NamedAudioClip
{
    public string key;        // Identifier string (e.g. "JUMP", "HURT", "COIN")
    public AudioClip clip;    // Actual audio file, assigned in the Inspector
}

public class SoundManager : MonoBehaviour
{
    // Singleton instance so other scripts can call SoundManager.Instance.PlaySFX(...)
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;   // Single AudioSource used to play all sound effects

    // List of clips you can assign in the Inspector (key + clip pairs)
    public List<NamedAudioClip> audioClips;

    // Internal dictionary for fast lookup by key
    private Dictionary<string, AudioClip> clipMap;

    private void Awake()
    {
        // --- Singleton setup ---
        // If another SoundManager already exists, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Otherwise, set this as the active instance
        Instance = this;

        // Keep SoundManager alive when changing scenes
        DontDestroyOnLoad(gameObject);

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // --- Build dictionary from Inspector list ---
        clipMap = new Dictionary<string, AudioClip>();
        foreach (var entry in audioClips)
        {
            // Only add if the key is unique
            if (!clipMap.ContainsKey(entry.key))
            {
                clipMap.Add(entry.key, entry.clip);
            }
        }
    }

    // Public method to play a sound effect by key
    // Example: SoundManager.Instance.PlaySFX("JUMP");
    public void PlaySFX(string key, float volume = 1f)
    {
        // Check if the key exists and has a valid clip
        if (clipMap.ContainsKey(key) && clipMap[key] != null)
        {
            // Play the clip once at the given volume (allows overlapping sounds)
            audioSource.PlayOneShot(clipMap[key], volume);
        }
        else
        {
            // Warn if the key was not found in the dictionary
            Debug.LogWarning($"SoundManager: No clip mapped for key '{key}'");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public const int MAX_MUSIC_PLAYER = 1;

    private void Awake() {
        int numOfMusicPlayer = FindObjectsOfType<MusicPlayer>().Length;
        if(numOfMusicPlayer > MAX_MUSIC_PLAYER) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }

    }
}

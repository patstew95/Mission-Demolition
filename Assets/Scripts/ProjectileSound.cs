using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileSound : MonoBehaviour {
    private AudioSource aud;
    private Rigidbody   rigid;
    private bool        launched = false;

    void Awake() {
        aud   = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        aud.playOnAwake = false;
        aud.loop        = false;          
    }

    void Update() {
        if (!launched && !rigid.isKinematic) {
            launched = true;
            aud.Play();
        }
    }
}

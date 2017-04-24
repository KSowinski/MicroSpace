using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip Laser1;
    public AudioClip Laser2;
    public AudioClip Laser3;

    public AudioClip Damage1;
    public AudioClip Damage2;

    public AudioClip Explosion1;
    public AudioClip Explosion2;

    public AudioClip Resource1;
    public AudioClip Buy1;


    private AudioSource _audio;

    public static SoundManager Inst()
    {
        return FindObjectOfType<SoundManager>();
    }

    public void Start()
    {
        _audio = transform.GetComponent<AudioSource>();
    }

    public void Laser()
    {
        var rnd = Random.Range(1, 4);
        if (rnd == 1) _audio.clip = Laser1;
        if (rnd == 2) _audio.clip = Laser2;
        if (rnd == 3) _audio.clip = Laser3;
        _audio.Play();
    }

    public void Damage()
    {
        var rnd = Random.Range(1, 3);
        if (rnd == 1) _audio.clip = Damage1;
        if (rnd == 2) _audio.clip = Damage2;
        _audio.Play();
    }

    public void Explosion()
    {
        var rnd = Random.Range(1, 3);
        if (rnd == 1) _audio.clip = Explosion1;
        if (rnd == 2) _audio.clip = Explosion2;
        _audio.Play();
    }

    public void Resource()
    {
        _audio.clip = Resource1;
        _audio.Play();
    }
    
    public void Buy()
    {
        _audio.clip = Buy1;
        _audio.Play();
    }
}

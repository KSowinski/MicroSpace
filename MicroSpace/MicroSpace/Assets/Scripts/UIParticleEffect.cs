using UnityEngine;

public class UIParticleEffect: MonoBehaviour
{
    public GameObject EffectPrefab;
    public void Show()
    {
        Instantiate(EffectPrefab, transform.position, Quaternion.identity);
        SoundManager.Inst().Resource();
    }
}
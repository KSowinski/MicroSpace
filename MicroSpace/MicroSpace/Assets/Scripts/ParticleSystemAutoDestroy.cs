using System.Collections;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    public float Duration;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }
}

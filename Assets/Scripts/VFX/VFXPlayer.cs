using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _vfx;

    public void Play()
    {
        for (int i = 0; i < _vfx.Length; i++)
        {
            _vfx[i].Play();
        }
    }
}

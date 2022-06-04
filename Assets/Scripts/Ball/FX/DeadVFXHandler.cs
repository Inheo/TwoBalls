using UnityEngine;

[RequireComponent(typeof(Ball))]
public class DeadVFXHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfxPrefab;

    private Ball _ball;

    private void Awake()
    {
        _ball = GetComponent<Ball>();
    }

    private void Start()
    {
        _ball.OnFail += Play;
    }

    private void OnDestroy()
    {
        _ball.OnFail -= Play;
    }

    private void Play()
    {
        Instantiate(_vfxPrefab, transform.position, Quaternion.identity);
    }
}

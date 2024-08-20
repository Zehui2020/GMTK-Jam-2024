using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealthLight : MonoBehaviour
{
    private Light2D _source;

    [SerializeField] private float maximumDim;
    [SerializeField] private float maximumBoost;
    [SerializeField] private float speed;
    [SerializeField] private float strength;
    [SerializeField] private float blowIntensity;

    [SerializeField] private ParticleSystem flickerSparkPS;
    [SerializeField] private ParticleSystem blowSparkPS;

    [SerializeField] private int blowThreshold;

    private bool noFlicker = false;
    private float initialIntensity;

    private Coroutine sparksRoutine;

    public event System.Action<HealthLight> OnBlowEvent;

    public void SetThreshold(int threshold)
    {
        blowThreshold = threshold;
    }

    public void Reset()
    {
        maximumDim = 0.2f;
        maximumBoost = 0.2f;
        speed = 0.1f;
        strength = 250;
    }

    public void Start()
    {
        _source = GetComponent<Light2D>();
        initialIntensity = _source.intensity;
    }

    public void StartFlicker()
    {
        if (sparksRoutine != null)
            return;

        AudioManager.Instance.Play("LightFlicker");
        StartCoroutine(Flicker());
        sparksRoutine = StartCoroutine(SparksRoutine());
    }

    private IEnumerator Flicker()
    {
        yield return new WaitForSeconds(2f);

        while (!noFlicker)
        {
            _source.intensity = Mathf.Lerp(_source.intensity, Random.Range(initialIntensity - maximumDim, initialIntensity + maximumBoost), strength * Time.deltaTime);
            yield return new WaitForSeconds(speed);
        }
    }

    public void BlowLight()
    {
        if (noFlicker)
            return;

        noFlicker = true;
        if (sparksRoutine != null)
            StopCoroutine(sparksRoutine);

        AudioManager.Instance.PlayOneShot("LightExplode");
        OnBlowEvent?.Invoke(this);
        StartCoroutine(BlowRoutine());
    }

    private IEnumerator SparksRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            int randNum = Random.Range(5, 15);
            flickerSparkPS.Play();
            yield return new WaitForSeconds(randNum);
        }
    }

    private IEnumerator BlowRoutine()
    {
        float timer = 0.6f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _source.intensity = Mathf.Lerp(_source.intensity, blowIntensity, 5f * Time.deltaTime);
            yield return null;
        }

        timer = 0.6f;
        blowSparkPS.Play();

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _source.intensity = Mathf.Lerp(_source.intensity, 0, 10f * Time.deltaTime);
            yield return null;
        }

        _source.enabled = false;
    }

    public void UpdateLight(int currentHealth)
    {
        if (currentHealth <= blowThreshold)
            BlowLight();
    }

    private void OnDisable()
    {
        OnBlowEvent = null;
    }
}
using UnityEngine;
using UnityEngine.VFX;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private float killTimer = 5;
    public VisualEffect vfx;
    private AudioSource audioSource;
    public AudioClip Sound;
    private VFXEventAttribute eventAttribute;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(BuildingSystem.current.effect.transform);

        vfx = transform.GetComponent<VisualEffect>();
        audioSource = transform.GetComponent<AudioSource>();

        if (vfx != null)
        {
        eventAttribute = vfx.CreateVFXEventAttribute();
        vfx.SendEvent("OnPlay", eventAttribute);

        }

        if(audioSource != null)
        {
            audioSource.PlayOneShot(Sound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        killTimer -= Time.deltaTime;
        if(killTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    // public AudioClip footstepClip;
    // public AudioClip jumpClip;
    public AudioClip shootClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

   /* public void PlayFootstep()
    {
        if (footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }
    }

    public void PlayJump()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }
*/
    

    public void PlayShoot()
    {
        if (shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }
}

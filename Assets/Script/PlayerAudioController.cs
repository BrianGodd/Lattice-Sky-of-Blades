using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    private AudioSource m_AudioSource;
    [SerializeField] private float m_StepInterval = 5f;
    [SerializeField] private float m_RunstepLenghten = 0.7f;
    private float m_StepCycle;
    private float m_NextStep;
    public float stepSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerCharacter.Instance != null)
        {
            PlayerCharacter.Instance.OnJump += PlayJumpSound;
            PlayerCharacter.Instance.OnLand += PlayLandingSound;
        }   
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProgressStepCycle(stepSpeed);
    }

    private void ProgressStepCycle(float speed)
    {
        if (PlayerCharacter.Instance.CurrentVelocity.sqrMagnitude > 0 && PlayerCharacter.Instance.IsWalking)
        {
            m_StepCycle += (PlayerCharacter.Instance.CurrentVelocity.magnitude + (speed*(PlayerCharacter.Instance.IsWalking ? 1f : m_RunstepLenghten)))*
                            Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }

    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }

    private void PlayFootStepAudio()
    {
        if (!PlayerCharacter.Instance.IsGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }
}

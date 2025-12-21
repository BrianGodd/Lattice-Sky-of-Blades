using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Update()
    {
        
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 0)
        {
            Destroy(gameObject);
            return;
        }
        
    }
    public void PlaySFX3D(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        Debug.Log("Playing 3D SFX: " + clip.name + " at " + position);
        
        GameObject go = new GameObject("SFX_OneShot");
        go.transform.position = position;

        AudioSource a = go.AddComponent<AudioSource>();
        a.clip = clip;
        a.volume = volume;
        a.spatialBlend = 1f;   // 3D
        a.rolloffMode = AudioRolloffMode.Logarithmic;
        a.minDistance = 1f;
        a.maxDistance = 20f;

        a.Play();
        Destroy(go, clip.length);
    }

}

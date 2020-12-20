using UnityEngine;


public class AudioSpeedMatcher : MonoBehaviour
{

  AudioSource audioSource;


  public void Start()
  {
      audioSource = GetComponent<AudioSource>();
  }

  public void Update()
  {
      audioSource.pitch = Time.timeScale;
  }
}

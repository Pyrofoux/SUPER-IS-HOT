using UnityEngine;


public class AudioSpeedMatcher : MonoBehaviour
{

  public AudioSource audioSource;


  public void Start()
  {
      
  }

  public void Update()
  {
      audioSource.pitch = Time.timeScale;
  }
}

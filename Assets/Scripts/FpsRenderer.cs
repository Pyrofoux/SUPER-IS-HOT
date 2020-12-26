using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsRenderer : MonoBehaviour
{
  [Header("HUD thingies")]
    public Image weaponCursor;
    public GameObject hud;
    public GameObject TextSUPER;
    public GameObject TextIS;
    public GameObject TextHOT;

    [Header("Audio clips")]
    public AudioClip SuperHotClip;
    public AudioClip GunShotClip;
    public AudioClip GunReloadClip;
    public AudioClip GunPickUpClip;
    public AudioClip GunThrowClip;
    public AudioClip GunEmptyClip;
    public AudioClip DeathClip;

    [Header("Themes")]
    public AudioClip BabaOriginalTheme;
    public AudioClip BabaRemixTheme;

    [Header("Audio Sources")]
    public AudioSource audioSourceTimeDeformed;
    public AudioSource audioSourceClassic;
    public AudioSource audioSourceBackgroundTheme;
    private WeaponScript weapon;

    bool dying = false;
    bool winning = false;
    bool firstThemePlay = false;

    private float bgVolume = 0.75f;
    private float soundsVolume = 0.9f;

    EffectsApplicator effectsApplicator;
    // Start is called before the first frame update
    void Start()
    {
      effectsApplicator = GetComponent<EffectsApplicator>();
      weapon = effectsApplicator.weapon;

      TextSUPER.SetActive(false);
      TextIS.SetActive(false);
      TextHOT.SetActive(false);

      // Fade in Background theme
      StartCoroutine(FadeIn(audioSourceBackgroundTheme, 0.1f, bgVolume));

    }

    // Update is called once per frame
    void Update()
    {
      weapon = effectsApplicator.weapon;


      if(dying)
      {
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 10);
      }
      //Stop time when Win
      //

    }

    // Gun UI stuff, could be placed somewhere else ideally
    public void ResetCursor()
    {
      weaponCursor.transform.localEulerAngles = new Vector3(0,0,0);
      weaponCursor.transform.localScale = WeaponScript.baseScale;
      Color color = weaponCursor.material.color;
      color.a = 1f;
      weaponCursor.material.color = color;
    }

    public void UpdateCursor()
    {

      if(weapon == null)
      {
        ResetCursor();
      }
      else
      {

        weaponCursor.transform.localEulerAngles = weapon.cursorTransform.transform.localEulerAngles;
        weaponCursor.transform.localScale = weapon.cursorTransform.transform.localScale;

        if(weapon.reloading)
        {
          Color color = weaponCursor.material.color;
          color.a = 0.75f;
          weaponCursor.material.color = color;
        }
        else
        {
          Color color = weaponCursor.material.color;
          color.a = 1f;
          weaponCursor.material.color = color;
        }

      }
    }


    public void PlaySound(string soundName)
    {
      float baseVolume = soundsVolume;
      if(soundName == "gunshot")
      {
        audioSourceClassic.PlayOneShot(GunShotClip, baseVolume);
        if(!effectsApplicator.CheckTimeMoves())
        audioSourceTimeDeformed.PlayOneShot(GunShotClip, baseVolume);
      }
      else if(soundName == "gun reload")
      {
        audioSourceClassic.PlayOneShot(GunReloadClip, baseVolume);
      }
      else if(soundName == "gun throw")
      {
        audioSourceClassic.PlayOneShot(GunThrowClip, baseVolume);
        if(!effectsApplicator.CheckTimeMoves())
        audioSourceTimeDeformed.PlayOneShot(GunThrowClip, baseVolume);
      }
      else if(soundName == "gun pickup")
      {
        audioSourceClassic.PlayOneShot(GunPickUpClip, baseVolume);
      }
      else if(soundName == "gun empty")
      {
        audioSourceClassic.PlayOneShot(GunEmptyClip, baseVolume);
      }
      else if(soundName == "death")
      {
        audioSourceClassic.PlayOneShot(DeathClip, baseVolume);
      }
      else if(soundName == "SUPERHOT")
      {
        audioSourceClassic.PlayOneShot(SuperHotClip, 1f);
      }
    }

    public void PlayClassicTheme()
    {
      float position = audioSourceBackgroundTheme.time+0.0f;
      audioSourceBackgroundTheme.Stop();
      audioSourceBackgroundTheme.loop = true;
      audioSourceBackgroundTheme.clip = BabaOriginalTheme;
      audioSourceBackgroundTheme.Play();
      audioSourceBackgroundTheme.time = position;
    }

    public void PlayRemixTheme()
    {
      float position = audioSourceBackgroundTheme.time+0.0f;
      audioSourceBackgroundTheme.Stop();
      audioSourceBackgroundTheme.loop = true;
      audioSourceBackgroundTheme.clip = BabaRemixTheme;
      audioSourceBackgroundTheme.Play();
      audioSourceBackgroundTheme.time = position;

    }

    public static IEnumerator FadeOut (AudioSource audioSource, float FadeStep)
    {

        while (audioSource.volume > 0) {
            audioSource.volume -= FadeStep;

            yield return null;
        }
        audioSource.volume = 0;
    }

    public static IEnumerator FadeIn (AudioSource audioSource, float FadeStep, float targetVolume)
    {
        audioSource.volume = 0;

        while (audioSource.volume < targetVolume) {
            audioSource.volume += FadeStep;

            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void ReloadLevel()
    {
      if(effectsApplicator.fading) return;
        effectsApplicator.fading = true;

        float duration = 1f;

        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Initiate.Fade(sceneName, Color.white, 1/duration);

    }

    public void LoadLevel(int from, int to)
    {

        float duration = 1f;
        string sceneName;
        Color colorFade = Color.white;

        if(from == 0 || to == 0)colorFade = Color.black;

          sceneName = "Level-"+to.ToString();
          Initiate.Fade(sceneName, colorFade, 1/duration);



    }

    public void Die()
    {
      if(effectsApplicator.fading) return;
      effectsApplicator.fading = true;
      effectsApplicator.ThrowWeapon();
      // + Camera "fall" effect in effectsApplicator

      dying = true;

      PlaySound("death");

      string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
      float duration = 1.5f;
      Initiate.Fade(sceneName, Color.red, 1/duration);
    }

    public void Win()
    {
      if(effectsApplicator.fading) return;
      effectsApplicator.fading = true;

      winning = true;

      effectsApplicator.ManualTimeStop(true);
      string nextLevel = effectsApplicator.getNextLevel();

      StartCoroutine(EndAnimation(3, nextLevel));

    }

    //Usage: StartCoroutine(Wait(float time))
    IEnumerator EndAnimation(int times, string sceneName)
    {
      float durationA = 0.5f;
      float durationB = 1f;
      float durationC = 0.8f;
      float durationD = 0.68f;
        for(int i = 0; i < times; i++)
        {
          TextSUPER.SetActive(false);
          TextIS.SetActive(false);
          TextHOT.SetActive(false);

          yield return new WaitForSecondsRealtime(durationA);
          PlaySound("SUPERHOT");
          TextSUPER.SetActive(true);
          yield return new WaitForSecondsRealtime(durationB);
          TextIS.SetActive(true);
          yield return new WaitForSecondsRealtime(durationC);
          TextHOT.SetActive(true);
          yield return new WaitForSecondsRealtime(durationD);

          if(i == times-2)
          {
            Initiate.Fade(sceneName, Color.white, 1/(durationA+durationB+durationC+durationD));
          }

        }

    }

    public void OpenDisplay()
    {
      PlayRemixTheme();
      hud.SetActive(true);
    }

    public void CloseDisplay()
    {


      PlayClassicTheme();
      hud.SetActive(false);
    }


}

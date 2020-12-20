using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsRenderer : MonoBehaviour
{

    public Image weaponCursor;

    [Header("Audio clips")]
    public AudioClip SuperHotClip;
    public AudioClip GunShotClip;
    public AudioClip GunReloadClip;
    public AudioClip GunPickUpClip;
    public AudioClip GunThrowClip;
    public AudioClip DeathClip;

    private AudioSource audioSource;
    private GameObject hud;
    private WeaponScript weapon;

    bool dying = false;

    EffectsApplicator effectsApplicator;
    // Start is called before the first frame update
    void Start()
    {
      hud = (GameObject) GameObject.Find("HUD_SuperHot");
      effectsApplicator = GetComponent<EffectsApplicator>();
      weapon = effectsApplicator.weapon;

      audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      weapon = effectsApplicator.weapon;


      if(dying)
      {
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 10);
      }

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


    public void PlaySound(string soundName, Vector3 position)
    {
      float baseVolume = 0.9f;
      if(soundName == "gunshot")
      {
        audioSource.PlayOneShot(GunShotClip, baseVolume);
      }
      else if(soundName == "gun reload")
      {
        audioSource.PlayOneShot(GunReloadClip, baseVolume);
      }
      else if(soundName == "gun throw")
      {
        audioSource.PlayOneShot(GunThrowClip, baseVolume);
      }
      else if(soundName == "gun pickup")
      {
        audioSource.PlayOneShot(GunPickUpClip, baseVolume);
      }
      else if(soundName == "death")
      {
        audioSource.PlayOneShot(DeathClip, baseVolume);
      }
    }

    public void ReloadLevel()
    {
      if(effectsApplicator.fading) return;
        effectsApplicator.fading = true;
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Initiate.Fade(sceneName, Color.white, 1.0f);

    }

    public void Die()
    {
      if(effectsApplicator.fading) return;
      effectsApplicator.fading = true;
      //effectsApplicator.ThrowWeapon();
      // Camera "fall" effect

      dying = true;

      string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
      Initiate.Fade(sceneName, Color.red, 1.5f);
    }

    public void Win()
    {
      effectsApplicator.fading = true;
      audioSource.PlayOneShot(SuperHotClip, 1.0f);

      string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
      float time = 5f;
      Initiate.Fade(sceneName, Color.white, 1/time);
    }

    public void OpenDisplay()
    {
      hud.SetActive(true);
    }

    public void CloseDisplay()
    {
      hud.SetActive(false);
    }
}

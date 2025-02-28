﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class WeaponScript : MonoBehaviour
{


    private Rigidbody rb;
    private Collider collider;
    private Renderer renderer;
    private  IEnumerator reloadCouroutine;

    [Space]
    [Header("Weapon Settings")]
    public float reloadTime = .3f;
    public int bulletAmount = 3;
    static public Vector3 baseScale = new Vector3(5,5,0);

    [Header("DO NOT TOUCH")]
    RuleHandler ruleHandler;
    EffectsApplicator effectsApplicator;
    public bool active = true;
    public bool reloading;
    public bool realizedEmpty = false; // player must realize a gun is empty before throwing it
    public GameObject cursorTransform;
    public GameObject lastBullet;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();

        //get time handling script -- for game pause

        effectsApplicator = (EffectsApplicator)GameObject.FindObjectOfType(typeof(EffectsApplicator));
        //get ruleHandler -- to modify rules in SuperHot
        ruleHandler = (RuleHandler)GameObject.FindObjectOfType(typeof(RuleHandler));

        cursorTransform = new GameObject();
        cursorTransform.transform.localScale = WeaponScript.baseScale;

        ChangeSettings();
    }

    // Change physics based on it being the main weapon or not
    void ChangeSettings()
    {
        if (transform.parent != null)
            return;

        rb.isKinematic = (effectsApplicator.weapon == this) ? true : false;
        rb.interpolation = (effectsApplicator.weapon == this) ? RigidbodyInterpolation.None : RigidbodyInterpolation.Interpolate;
        collider.isTrigger = (effectsApplicator.weapon == this);
    }

    public void Shoot(Vector3 pos,Quaternion rot, bool isEnemy)
    {
        if (reloading)
            return;

        if (bulletAmount <= 0)
            return;


        if(effectsApplicator.weapon == this)
        {
          bulletAmount--;
        }


        GameObject bullet = Instantiate(effectsApplicator.bulletPrefab, pos, rot);

        if(! isEnemy)
        {
          //bullet.GetComponent<BulletMovement>().SetScripts(ruleHandler, EffectsApplicator);
          bullet.tag = "PlayerBullet";
          bullet.layer = LayerMask.NameToLayer("Player Bullet");
          effectsApplicator.bulletList.Add(bullet);

          effectsApplicator.PlaySound("gunshot");

        }
        else
        {
          bullet.tag = "EnnemyBullet";
        }

        if (GetComponentInChildren<ParticleSystem>() != null)
            GetComponentInChildren<ParticleSystem>().Play();

        if(effectsApplicator.weapon == this)
        {
          reloading = true;
          reloadCouroutine = Reload();
          StartCoroutine(reloadCouroutine);
        }


        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .01f, 10, 90, false, true).SetUpdate(true);

        if(effectsApplicator.weapon == this)
            transform.DOLocalMoveZ(-.1f, .05f).OnComplete(()=>transform.DOLocalMoveZ(0,.2f));
    }

    public void Throw()
    {

        //Throwing gun animation
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMove(transform.position - transform.forward, .01f)).SetUpdate(true);
        s.AppendCallback(() => transform.parent = null);
        s.AppendCallback(() => transform.position = Camera.main.transform.position + (Camera.main.transform.right * .1f));
        s.AppendCallback(() => ChangeSettings());
        s.AppendCallback(() => rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse));
        s.AppendCallback(() => rb.AddTorque(transform.transform.right + transform.transform.up * 20, ForceMode.Impulse));

        realizedEmpty = false; //FOrget that a gun is empty when throwing it
        effectsApplicator.PlaySound("gun throw");

    }

    public void Pickup()
    {
        if (!active)
            return;

        effectsApplicator.weapon = this;
        ChangeSettings();

        transform.parent = effectsApplicator.weaponHolder;

        transform.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.OutBack).SetUpdate(true);
        transform.DOLocalRotate(Vector3.zero, .25f).SetUpdate(true);

        effectsApplicator.PlaySound("gun pickup");
    }

    public void Release()
    {
        active = true;
        transform.parent = null;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        collider.isTrigger = false;

        rb.AddForce((Camera.main.transform.position - transform.position) * 2, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 2, ForceMode.Impulse);

    }

    IEnumerator Reload()
    {
        /*if (effectsApplicator.weapon != this)
            yield break;*/
        ReloadUI(reloadTime);

        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        effectsApplicator.PlaySound("gun reload");
    }

    //Reloading icon
    public void ReloadUI(float time)
    {
        cursorTransform.transform.DORotate(new Vector3(0, 0, 90), time, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(() => cursorTransform.transform.DOPunchScale(Vector3.one / 3, .2f, 10, 1).SetUpdate(true));
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && collision.relativeVelocity.magnitude < 15)
        {
            BodyPartScript bp = collision.gameObject.GetComponent<BodyPartScript>();

            if (!bp.enemy.dead)
                Instantiate(effectsApplicator.hitParticlePrefab, transform.position, transform.rotation);

            bp.HidePartAndReplace();
            bp.enemy.Kill();
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class WeaponScript : MonoBehaviour
{
    [Header("Bools")]
    public bool active = true;
    public bool reloading;

    private Rigidbody rb;
    private Collider collider;
    private Renderer renderer;

    [Space]
    [Header("Weapon Settings")]
    public float reloadTime = .3f;
    public int bulletAmount = 6;

    [Header("DO NOT TOUCH")]
    RuleHandler ruleHandler;
    EffectsApplicator effectsApplicator;
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

        if(!effectsApplicator.weapon == this)
            bulletAmount--;

        GameObject bullet = Instantiate(effectsApplicator.bulletPrefab, pos, rot);

        if(! isEnemy)
        {
          //bullet.GetComponent<BulletMovement>().SetScripts(ruleHandler, EffectsApplicator);
          bullet.tag = "PlayerBullet";
          bullet.layer = LayerMask.NameToLayer("Player Bullet");
          effectsApplicator.bulletList.Add(bullet);
        }
        else
        {
          bullet.tag = "EnnemyBullet";
        }

        if (GetComponentInChildren<ParticleSystem>() != null)
            GetComponentInChildren<ParticleSystem>().Play();

        if(effectsApplicator.weapon == this)
            StartCoroutine(Reload());

        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .01f, 10, 90, false, true).SetUpdate(true);

        if(effectsApplicator.weapon == this)
            transform.DOLocalMoveZ(-.1f, .05f).OnComplete(()=>transform.DOLocalMoveZ(0,.2f));
    }

    public void Throw()
    {
        //Throwing animation
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOMove(transform.position - transform.forward, .01f)).SetUpdate(true);
        s.AppendCallback(() => transform.parent = null);
        s.AppendCallback(() => transform.position = Camera.main.transform.position + (Camera.main.transform.right * .1f));
        s.AppendCallback(() => ChangeSettings());
        s.AppendCallback(() => rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse));
        s.AppendCallback(() => rb.AddTorque(transform.transform.right + transform.transform.up * 20, ForceMode.Impulse));
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
        if (effectsApplicator.weapon != this)
            yield break;
        effectsApplicator.ReloadUI(reloadTime);
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float startForce;
    public int dmg;
    [SerializeField] float destroyTime;

    [SerializeField] Rigidbody rb;

    Collider sender;

    bool isStuck;

    // Start is called before the first frame update
    public void Fire(Collider sender, float power)
    {
        this.sender = sender;

        rb.AddForce(transform.forward * startForce * power, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStuck)
        {

        }
        else
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (sender == other) { return; }

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        isStuck = true;

        transform.parent = other.transform;

        if (other.gameObject.TryGetComponent(out IHitable hitableThing))
        {
            hitableThing.OnHit(this);
        }
    }
}

public interface IHitable
{
    public void OnHit(Arrow arrow) { }
}

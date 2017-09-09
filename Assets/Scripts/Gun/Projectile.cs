using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    public float speed = 10;
    public bool playerFire;

    void Start()
    {
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
        CheckCollisions(moveDistance);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + .1f, collisionMask, QueryTriggerInteraction.Collide))
        {
            Entity entity = hit.collider.GetComponent<Entity>();
            if (entity != null)
            {
                OnHitObject(entity, hit.point);
            }
            Destroy(gameObject);
        }
    }

    void OnHitObject(Entity entity, Vector3 hitPoint)
    {
        if ((entity.GetComponent<Player>() == null && playerFire) || (entity.GetComponent<Player>() != null && !playerFire))
        {
            entity.TakeHit(hitPoint, transform.forward);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

using Mirror;

public class Bomb : NetworkBehaviour
{
    public float explosionRadius = 2f;
    public float explosionDelay = 2f;
    public float destroyDelay = 1f;

    public GameObject linePrefab;

    private Animation anim;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    void Awake()
    {
        // Get the animation
        anim = GetComponent<Animation>();
    }
    void Start()
    {
        StartCoroutine(Explode());
    }

    public IEnumerator Explode()
    {
        // After a few seconds explode
        yield return new WaitForSeconds(explosionDelay);

        Explode(transform.position, explosionRadius);
        CmdExplode(transform.position, explosionRadius);

        // Play the shrink animation
        anim.Play("Shrink");

        yield return new WaitForSeconds(destroyDelay);

        // Finally destroy the gameObject
        NetworkServer.Destroy(gameObject);
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        // Have a line renderer into the bomb
        GameObject clone = Instantiate(linePrefab, transform);
        LineRenderer line = clone.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    [Command]
    void CmdExplode(Vector3 position, float radius)
    {
        // Have the ability of destroying on the Network
        Collider[] hits = Physics.OverlapSphere(position, radius);
        foreach (var hit in hits)
        {
            NetworkIdentity networkId = hit.GetComponent<NetworkIdentity>();
            if (networkId && hit.name.Contains("Enemy"))
            {
                NetworkServer.Destroy(hit.gameObject);
            }
        }
    }

    void Explode(Vector3 position, float radius)
    {
        // Have the line on Network
        Collider[] hits = Physics.OverlapSphere(position, radius);
        foreach (var hit in hits)
        {
            NetworkIdentity networkId = hit.GetComponent<NetworkIdentity>();
            if (networkId && hit.name.Contains("Enemy"))
            {
                CreateLine(transform.position, hit.transform.position);
            }
        }
    }


}
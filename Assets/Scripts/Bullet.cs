using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 dir;
    float life = 0.9f;

    void Update()
    {
        transform.position += dir * 16f * Time.deltaTime;
        foreach (Zombie z in Zombie.All)
        {
            Vector3 d = z.transform.position - transform.position; d.y = 0;
            if (d.magnitude < 1f) { z.Hit(); Destroy(gameObject); return; }
        }
        if ((life -= Time.deltaTime) <= 0) Destroy(gameObject);
    }
}

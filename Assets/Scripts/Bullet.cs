using UnityEngine;

// Flies straight; damages the first zombie it touches.
public class Bullet : MonoBehaviour
{
    public Vector3 dir = Vector3.forward;
    public float speed = 16f;
    public float life = 0.9f;
    public float hitRadius = 1.0f;
    float t;

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        t += Time.deltaTime;
        for (int i = Zombie.All.Count - 1; i >= 0; i--)
        {
            Zombie z = Zombie.All[i];
            Vector3 d = z.transform.position - transform.position;
            d.y = 0f;
            if (d.magnitude < hitRadius)
            {
                z.Hit(1);
                Destroy(gameObject);
                return;
            }
        }
        if (t > life) Destroy(gameObject);
    }
}

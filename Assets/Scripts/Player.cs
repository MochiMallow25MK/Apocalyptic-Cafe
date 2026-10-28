using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6f, fireRate = 0.35f;
    public GameObject bulletPrefab;
    Vector3 face = Vector3.forward;
    float nextShot;

    void Update()
    {
        Game g = Game.I;
        if (g.over) { g.hint = ""; return; }

        // move (clamped to the play area)
        Vector3 d = Vector3.ClampMagnitude(new Vector3(Keys.Move.x, 0, Keys.Move.y), 1f);
        if (d.sqrMagnitude > 0.01f) { face = d.normalized; transform.rotation = Quaternion.LookRotation(face); }
        Vector3 p = transform.position + d * speed * Time.deltaTime;
        p.x = Mathf.Clamp(p.x, -14.5f, 14.5f);
        p.z = Mathf.Clamp(p.z, -10.5f, 2.2f);
        transform.position = p;

        // nearest station in range
        Station near = null; float best = 999f;
        foreach (Station s in Station.All)
        {
            Vector3 o = s.transform.position - p; o.y = 0;
            if (o.magnitude <= s.range && o.magnitude < best) { near = s; best = o.magnitude; }
        }
        g.hint = near != null ? "[E]  " + near.Prompt : "";
        if (near != null && Keys.Interact) near.Use();

        // shoot
        if (Keys.Fire && Time.time >= nextShot)
        {
            nextShot = Time.time + fireRate;
            GameObject b = Instantiate(bulletPrefab, transform.position + face * 0.9f, Quaternion.identity);
            b.GetComponent<Bullet>().dir = face;
        }
    }
}

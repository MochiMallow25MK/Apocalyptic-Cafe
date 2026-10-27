using UnityEngine;

// WASD / arrows move, E interacts with the nearest thing, SPACE shoots in the direction you face.
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float fireCooldown = 0.35f;
    public Vector2 minXZ = new Vector2(-14.5f, -10.5f);
    public Vector2 maxXZ = new Vector2(14.5f, 2.2f);

    Vector3 facing = Vector3.forward;
    float nextFire;

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null) return;
        if (gm.State != GameState.Playing) { gm.Hint = ""; return; }

        // --- move ---
        Vector2 m = GameInput.Move;
        Vector3 dir = new Vector3(m.x, 0f, m.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();
        if (dir.sqrMagnitude > 0.01f)
        {
            facing = dir.normalized;
            transform.rotation = Quaternion.LookRotation(facing);
        }
        Vector3 p = transform.position + dir * speed * Time.deltaTime;
        p.x = Mathf.Clamp(p.x, minXZ.x, maxXZ.x);
        p.z = Mathf.Clamp(p.z, minXZ.y, maxXZ.y);
        transform.position = p;

        // --- interact ---
        Interactable near = FindNearest();
        gm.Hint = (near != null) ? "[E]  " + near.Prompt : "";
        if (near != null && GameInput.InteractDown)
        {
            if (near.CanInteract) near.Interact();
            else gm.Toast(near.Prompt);
        }

        // --- shoot ---
        if (GameInput.FireHeld && Time.time >= nextFire)
        {
            nextFire = Time.time + fireCooldown;
            Shoot();
        }
    }

    Interactable FindNearest()
    {
        Interactable best = null;
        float bestDist = float.MaxValue;
        foreach (Interactable i in Interactable.All)
        {
            Vector3 d = i.transform.position - transform.position;
            d.y = 0f;
            float dist = d.magnitude;
            if (dist <= i.radius && dist < bestDist) { best = i; bestDist = dist; }
        }
        return best;
    }

    void Shoot()
    {
        GameObject b = Prim.Make(PrimitiveType.Sphere, "Bullet", transform.position + Vector3.up * 1f + facing * 0.9f, Vector3.one * 0.35f, new Color(1f, 0.9f, 0.3f));
        Bullet bullet = b.AddComponent<Bullet>();
        bullet.dir = facing;
    }
}

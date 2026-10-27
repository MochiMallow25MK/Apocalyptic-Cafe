using System.Collections.Generic;
using UnityEngine;

// Walks toward the cafe. Once close, it keeps draining reputation until you shoot it.
public class Zombie : MonoBehaviour
{
    public static List<Zombie> All = new List<Zombie>();

    public float speed = 1.6f;
    public int health = 3;
    public float attackRange = 3f;
    public int reputationDamagePerSecond = 2;
    public Vector3 target = new Vector3(0f, 0f, 4.5f);

    float attackTimer, flashTimer;
    Renderer body;
    Color normal = new Color(0.4f, 0.7f, 0.3f);
    Color attacking = new Color(0.75f, 0.55f, 0.2f);
    bool isAttacking;

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }
    void Start() { body = Prim.BodyRenderer(transform); }

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.State != GameState.Playing) return;

        Vector3 to = target - transform.position;
        to.y = 0f;
        if (to.magnitude > attackRange)
        {
            transform.position += to.normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(to.normalized);
            isAttacking = false;
        }
        else
        {
            isAttacking = true;
            attackTimer += Time.deltaTime;
            if (attackTimer >= 1f)
            {
                attackTimer = 0f;
                gm.AddReputation(-reputationDamagePerSecond, "Zombies are scaring customers!");
            }
        }

        if (body != null)
        {
            if (flashTimer > 0f) { flashTimer -= Time.deltaTime; body.material.color = Color.white; }   // hit flash
            else body.material.color = isAttacking ? attacking : normal;                               // orange = attacking
        }
    }

    public void Hit(int damage)
    {
        if (health <= 0) return;      // already dead this frame
        health -= damage;
        flashTimer = 0.12f;
        if (health <= 0)
        {
            GameManager.Instance.OnZombieKilled();
            Destroy(gameObject);
        }
    }
}

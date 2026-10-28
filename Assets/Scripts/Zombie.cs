using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public static List<Zombie> All = new List<Zombie>();
    [HideInInspector] public Transform target;     // set by Spawner (the counter)
    public int hp = 3;
    public float speed = 1.6f, reach = 3f;
    float tick;
    Renderer r;
    Color baseColor;

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }
    void Start() { r = GetComponent<Renderer>(); baseColor = r.material.color; }

    void Update()
    {
        if (Game.I.over) return;
        r.material.color = Color.Lerp(r.material.color, baseColor, 6f * Time.deltaTime);   // hit flash fades
        Vector3 to = target.position - transform.position; to.y = 0;
        if (to.magnitude > reach)
        {
            transform.position += to.normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(to);
        }
        else if ((tick += Time.deltaTime) >= 1f)
        {
            tick = 0f;
            Game.I.Rep(-2, "Zombies are scaring customers!");
        }
    }

    public void Hit()
    {
        if (hp <= 0) return;
        hp--;
        r.material.color = Color.white;
        if (hp <= 0) { Game.I.kills++; Game.I.Say("Zombie down!"); Destroy(gameObject); }
    }
}

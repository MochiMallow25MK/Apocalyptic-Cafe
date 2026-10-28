using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public static List<Customer> All = new List<Customer>();
    public static int Waiting { get { int n = 0; foreach (Customer c in All) if (c.waiting) n++; return n; } }

    public float patience = 35f;
    [HideInInspector] public int order;                 // 0 Coffee, 1 Fruit Juice, 2 Fruit Latte
    [HideInInspector] public Vector3 seat, exit;
    [HideInInspector] public bool waiting, leaving;

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    void Update()
    {
        if (Game.I.over) return;
        transform.position = Vector3.MoveTowards(transform.position, leaving ? exit : seat, 3f * Time.deltaTime);
        if (leaving) { if ((transform.position - exit).sqrMagnitude < 0.01f) Destroy(gameObject); return; }
        waiting = (transform.position - seat).sqrMagnitude < 0.01f;
        if (waiting && (patience -= Time.deltaTime) <= 0f) { Game.I.Rep(-10, "A customer left angry!"); Leave(false); }
    }

    public void Leave(bool happy)
    {
        leaving = true; waiting = false;
        GetComponent<Renderer>().material.color = happy ? Color.green : Color.red;
    }
}

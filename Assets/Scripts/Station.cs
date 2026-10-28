using System.Collections.Generic;
using UnityEngine;

// Used by the 6 farm plots AND the counter. Choose the Kind in the Inspector.
public class Station : MonoBehaviour
{
    public static List<Station> All = new List<Station>();
    public enum Kind { Coffee, Fruit, Counter }
    public Kind kind;
    public float growTime = 6f, range = 1.8f;
    public Transform crop;                 // the child sphere (plots only)
    float timer;

    void OnEnable() { All.Add(this); timer = growTime; }   // plots start ripe
    void OnDisable() { All.Remove(this); }
    bool Ready { get { return timer >= growTime; } }

    public string Label
    {
        get
        {
            if (kind == Kind.Counter) return "COUNTER (" + Customer.Waiting + " waiting)";
            return Ready ? kind + " READY" : kind + " " + Mathf.RoundToInt(100f * timer / growTime) + "%";
        }
    }

    public string Prompt
    {
        get
        {
            if (kind == Kind.Counter) return "Serve a customer";
            return Ready ? "Harvest " + kind : kind + " still growing";
        }
    }

    void Update()
    {
        if (kind == Kind.Counter || Game.I.over) return;
        timer = Mathf.Min(timer + Time.deltaTime, growTime);
        float s = Mathf.Lerp(0.2f, 0.9f, timer / growTime);                 // crop grows
        float bob = Ready ? Mathf.Sin(Time.time * 5f) * 0.08f : 0f;         // ripe crop bobs
        crop.localScale = Vector3.one * s;
        crop.localPosition = new Vector3(0f, 0.25f + s * 0.5f + bob, 0f);
    }

    public void Use()
    {
        Game g = Game.I;
        if (kind == Kind.Counter)
        {
            Customer best = null;
            foreach (Customer c in Customer.All)
                if (c.waiting && g.CanServe(c.order) && (best == null || c.patience < best.patience)) best = c;
            if (best == null) { g.Say(Customer.Waiting > 0 ? "You don't have the ingredients" : "No customers waiting"); return; }
            g.Serve(best.order); best.Leave(true); g.didServe = true;
            g.Say("Served " + Game.Names[best.order] + "!  +$" + Game.Price[best.order]);
            return;
        }
        if (!Ready) { g.Say("Still growing..."); return; }
        if (!g.Take(kind == Kind.Coffee)) { g.Say("Hands full! Deliver to the counter"); return; }
        timer = 0f; g.didHarvest = true; g.Say("+1 " + kind);
    }
}

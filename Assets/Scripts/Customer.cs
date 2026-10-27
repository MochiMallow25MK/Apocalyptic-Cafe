using System.Collections.Generic;
using UnityEngine;

public enum OrderType { Coffee, FruitJuice, FruitLatte }

// What each order needs and pays. Change the numbers here to rebalance.
public static class Orders
{
    public static int Beans(OrderType o) { return (o == OrderType.Coffee || o == OrderType.FruitLatte) ? 1 : 0; }
    public static int Fruits(OrderType o) { return (o == OrderType.FruitJuice || o == OrderType.FruitLatte) ? 1 : 0; }
    public static int Price(OrderType o)
    {
        switch (o)
        {
            case OrderType.Coffee: return 4;
            case OrderType.FruitJuice: return 4;
            default: return 9;
        }
    }
    public static string Name(OrderType o)
    {
        switch (o)
        {
            case OrderType.Coffee: return "Coffee";
            case OrderType.FruitJuice: return "Fruit Juice";
            default: return "Fruit Latte";
        }
    }
}

// A survivor: walks to a free spot, waits (patience runs down), then leaves happy or angry.
public class Customer : MonoBehaviour
{
    public static List<Customer> All = new List<Customer>();
    public static int WaitingCount
    {
        get { int n = 0; foreach (Customer c in All) if (c.IsWaiting) n++; return n; }
    }

    enum S { Arriving, Waiting, Leaving }

    public OrderType order;
    public float PatienceLeft { get; private set; }
    public bool IsWaiting { get { return state == S.Waiting; } }

    S state = S.Arriving;
    Vector3 slotPos, exitPoint;
    int slotIndex;

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    public void Init(OrderType o, int slot, Vector3 slotPosition, Vector3 exit, float patience)
    {
        order = o;
        slotIndex = slot;
        slotPos = slotPosition;
        exitPoint = exit;
        PatienceLeft = patience;
    }

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.State != GameState.Playing) return;

        switch (state)
        {
            case S.Arriving:
                if (MoveTo(slotPos, 3f)) state = S.Waiting;
                break;
            case S.Waiting:
                PatienceLeft -= Time.deltaTime;
                if (PatienceLeft <= 0f)
                {
                    gm.AddReputation(-gm.reputationLostAngryCustomer, "A customer left angry!  -" + gm.reputationLostAngryCustomer + " reputation");
                    Leave(false);
                }
                break;
            case S.Leaving:
                if (MoveTo(exitPoint, 4f)) Destroy(gameObject);
                break;
        }
    }

    bool MoveTo(Vector3 target, float speed)
    {
        Vector3 to = target - transform.position;
        to.y = 0f;
        float step = speed * Time.deltaTime;
        if (to.magnitude <= step)
        {
            transform.position = new Vector3(target.x, transform.position.y, target.z);
            return true;
        }
        transform.position += to.normalized * step;
        transform.rotation = Quaternion.LookRotation(to.normalized);
        return false;
    }

    public void Leave(bool happy)
    {
        if (state == S.Leaving) return;
        state = S.Leaving;
        if (Spawner.Instance != null) Spawner.Instance.Release(slotIndex);
        Renderer r = Prim.BodyRenderer(transform);
        if (r != null) r.material.color = happy ? new Color(0.3f, 0.9f, 0.4f) : new Color(0.9f, 0.3f, 0.3f);
    }
}

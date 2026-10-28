using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject customerPrefab, zombiePrefab;
    public Transform counter, customerStart, customerExit;
    public Transform[] seats, zombieSpawns;
    public float firstCustomer = 5f, customerEvery = 11f;
    public float firstZombie = 25f, zombieEvery = 14f, zombieMin = 6f;
    Customer[] taken;
    float ct, zt;

    void Start() { taken = new Customer[seats.Length]; ct = firstCustomer; zt = firstZombie; }

    void Update()
    {
        if (Game.I.over) return;
        if ((ct -= Time.deltaTime) <= 0f) ct = SpawnCustomer() ? customerEvery : 2f;
        if ((zt -= Time.deltaTime) <= 0f)
        {
            SpawnZombie();
            zombieEvery = Mathf.Max(zombieMin, zombieEvery - 1f);     // waves get faster
            zt = zombieEvery;
        }
    }

    bool SpawnCustomer()
    {
        for (int i = 0; i < seats.Length; i++)
        {
            if (taken[i] != null && !taken[i].leaving) continue;      // seat busy
            Customer c = Instantiate(customerPrefab, customerStart.position, Quaternion.identity).GetComponent<Customer>();
            float r = Random.value;
            c.order = r < 0.45f ? 0 : r < 0.75f ? 1 : 2;
            c.seat = seats[i].position; c.exit = customerExit.position;
            c.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.value, 0.35f, 0.95f);
            taken[i] = c;
            return true;
        }
        return false;
    }

    void SpawnZombie()
    {
        Transform sp = zombieSpawns[Random.Range(0, zombieSpawns.Length)];
        Zombie z = Instantiate(zombiePrefab, sp.position, Quaternion.identity).GetComponent<Zombie>();
        z.target = counter;
        Game.I.Say("A zombie is coming!");
    }
}

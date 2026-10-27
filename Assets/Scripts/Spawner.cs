using UnityEngine;

// Creates customers and zombies on a timer. All numbers are public: tweak them in the Inspector.
public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [Header("Customers")]
    public float firstCustomerAt = 5f;
    public float customerInterval = 11f;
    public float customerPatience = 35f;

    [Header("Zombies (difficulty ramps up)")]
    public float firstZombieAt = 25f;        // grace period = gentle onboarding
    public float zombieInterval = 14f;
    public float minZombieInterval = 6f;
    public float intervalDrop = 1f;          // each wave comes this many seconds sooner
    public int zombieHealth = 3;
    public float zombieSpeed = 1.6f;

    [Header("Optional: drag imported models (prefabs) here")]
    public GameObject customerModel;
    public GameObject zombieModel;

    public Vector3[] slots = new Vector3[]
    {
        new Vector3(-3f, 0f, 3.2f), new Vector3(-1f, 0f, 3.2f),
        new Vector3(1f, 0f, 3.2f),  new Vector3(3f, 0f, 3.2f)
    };

    bool[] used;
    float customerTimer, zombieTimer, currentZombieInterval;

    void Awake()
    {
        Instance = this;
        used = new bool[slots.Length];
        customerTimer = firstCustomerAt;
        zombieTimer = firstZombieAt;
        currentZombieInterval = zombieInterval;
    }

    public void Release(int index)
    {
        if (used != null && index >= 0 && index < used.Length) used[index] = false;
    }

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.State != GameState.Playing) return;

        customerTimer -= Time.deltaTime;
        if (customerTimer <= 0f)
        {
            customerTimer = SpawnCustomer() ? customerInterval : 2f;   // retry soon if the cafe is full
        }

        zombieTimer -= Time.deltaTime;
        if (zombieTimer <= 0f)
        {
            SpawnZombie();
            currentZombieInterval = Mathf.Max(minZombieInterval, currentZombieInterval - intervalDrop);
            zombieTimer = currentZombieInterval;
        }
    }

    OrderType PickOrder()
    {
        float r = Random.value;
        if (r < 0.45f) return OrderType.Coffee;
        if (r < 0.75f) return OrderType.FruitJuice;
        return OrderType.FruitLatte;
    }

    bool SpawnCustomer()
    {
        int free = -1;
        for (int i = 0; i < used.Length; i++) { if (!used[i]) { free = i; break; } }
        if (free < 0) return false;
        used[free] = true;

        Color c = Color.HSVToRGB(Random.value, 0.35f, 0.95f);
        GameObject go = Prim.Character("Customer", new Vector3(-15.5f, 0f, 3.2f), new Vector3(0.8f, 0.8f, 0.8f), c, customerModel);
        Customer cust = go.AddComponent<Customer>();
        cust.Init(PickOrder(), free, slots[free], new Vector3(15.5f, 0f, 3.2f), customerPatience);
        return true;
    }

    void SpawnZombie()
    {
        Vector3 pos;
        int side = Random.Range(0, 3);
        if (side == 0) pos = new Vector3(Random.Range(-13f, 13f), 0f, -11f);
        else if (side == 1) pos = new Vector3(-15f, 0f, Random.Range(-10f, -1f));
        else pos = new Vector3(15f, 0f, Random.Range(-10f, -1f));

        GameObject go = Prim.Character("Zombie", pos, new Vector3(1f, 0.9f, 1f), new Color(0.4f, 0.7f, 0.3f), zombieModel);
        Zombie z = go.AddComponent<Zombie>();
        z.health = zombieHealth;
        z.speed = zombieSpeed;
        GameManager.Instance.Toast("A zombie is coming!");
    }
}

using UnityEngine;

// Builds the whole prototype level when you press Play.
// Put this on ONE empty GameObject in an empty scene and you are done.
[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(Spawner))]
public class GameBootstrap : MonoBehaviour
{
    [Header("Optional: drag an imported model (prefab) here")]
    public GameObject playerModel;

    void Start()
    {
        SetupCamera();
        EnsureLight();
        BuildWorld();
    }

    void SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject g = new GameObject("Main Camera");
            g.tag = "MainCamera";
            cam = g.AddComponent<Camera>();
            g.AddComponent<AudioListener>();
        }
        cam.transform.position = new Vector3(0f, 22f, -9f);
        cam.transform.LookAt(new Vector3(0f, 0f, 1f));
        cam.fieldOfView = 50f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.08f, 0.08f, 0.1f);
    }

    void EnsureLight()
    {
        if (FindObjectsByType<Light>(FindObjectsSortMode.None).Length > 0) return;
        GameObject l = new GameObject("Sun");
        Light light = l.AddComponent<Light>();
        light.type = LightType.Directional;
        l.transform.rotation = Quaternion.Euler(55f, -30f, 0f);
    }

    void BuildWorld()
    {
        GameManager gm = GameManager.Instance;

        // ground
        Prim.Make(PrimitiveType.Cube, "Ground", new Vector3(0f, -0.1f, -0.5f), new Vector3(34f, 0.2f, 26f), new Color(0.27f, 0.29f, 0.22f));

        // cafe building + counter
        Prim.Make(PrimitiveType.Cube, "CafeBuilding", new Vector3(0f, 1.5f, 8f), new Vector3(11f, 3f, 5f), new Color(0.6f, 0.4f, 0.3f));
        GameObject counter = Prim.Make(PrimitiveType.Cube, "Counter", new Vector3(0f, 0.5f, 4.5f), new Vector3(6f, 1f, 1f), new Color(0.85f, 0.7f, 0.4f));
        CafeCounter cc = counter.AddComponent<CafeCounter>();
        cc.radius = 4.2f;
        gm.AddLabel(new Vector3(0f, 3.4f, 8f), "APOCALYPTIC CAFE");

        // farm: coffee on the left, fruit on the right
        for (int i = 0; i < 3; i++)
        {
            MakePlot(CropType.Coffee, new Vector3(-11f + i * 2.2f, 0f, -4f), 6f);
            MakePlot(CropType.Fruit, new Vector3(6.6f + i * 2.2f, 0f, -4f), 4f);
        }

        // player
        GameObject player = Prim.Character("Player", new Vector3(0f, 0f, -1f), Vector3.one, new Color(0.3f, 0.75f, 0.95f), playerModel);
        player.AddComponent<PlayerController>();
    }

    void MakePlot(CropType type, Vector3 pos, float growTime)
    {
        GameObject go = new GameObject(type + "Plot");
        go.transform.position = pos;
        FarmPlot plot = go.AddComponent<FarmPlot>();
        plot.Init(type, growTime);
    }
}

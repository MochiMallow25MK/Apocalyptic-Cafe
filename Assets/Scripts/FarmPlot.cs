using UnityEngine;

public enum CropType { Coffee, Fruit }

// A plot grows a crop. When it is ripe the player presses E to harvest it, then it regrows.
public class FarmPlot : Interactable
{
    public CropType crop = CropType.Coffee;
    public float growTime = 6f;

    float timer;
    bool ready;
    Transform cropT;

    public void Init(CropType type, float time)
    {
        crop = type;
        growTime = time;
        timer = growTime;      // start ripe so the player can harvest right away (onboarding)
        ready = true;
        radius = 1.8f;
        Prim.Make(PrimitiveType.Cube, "Soil", transform.position + Vector3.up * 0.1f, new Vector3(1.8f, 0.2f, 1.8f), new Color(0.6f, 0.5f, 0.35f), transform);
        Color c = (type == CropType.Coffee) ? new Color(0.3f, 0.15f, 0.05f) : new Color(0.9f, 0.25f, 0.2f);
        cropT = Prim.Make(PrimitiveType.Sphere, "Crop", transform.position + Vector3.up * 0.7f, Vector3.one * 0.9f, c, transform).transform;
    }

    string CropName { get { return crop == CropType.Coffee ? "Coffee Beans" : "Fruit"; } }

    public override string Prompt { get { return ready ? "Harvest " + CropName : CropName + " still growing..."; } }
    public override bool CanInteract { get { return ready; } }
    public override string WorldLabel
    {
        get { return ready ? CropName + " READY" : CropName + " " + Mathf.RoundToInt(100f * timer / growTime) + "%"; }
    }

    void Update()
    {
        if (cropT == null) return;
        GameManager gm = GameManager.Instance;
        if (gm == null || gm.State != GameState.Playing) return;

        if (!ready)
        {
            timer += Time.deltaTime;
            if (timer >= growTime) { timer = growTime; ready = true; }
        }
        float g = Mathf.Clamp01(timer / growTime);
        float size = Mathf.Lerp(0.15f, 0.9f, g);                 // crop grows visibly
        float bob = ready ? Mathf.Sin(Time.time * 5f) * 0.08f : 0f; // ripe crop bobs = "pick me"
        cropT.localScale = Vector3.one * size;
        cropT.position = transform.position + Vector3.up * (0.3f + size * 0.45f + bob);
    }

    public override void Interact()
    {
        GameManager gm = GameManager.Instance;
        if (!gm.TryCollect(crop)) { gm.Toast("Hands full! Deliver to the counter first"); return; }
        ready = false;
        timer = 0f;
        gm.HarvestedOnce = true;
        gm.Toast("+1 " + (crop == CropType.Coffee ? "Coffee Beans" : "Fruit"));
    }
}

using UnityEngine;

// Helpers that build the whole prototype from Unity's built-in shapes,
// so you do NOT need to import any art to run the game.
public static class Prim
{
    public static GameObject Make(PrimitiveType type, string name, Vector3 pos, Vector3 scale, Color color, Transform parent = null)
    {
        GameObject go = GameObject.CreatePrimitive(type);
        go.name = name;
        Collider col = go.GetComponent<Collider>();
        if (col != null) Object.Destroy(col);          // we use simple distance checks, not physics
        go.transform.position = pos;
        go.transform.localScale = scale;
        if (parent != null) go.transform.SetParent(parent, true);
        go.GetComponent<Renderer>().material.color = color;
        return go;
    }

    // A character = empty root (feet on the ground) + capsule "Body" + small "Nose" showing facing.
    // If you assign an imported model prefab, it replaces the capsule.
    public static GameObject Character(string name, Vector3 pos, Vector3 bodyScale, Color color, GameObject modelPrefab)
    {
        GameObject root = new GameObject(name);
        root.transform.position = pos;
        GameObject body = Make(PrimitiveType.Capsule, "Body", pos + Vector3.up * bodyScale.y, bodyScale, color, root.transform);
        GameObject nose = Make(PrimitiveType.Cube, "Nose", pos + new Vector3(0f, bodyScale.y * 1.2f, bodyScale.z * 0.5f), new Vector3(0.25f, 0.25f, 0.5f), new Color(1f, 0.9f, 0.4f), root.transform);
        if (modelPrefab != null)
        {
            body.GetComponent<Renderer>().enabled = false;
            Object.Destroy(nose);
            GameObject model = Object.Instantiate(modelPrefab, root.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
        }
        return root;
    }

    public static Renderer BodyRenderer(Transform root)
    {
        Transform b = root.Find("Body");
        return b != null ? b.GetComponent<Renderer>() : null;
    }
}

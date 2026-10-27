using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Won, Lost }

// Holds the rules, the numbers (score, reputation, inventory) and draws the whole HUD.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Win / lose rules")]
    public int startReputation = 50;
    public int maxReputation = 100;
    public int customersToWin = 8;

    [Header("Player carrying")]
    public int carryLimit = 3;

    [Header("Reputation changes")]
    public int reputationPerServe = 8;
    public int reputationLostAngryCustomer = 10;

    public GameState State { get; private set; }
    public int Reputation { get; private set; }
    public int Coins { get; private set; }
    public int Served { get; private set; }
    public int Beans { get; private set; }
    public int Fruits { get; private set; }
    public int ZombiesKilled { get; private set; }
    public float Elapsed { get; private set; }

    public string Hint = "";
    public bool HarvestedOnce, ServedOnce, KilledOnce;

    string toast = "";
    float toastTimer;

    struct StaticLabel { public Vector3 pos; public string text; }
    List<StaticLabel> staticLabels = new List<StaticLabel>();

    GUIStyle centerStyle, leftStyle;

    void Awake()
    {
        Instance = this;
        State = GameState.Playing;
        Reputation = startReputation;
    }

    // ---------- rules / state ----------
    public void AddLabel(Vector3 pos, string text)
    {
        StaticLabel l; l.pos = pos; l.text = text;
        staticLabels.Add(l);
    }

    public void Toast(string msg) { toast = msg; toastTimer = 2.5f; }

    public void AddReputation(int amount, string message = null)
    {
        Reputation = Mathf.Clamp(Reputation + amount, 0, maxReputation);
        if (message != null) Toast(message);
    }

    public bool TryCollect(CropType type)
    {
        if (type == CropType.Coffee) { if (Beans >= carryLimit) return false; Beans++; }
        else { if (Fruits >= carryLimit) return false; Fruits++; }
        return true;
    }

    public bool CanFill(OrderType o) { return Beans >= Orders.Beans(o) && Fruits >= Orders.Fruits(o); }
    public void Consume(OrderType o) { Beans -= Orders.Beans(o); Fruits -= Orders.Fruits(o); }

    public void Serve(int price)
    {
        Coins += price;
        Served++;
        ServedOnce = true;
        AddReputation(reputationPerServe);
    }

    public void OnZombieKilled()
    {
        ZombiesKilled++;
        KilledOnce = true;
        Toast("Zombie down!");
    }

    void Update()
    {
        if (toastTimer > 0f) toastTimer -= Time.deltaTime;
        if (GameInput.RestartDown) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (State != GameState.Playing) return;

        Elapsed += Time.deltaTime;
        if (Reputation <= 0) State = GameState.Lost;
        else if (Served >= customersToWin) State = GameState.Won;
    }

    // ---------- HUD (IMGUI: no Canvas or TextMeshPro needed) ----------
    void Fill(Rect r, Color c)
    {
        Color old = GUI.color;
        GUI.color = c;
        GUI.DrawTexture(r, Texture2D.whiteTexture);
        GUI.color = old;
    }

    void Text(Rect r, string s, GUIStyle st, Color c)
    {
        st.normal.textColor = Color.black;
        GUI.Label(new Rect(r.x + 1.5f, r.y + 1.5f, r.width, r.height), s, st);   // shadow
        st.normal.textColor = c;
        GUI.Label(r, s, st);
    }

    void WorldText(Vector3 worldPos, string s, Color c)
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        Vector3 sp = cam.WorldToScreenPoint(worldPos);
        if (sp.z <= 0f) return;
        Vector2 size = centerStyle.CalcSize(new GUIContent(s));
        Rect r = new Rect(sp.x - size.x / 2f - 6f, Screen.height - sp.y - size.y / 2f, size.x + 12f, size.y + 2f);
        Fill(r, new Color(0f, 0f, 0f, 0.55f));
        Text(r, s, centerStyle, c);
    }

    string Tutorial()
    {
        if (!HarvestedOnce) return "Walk to a farm plot and press E to harvest";
        if (!ServedOnce) return "Carry ingredients to the COUNTER and press E to serve a customer";
        if (Zombie.All.Count > 0 && !KilledOnce) return "Zombies are attacking! Face them and hold SPACE to shoot";
        return "";
    }

    void OnGUI()
    {
        if (Event.current.type != EventType.Repaint) return;

        int fs = Mathf.Max(14, Screen.height / 36);
        if (centerStyle == null)
        {
            centerStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = false };
            leftStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, wordWrap = false };
        }
        centerStyle.fontSize = fs;
        leftStyle.fontSize = fs;
        float pad = fs * 0.6f;
        float line = fs * 1.5f;

        // --- top-left status panel ---
        float w = fs * 19f;
        Fill(new Rect(pad, pad, w, line * 5f + pad), new Color(0f, 0f, 0f, 0.6f));
        float x = pad * 2f, y = pad * 1.4f;
        Text(new Rect(x, y, w, line), "REPUTATION  " + Reputation + " / " + maxReputation, leftStyle, Color.white);
        Rect bar = new Rect(x, y + line, w - pad * 2f, fs * 0.9f);
        Fill(bar, new Color(0.25f, 0.25f, 0.25f));
        float frac = Mathf.Clamp01(Reputation / (float)maxReputation);
        Fill(new Rect(bar.x, bar.y, bar.width * frac, bar.height), Color.Lerp(new Color(0.9f, 0.2f, 0.2f), new Color(0.3f, 0.85f, 0.35f), frac));
        Text(new Rect(x, y + line * 2f, w, line), "Coins $" + Coins + "     Served " + Served + " / " + customersToWin, leftStyle, new Color(1f, 0.9f, 0.5f));
        Text(new Rect(x, y + line * 3f, w, line), "Carrying   Beans " + Beans + "/" + carryLimit + "    Fruit " + Fruits + "/" + carryLimit, leftStyle, Color.white);
        int secs = Mathf.FloorToInt(Elapsed);
        Text(new Rect(x, y + line * 4f, w, line), "Time " + (secs / 60) + ":" + (secs % 60).ToString("00") + "     Zombies killed " + ZombiesKilled, leftStyle, new Color(0.8f, 0.9f, 0.8f));

        // --- top-right order menu ---
        float mw = fs * 17f;
        float mx = Screen.width - mw - pad;
        Fill(new Rect(mx, pad, mw, line * 4f + pad), new Color(0f, 0f, 0f, 0.6f));
        Text(new Rect(mx + pad, pad * 1.4f, mw, line), "ORDER MENU", leftStyle, new Color(1f, 0.8f, 0.4f));
        int row = 1;
        foreach (OrderType o in System.Enum.GetValues(typeof(OrderType)))
        {
            string need = (Orders.Beans(o) > 0 ? Orders.Beans(o) + " bean " : "") + (Orders.Fruits(o) > 0 ? Orders.Fruits(o) + " fruit" : "");
            Text(new Rect(mx + pad, pad * 1.4f + line * row, mw, line), Orders.Name(o) + ": " + need + " ($" + Orders.Price(o) + ")", leftStyle, Color.white);
            row++;
        }

        // --- world labels ---
        foreach (StaticLabel l in staticLabels) WorldText(l.pos, l.text, new Color(1f, 0.85f, 0.5f));
        foreach (Interactable i in Interactable.All) WorldText(i.transform.position + Vector3.up * 2f, i.WorldLabel, Color.white);
        foreach (Customer c in Customer.All)
        {
            if (!c.IsWaiting) continue;
            bool canServe = CanFill(c.order);
            Color col = canServe ? new Color(0.4f, 1f, 0.5f) : (c.PatienceLeft < 10f ? new Color(1f, 0.4f, 0.4f) : Color.white);
            WorldText(c.transform.position + Vector3.up * 2.4f, Orders.Name(c.order) + "  " + Mathf.CeilToInt(c.PatienceLeft) + "s" + (canServe ? "  (you can serve!)" : ""), col);
        }

        // --- bottom: tutorial, hint, controls ---
        string tut = Tutorial();
        if (tut != "") WorldTextScreen(tut, Screen.height - line * 3.4f, fs, new Color(1f, 0.9f, 0.4f));
        if (Hint != "") WorldTextScreen(Hint, Screen.height - line * 2.1f, fs, Color.white);
        Text(new Rect(pad, Screen.height - line - pad * 0.5f, Screen.width, line), "WASD / Arrows: move     E: interact     SPACE: shoot     R: restart", leftStyle, new Color(1f, 1f, 1f, 0.75f));

        // --- toast message ---
        if (toastTimer > 0f && toast != "") WorldTextScreen(toast, line * 6.2f, fs, new Color(1f, 0.95f, 0.6f));

        // --- end screen ---
        if (State != GameState.Playing)
        {
            Fill(new Rect(0, 0, Screen.width, Screen.height), new Color(0f, 0f, 0f, 0.7f));
            GUIStyle bigStyle = new GUIStyle(centerStyle);
            bigStyle.fontSize = fs * 2;
            bool won = State == GameState.Won;
            Text(new Rect(0, Screen.height * 0.3f, Screen.width, fs * 3f), won ? "THE CAFE SURVIVED!" : "REPUTATION RUINED - THE CAFE CLOSED", bigStyle, won ? new Color(0.5f, 1f, 0.6f) : new Color(1f, 0.45f, 0.45f));
            Text(new Rect(0, Screen.height * 0.3f + fs * 3f, Screen.width, line), "Served " + Served + " customers   |   Coins $" + Coins + "   |   Zombies killed " + ZombiesKilled + "   |   Time " + (secs / 60) + ":" + (secs % 60).ToString("00"), centerStyle, Color.white);
            Text(new Rect(0, Screen.height * 0.3f + fs * 5f, Screen.width, line), "Press R to play again", centerStyle, new Color(1f, 0.9f, 0.4f));
        }
    }

    void WorldTextScreen(string s, float yPos, int fs, Color c)
    {
        Vector2 size = centerStyle.CalcSize(new GUIContent(s));
        Rect r = new Rect(Screen.width / 2f - size.x / 2f - 10f, yPos, size.x + 20f, size.y + 4f);
        Fill(r, new Color(0f, 0f, 0f, 0.6f));
        Text(r, s, centerStyle, c);
    }
}

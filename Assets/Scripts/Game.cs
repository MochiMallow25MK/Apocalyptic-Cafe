using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game I;
    public int rep = 50, winAt = 10, carry = 3;
    [HideInInspector] public int coins, served, kills, beans, fruits;
    [HideInInspector] public bool over, won, didHarvest, didServe;
    [HideInInspector] public string hint = "";
    string toast = "";
    float toastT, time;
    int fs;
    GUIStyle mid;

    public static readonly string[] Names = { "Coffee", "Fruit Juice", "Fruit Latte" };
    public static readonly int[] NeedBean = { 1, 0, 1 }, NeedFruit = { 0, 1, 1 }, Price = { 4, 4, 9 };

    void Awake() { I = this; }
    public void Say(string m) { toast = m; toastT = 2.5f; }
    public void Rep(int d, string m = null) { rep = Mathf.Clamp(rep + d, 0, 100); if (m != null) Say(m); }
    public bool CanServe(int o) { return beans >= NeedBean[o] && fruits >= NeedFruit[o]; }
    public void Serve(int o) { beans -= NeedBean[o]; fruits -= NeedFruit[o]; coins += Price[o]; served++; Rep(10); }

    public bool Take(bool coffee)
    {
        if (coffee) { if (beans >= carry) return false; beans++; }
        else { if (fruits >= carry) return false; fruits++; }
        return true;
    }

    void Update()
    {
        toastT -= Time.deltaTime;
        if (Keys.Restart) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (over) return;
        time += Time.deltaTime;
        if (rep <= 0 || served >= winAt) { over = true; won = rep > 0; }
    }

    void Tag(Vector3 world, string text)
    {
        Vector3 p = Camera.main.WorldToScreenPoint(world);
        if (p.z > 0) GUI.Label(new Rect(p.x - fs * 6, Screen.height - p.y - fs, fs * 12, fs * 2), text, mid);
    }

    void Msg(string text, float y)
    {
        if (text == "") return;
        Rect r = new Rect(Screen.width / 2f - fs * 15, y, fs * 30, fs * 1.8f);
        GUI.Box(r, ""); GUI.Label(r, text, mid);
    }

    void OnGUI()
    {
        if (Event.current.type != EventType.Repaint) return;
        fs = Mathf.Max(14, Screen.height / 36);
        if (mid == null) mid = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        mid.fontSize = fs; GUI.skin.label.fontSize = fs;

        // status panel (top-left) + reputation bar
        GUI.Box(new Rect(10, 10, fs * 17, fs * 7.4f), "");
        GUI.Label(new Rect(20, 14, fs * 16, fs * 1.6f), "REPUTATION " + rep + "/100");
        GUI.color = Color.Lerp(Color.red, Color.green, rep / 100f);
        GUI.DrawTexture(new Rect(20, 14 + fs * 1.6f, fs * 16 * rep / 100f, fs * 0.5f), Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(20, 14 + fs * 2.3f, fs * 16, fs * 5), "Coins $" + coins + "   Served " + served + "/" + winAt + "\nBeans " + beans + "/" + carry + "   Fruit " + fruits + "/" + carry + "\nTime " + (int)time + "s   Zombies killed " + kills);

        // order menu (top-right)
        GUI.Box(new Rect(Screen.width - fs * 17 - 10, 10, fs * 17, fs * 6), "");
        GUI.Label(new Rect(Screen.width - fs * 17, 14, fs * 16, fs * 5.6f), "ORDER MENU\nCoffee: 1 bean ($4)\nFruit Juice: 1 fruit ($4)\nFruit Latte: bean + fruit ($9)");

        // floating labels
        foreach (Station s in Station.All) Tag(s.transform.position + Vector3.up * 2f, s.Label);
        foreach (Customer c in Customer.All)
        {
            if (!c.waiting) continue;
            GUI.color = CanServe(c.order) ? Color.green : Color.white;
            Tag(c.transform.position + Vector3.up * 2.4f, Names[c.order] + "  " + Mathf.CeilToInt(c.patience) + "s");
            GUI.color = Color.white;
        }

        // messages
        string tip = !didHarvest ? "Walk to a plot and press E to harvest"
                   : !didServe ? "Take ingredients to the COUNTER and press E"
                   : (Zombie.All.Count > 0 && kills == 0) ? "Zombies! Face them and hold SPACE to shoot" : "";
        Msg(tip, Screen.height - fs * 6f);
        Msg(hint, Screen.height - fs * 3.8f);
        Msg(toastT > 0 ? toast : "", fs * 8f);
        GUI.Label(new Rect(10, Screen.height - fs * 2, Screen.width, fs * 2), "WASD move    E interact    SPACE shoot    R restart");

        if (over)
        {
            GUI.color = new Color(0, 0, 0, 0.7f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
            mid.fontSize = fs * 2;
            GUI.Label(new Rect(0, Screen.height * 0.35f, Screen.width, fs * 3), won ? "THE CAFE SURVIVED!" : "REPUTATION RUINED", mid);
            mid.fontSize = fs;
            GUI.Label(new Rect(0, Screen.height * 0.35f + fs * 3, Screen.width, fs * 2), "Press R to play again", mid);
        }
    }
}

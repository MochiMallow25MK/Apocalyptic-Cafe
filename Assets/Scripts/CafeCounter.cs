using UnityEngine;

// The counter: press E to serve the most impatient customer whose order you can fill.
public class CafeCounter : Interactable
{
    public override string Prompt
    {
        get { return Customer.WaitingCount > 0 ? "Serve a customer" : "Counter (no customers waiting)"; }
    }
    public override string WorldLabel { get { return "COUNTER  (" + Customer.WaitingCount + " waiting)"; } }
    public override bool CanInteract { get { return true; } }

    public override void Interact()
    {
        GameManager gm = GameManager.Instance;
        if (Customer.WaitingCount == 0) { gm.Toast("No customers waiting yet"); return; }

        Customer best = null;
        foreach (Customer c in Customer.All)
        {
            if (c.IsWaiting && gm.CanFill(c.order) && (best == null || c.PatienceLeft < best.PatienceLeft)) best = c;
        }
        if (best == null) { gm.Toast("You don't have the ingredients for any order"); return; }

        int price = Orders.Price(best.order);
        gm.Consume(best.order);
        gm.Serve(price);
        best.Leave(true);
        gm.Toast("Served " + Orders.Name(best.order) + "!  +$" + price + "  +" + gm.reputationPerServe + " reputation");
    }
}

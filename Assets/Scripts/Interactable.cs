using System.Collections.Generic;
using UnityEngine;

// Base class for anything the player can use with the E key (farm plots, the counter).
public abstract class Interactable : MonoBehaviour
{
    public static List<Interactable> All = new List<Interactable>();
    public float radius = 2f;                      // how close the player must be

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    public abstract string Prompt { get; }         // text shown at the bottom of the screen
    public abstract string WorldLabel { get; }     // text floating above the object
    public abstract bool CanInteract { get; }
    public abstract void Interact();
}

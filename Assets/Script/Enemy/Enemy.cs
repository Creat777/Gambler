using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int damage { get; protected set; }
    public GameObject player {  get; protected set; }

    public abstract void OutOfScreen();
}

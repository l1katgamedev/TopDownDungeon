using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Text currentAmmoText;
    public PlayerController player;


    private void Update()
    {
        currentAmmoText.text = player.currentAmmo.ToString();
    }
}

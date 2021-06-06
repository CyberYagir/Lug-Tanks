using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerBonus {
    public float fireRateIncrease = 1;
    public float speedIncrease = 1;
    public float heathAdd = 0;
    public float defenceIncrease = 1;
    public float time = 10;
}


public class TankModificators : MonoBehaviour
{
    public List<PlayerBonus> playerBonus;
}

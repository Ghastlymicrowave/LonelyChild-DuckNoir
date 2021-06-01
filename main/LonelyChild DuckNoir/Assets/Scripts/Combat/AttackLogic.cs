using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public class AttackLogic : MonoBehaviour
{
    public battleBehavior bb;
    public PlayerCursor playerCursor;
    public Attack attack;
    float timer = 0f;

    void Start()
    {
        playerCursor.bb = this;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int index = 0;
        foreach (float t in attack.spawnTimes)
        {
            if (timer > t && t > -1f) // -2f means that a time has already been met.
            {
                //Instantiate(attack.projectiles[index], attack.spawnLocations[index].position, attack.spawnLocations[index].rotation);
                Instantiate(attack.projectiles[index], attack.spawnLocations[index].position, attack.spawnLocations[index].rotation, this.gameObject.transform);
                attack.spawnTimes[index] = -2f;
            }
            index += 1;
        }
        if (timer > attack.duration)
        {
            EndTurn();
        }
    }
    void EndTurn()
    {
        //battle behavior end enemy turn
        this.gameObject.SetActive(false);
        print("endturn");
    }
    public void Damage(int damage)
    {
        print("damage" + attack.damage.ToString());
        if (damage + attack.damage > attack.maxDamage)
        {
            // battlebehavior damage player (attack.maxDamage - attack.damage)
            EndTurn();
        }
        else
        {
            attack.damage += damage;
            //battlebehavior damage player (damage)
        }
    }
}

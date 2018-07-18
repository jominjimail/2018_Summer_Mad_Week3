using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWind : MonoBehaviour {

    private float EffectDuration = 2.5f;
    private int EffectDamage = 4;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        if (other.tag == "AntiMonsters")
        {
            Unit enemy = other.GetComponent<BodyHitBox>().getUnitFromHitbox();
            if (enemy.IsAlive)
            {
                enemy.HitByDebuff(1, EffectDamage, EffectDuration);
            }
        }
    }

    public void Spawn()
    {
        StartCoroutine(SpellCast());
    }

    private IEnumerator SpellCast()
    {
        float Progress = 0;
        Vector3 to = 0.5f * (gamescript.GetRSpawnLoc() + gamescript.GetLSpawnLoc());
        while (Progress <= 2)
        {
            transform.position = Vector3.MoveTowards(gamescript.GetLSpawnLoc(), to, 12 * Progress);
            transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f), new Vector3(2, 2), Progress);
            Progress += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

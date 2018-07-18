using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public class Move : MonoBehaviour {

    [SerializeField]
    Vector3 to;
    Vector3 originalpos;
    Vector3 originalscale;
    public bool movemode;
    bool ismoving = false;
	
	void Start()
    {
        originalpos = transform.position;
        originalscale = transform.localScale;
    }
	public void MoveTo()
    {
        if(gameObject.GetComponent<AudioSource>() != null)
        {
            gameObject.GetComponent<AudioSource>().Play(0);
        }
        if (!ismoving)
            StartCoroutine(GoCrazy());
    }

    private IEnumerator GoCrazy()
    {
        float Progress = 0;
        ismoving = true;
        if (movemode)
        while (Progress <= 4)
        {
            transform.position = Vector3.MoveTowards(originalpos, to, 5 * Progress);
            transform.localScale = originalscale + new Vector3(Mathf.Sin(Progress*9), Mathf.Sin(Progress*5));
            Progress += Time.deltaTime;
            yield return null;
        }
        else
            while (Progress <= 2.5)
            {
                transform.localScale = Vector3.MoveTowards(originalscale, to, 1 * Progress);
                transform.position = originalpos + new Vector3(Mathf.Sin(Progress * 3), Mathf.Sin(Progress * 5),-9);
                Progress += Time.deltaTime;
                yield return null;
            }
        transform.position = originalpos;
        transform.localScale = originalscale;
        ismoving = false;
    }
}

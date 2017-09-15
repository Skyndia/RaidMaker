using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImage : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Rotate());
	}

    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(0, 0, -10);
            yield return new WaitForSeconds(0.02f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNER : MonoBehaviour {

    public GameObject[] characters;
    float randomTime;
    float timeCounter;
    public float deviation;
    int coin;
    

	void Update ()
    {
        if (timeCounter > randomTime)
        {
            coin = Random.Range(0, 8);
            GameObject newcharacter = Instantiate(characters[coin], transform.position + (transform.right * Random.Range(-1f, 1f)),transform.rotation * Quaternion.Euler(Vector3.up * Random.Range(-deviation, deviation)));
            if (coin < 4)
            {
                newcharacter.GetComponent<GirlTTPrefabMaker>().Getready();
                newcharacter.GetComponent<GirlTTPrefabMaker>().Randomize();
                newcharacter.GetComponent<playanimation>().playtheanimation("TTG_walk2");
                newcharacter.GetComponent<GirlTTPrefabMaker>().FIX();
            }
            else
            {
                newcharacter.GetComponent<BoyTTPrefabMaker>().Getready();
                newcharacter.GetComponent<BoyTTPrefabMaker>().Randomize();
                newcharacter.GetComponent<playanimation>().playtheanimation("TTB_walk2");
                newcharacter.GetComponent<BoyTTPrefabMaker>().FIX();
            }
            randomTime = Random.Range(1, 4);
            timeCounter = 0f;
        }
        timeCounter += Time.deltaTime;
    }
}

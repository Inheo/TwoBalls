using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class GirlVRTTPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int skintone;
    public bool hoodactive;
    public bool hoodon;
    public bool hoodup;
    public bool glassesactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject GOhands;
    GameObject GOneck;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject GOglasses;
    GameObject[] GOhoods;
    public Object[] MATSkins;
    public Object[] MATHairA;
    public Object[] MATHairB;
    public Object[] MATHairC;
    public Object[] MATHairD;
    public Object[] MATHairE;
    public Object[] MATEyes;
    public Object[] MATGlasses;
    public Object[] MATTshirt;
    public Object[] MATSweater;
    public Object[] MATHatA;
    public Object[] MATHatB;
    public Object[] MATHoods;
    Material headskin;


    void Start()
    {
        allOptions = false;
    }

    public void Getready()
    {
        GOhead = (GetComponent<Transform>().GetChild(0).gameObject);

        GOhair = new GameObject[7];
        GOchest = new GameObject[5];
        GOhoods = new GameObject[2];

        //load models
        GOhands = (GetComponent<Transform>().GetChild(7).gameObject);
        GOneck = (GetComponent<Transform>().GetChild(12).gameObject);
        for (int forAUX = 0; forAUX < 5; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 2).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOhair[forAUX + 5] = (GetComponent<Transform>().GetChild(forAUX + 8).gameObject);
        for (int forAUX = 0; forAUX < 5; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 13).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOhoods[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 10).gameObject);
        GOglasses = transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject;

        if (GOhair[0].activeSelf && GOhair[1].activeSelf && GOhair[2].activeSelf)
        {
            ResetSkin();
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            if (GOchest[0].activeSelf) hoodactive = true;
            if (GOhoods[0].activeSelf) { hoodon = true; hoodup = false; }
            if (GOhoods[1].activeSelf) { hoodon = true; hoodup = true; hair = 0; }
            if (!GOhoods[0].activeSelf && !GOhoods[1].activeSelf) hoodon = false;
            if (hair > 4) hatactive = true;
        }
    }

    void ResetSkin()
    {
        string[] allskins = new string[4] { "TTGirlA0", "TTGirlB0", "TTGirlC0", "TTGirlD0" };
        Material[] AUXmaterials;
        int materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        //ref head material
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
            for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                {
                    if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                    {
                        headskin = AUXmaterials[forAUX2];
                    }
                }
        GOhands.GetComponent<Renderer>().sharedMaterial  =headskin;
        GOneck.GetComponent<Renderer>().sharedMaterial = headskin;

        //chest
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }        }
       
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(false);
        GOglasses.SetActive(false);
        glassesactive = false;
        hoodactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(true);
        GOglasses.SetActive(true);
        glassesactive = true;
        hoodactive = true;
    }
    public void Menu()
    {
        allOptions = !allOptions;
    }

    public void Checkhood()
    {
        if (chest == 0)
        {
            hoodactive = true;
            if (hoodon) GOhoods[0].SetActive(true);
            GOhoods[1].SetActive(false);
        }
        else
        {
            hoodactive = false;
            GOhoods[0].SetActive(false);
            GOhoods[1].SetActive(false);
        }
    }
    public void Hoodonoff()
    {
        hoodon = !hoodon;
        GOhoods[0].SetActive(hoodon);
        GOhoods[1].SetActive(false);
        if (!hoodon) GOhair[hair].SetActive(true);

    }
    public void Hoodupdown()
    {
        if (GOhoods[0].activeSelf)
        {
            GOhoods[0].SetActive(false);
            GOhoods[1].SetActive(true);
            hoodup = true;
            GOhair[hair].SetActive(false);
            hatactive = false;

        }
        else
        {
            GOhoods[1].SetActive(false);
            GOhoods[0].SetActive(true);
            hoodup = false;
            GOhair[hair].SetActive(true);
            hatactive = true;

        }
    }
    public void Glasseson()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }

    //models
    public void Nexthat()
    {
        if (hoodactive && hoodup)
        {
            Hoodupdown();
        }
        if (hair < 5)
        {
            GOhair[hair].SetActive(false);
            hair = 5;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
        else
        {
            GOhair[hair].SetActive(false);
            if (hair < GOhair.Length - 1)
            {
                hair++;
                hatactive = true;
            }
            else
            {
                hair = 5;
                hatactive = false;
            }
            GOhair[hair].SetActive(true);
        }

    }
    public void Prevhat()
    {
        if (hoodactive && hoodup)
        {
            Hoodupdown();
        }
        if (hair < 6)
        {
            GOhair[hair].SetActive(false);
            hair = 6;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
        else
        {
            GOhair[hair].SetActive(false);
            if (hair > 5)
            {
                hair--;
                hatactive = true;
            }
            else
            {
                hair = 3;
                hatactive = false;
            }
            GOhair[hair].SetActive(true);
        }
    }
    public void Nexthair()
    {
        if (hoodup && hoodactive) Hoodupdown();
        GOhair[hair].SetActive(false);
        if (hatactive) hair = 0;
        hatactive = false;
        if (hair < GOhair.Length - 4) hair++;
        else hair = 0;
        GOhair[hair].SetActive(true);
    }
    public void Prevhair()
    {
        if (hoodup) Hoodupdown();
        GOhair[hair].SetActive(false);
        if (hatactive) hair = GOhair.Length - 3;
        hatactive = false;
        if (hair > 0) hair--;
        else hair = GOhair.Length - 4;
        GOhair[hair].SetActive(true);
    }
    public void Nextchest()
    {
        GOchest[chest].SetActive(false);
        if (chest < GOchest.Length - 1) chest++;
        else chest = 0;
        GOchest[chest].SetActive(true);
        Checkhood();

    }
    public void Prevchest()
    {
        GOchest[chest].SetActive(false);
        chest--;
        if (chest < 0) chest = GOchest.Length - 1;
        GOchest[chest].SetActive(true);
        Checkhood();
    }

    //materials
    public void Nextskincolor(int todo)
    {
        ChangeMaterials(MATSkins, todo);
    }
    public void Nextglasses(int todo)
    {
        ChangeMaterials(MATGlasses, todo);
    }
    public void Nexteyescolor(int todo)
    {
        ChangeMaterials(MATEyes, todo);
    }
    public void Nexthaircolor(int todo)
    {
        ChangeMaterials(MATHairA, todo);
        ChangeMaterials(MATHairB, todo);
        ChangeMaterials(MATHairC, todo);
        ChangeMaterials(MATHairD, todo);
        ChangeMaterials(MATHairE, todo);
    }
    public void Nexthatcolor(int todo)
    {
        if (hatactive && !hoodup)
        {
            if (hair == 5) ChangeMaterials(MATHatA, todo);
            if (hair == 6) ChangeMaterials(MATHatB, todo);
        }
    }
    public void Nextchestcolor(int todo)
    {        
        if (chest == 0)
        {
            ChangeMaterials(MATSweater, todo);
            ChangeMaterials(MATHoods, todo);
        }
        else ChangeMaterials(MATTshirt, todo);
    }

    public void Resetmodel()
    {
        Activateall();
        ChangeMaterials(MATHatA, 3);
        ChangeMaterials(MATHatB, 3);
        ChangeMaterials(MATSkins, 3);
        ChangeMaterials(MATHairA, 3);
        ChangeMaterials(MATHairB, 3);
        ChangeMaterials(MATHairC, 3);
        ChangeMaterials(MATHairD, 3);
        ChangeMaterials(MATHairE, 3);
        ChangeMaterials(MATHoods, 3);
        ChangeMaterials(MATGlasses, 3);
        ChangeMaterials(MATEyes, 3);
        ChangeMaterials(MATTshirt, 3);
        ChangeMaterials(MATSweater, 3);
        Menu();
    }    
    public void Randomize()
    {
        Deactivateall();
        //models
        hair = Random.Range(0, GOhair.Length);
        GOhair[hair].SetActive(true); if (hair > 4) hatactive = true; else hatactive = false;
        chest = Random.Range(0, GOchest.Length); GOchest[chest].SetActive(true);

        if (Random.Range(0, 4) > 2)
        {
            glassesactive = true;
            GOglasses.SetActive(true);
            ChangeMaterials(MATGlasses, 2);
        }
        else glassesactive = false;

        Checkhood();
        if (hoodactive)
        {
            if (Random.Range(0, 5) > 2)
            {
                hoodon = true;
                if (Random.Range(0, 5) > 2) Hoodupdown();
            }
        }
        //materials
        ChangeMaterials(MATEyes, 2);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 17)); forAUX2++) Nextchestcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 12)); forAUX2++) Nexthatcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 4)); forAUX2++) Nexthaircolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 4)); forAUX2++) Nextskincolor(0);
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 17; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<GirlVRTTPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 17; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<GirlVRTTPrefabMaker>());
        DestroyImmediate(gameObject);
    }

    void ChangeMaterial(GameObject GO, Object[] MAT, int todo)
    {
        bool found = false;
        int MATindex = 0;
        int subMAT = 0;
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;

        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX].name == MAT[forAUX2].name)
                {
                    subMAT = forAUX;
                    MATindex = forAUX2;
                    found = true;
                }
            }
        if (found)
        {
            if (todo == 0) //increase
            {
                MATindex++;
                if (MATindex > MAT.Length - 1) MATindex = 0;
            }
            if (todo == 1) //decrease
            {
                MATindex--;
                if (MATindex < 0) MATindex = MAT.Length - 1;
            }
            if (todo == 2) //random value
            {
                MATindex = Random.Range(0, MAT.Length);
            }
            if (todo == 3) //reset value
            {
                MATindex = 0;
            }
            if (todo == 4) //penultimate
            {
                MATindex = MAT.Length - 2;
            }
            if (todo == 5) //last one
            {
                MATindex = MAT.Length - 1;
            }
            AUXmaterials[subMAT] = MAT[MATindex] as Material;
            GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        }
    }
    void ChangeMaterials(Object[] MAT, int todo)
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) ChangeMaterial(GOhair[forAUX], MAT, todo);
        ChangeMaterial(GOhead, MAT, todo);
        ChangeMaterial(GOglasses, MAT, todo);
        ChangeMaterial(GOhands, MAT, todo);
        ChangeMaterial(GOneck, MAT, todo);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) ChangeMaterial(GOhoods[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) ChangeMaterial(GOchest[forAUX], MAT, todo);
    }
    void SwitchMaterial(GameObject GO, Object[] MAT1, Object[] MAT2)
    {
        Material[] AUXmaterials;
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        int materialcount = GO.GetComponent<Renderer>().sharedMaterials.Length;
        int index = 0;
        for (int forAUX = 0; forAUX < materialcount; forAUX++)
            for (int forAUX2 = 0; forAUX2 < MAT1.Length; forAUX2++)
            {
                if (AUXmaterials[forAUX].name == MAT1[forAUX2].name)
                {
                    index = forAUX2;
                    if (forAUX2 > MAT2.Length - 1) index -= (int)Mathf.Floor(index / 4) * 4;
                    AUXmaterials[forAUX] = MAT2[index] as Material;
                    GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                }
            }
    }
}
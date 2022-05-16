using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class BoyTTPrefabMaker : MonoBehaviour
{
    public bool allOptions;
    int hair;
    int chest;
    int legs;
    int feet;
    int tie;
    int jacket;
    int skintone;
    public bool hoodactive;
    public bool hoodon;
    public bool hoodup;
    public bool glassesactive;
    public bool jacketactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject GOheadsimple;
    GameObject[] GOfeet;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject[] GOlegs;
    GameObject GOglasses;
    GameObject GOjacket;
    GameObject[] GOhoods;
    public Object[] MATSkins;
    public Object[] MATHairA;
    public Object[] MATHairB;
    public Object[] MATHairC;
    public Object[] MATHairD;
    public Object[] MATHairE;
    public Object[] MATHairF;
    public Object[] MATEyes;
    public Object[] MATGlasses;
    public Object[] MATTshirt;
    public Object[] MATJacket;
    public Object[] MATSweater;
    public Object[] MATLegs;
    public Object[] MATFeetA;
    public Object[] MATFeetB;
    public Object[] MATFeetC;
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
        GOheadsimple = (GetComponent<Transform>().GetChild(1).gameObject);
        GetComponent<Transform>().GetChild(1).gameObject.SetActive(false); 
        GOfeet = new GameObject[5];
        GOhair = new GameObject[7];
        GOchest = new GameObject[9];
        GOlegs = new GameObject[8];
        GOhoods = new GameObject[2];

        //load models
        GOjacket = (GetComponent<Transform>().GetChild(18).gameObject);
        for (int forAUX = 0; forAUX < 5; forAUX++) GOfeet[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 4).gameObject);
        for (int forAUX = 0; forAUX < 7; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 9).gameObject);
        GOchest[0] = (GetComponent<Transform>().GetChild(3).gameObject);
        for (int forAUX = 0; forAUX < 8; forAUX++) GOlegs[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 19).gameObject);
        for (int forAUX = 0; forAUX < 8; forAUX++) GOchest[forAUX + 1] = (GetComponent<Transform>().GetChild(forAUX + 27).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOhoods[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 16).gameObject);
        GOglasses = transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject;
        
        if (GOfeet[0].activeSelf && GOfeet[1].activeSelf && GOfeet[2].activeSelf)
        {
            ResetSkin();
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            if (chest == 0 || chest > 3) while (!GOlegs[legs].activeSelf) legs++;
            while (!GOfeet[feet].activeSelf) feet++;
            if (GOjacket.activeSelf) jacketactive = true;
            if (GOchest[1].activeSelf) hoodactive = true;
            if (GOhoods[0].activeSelf) {  hoodon = true; hoodup = false; }
            if (GOhoods[1].activeSelf) {  hoodon = true; hoodup = true; hair = 0; }
            if (!GOhoods[0].activeSelf && !GOhoods[1].activeSelf) hoodon = false;
            if (hair > 4) hatactive = true;
        }
    }
    void ResetSkin()
    {
        string[] allskins = new string[4] { "TTBoyA0", "TTBoyB0", "TTBoyC0", "TTBoyD0" };
        Material[] AUXmaterials;
        int materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        //ref head material
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        materialcount = GOhead.GetComponent<Renderer>().sharedMaterials.Length;
        for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
            for (int forAUX3 = 0; forAUX3 < allskins.Length; forAUX3++)
                for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                {
                    if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                    {
                        headskin = AUXmaterials[forAUX2];
                    }
                }
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
                    }
        }
        //legs
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++)
        {
            AUXmaterials = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
        //feet
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++)
        {
            AUXmaterials = GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials;
            materialcount = GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials.Length;
            for (int forAUX2 = 0; forAUX2 < materialcount; forAUX2++)
                for (int forAUX3 = 0; forAUX3 < 4; forAUX3++)
                    for (int forAUX4 = 1; forAUX4 < 5; forAUX4++)
                    {
                        if (AUXmaterials[forAUX2].name == allskins[forAUX3] + forAUX4)
                        {
                            AUXmaterials[forAUX2] = headskin;
                            GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
                        }
                    }
        }
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(false);
        GOjacket.SetActive(false);
        GOglasses.SetActive(false);
        jacketactive = false;
        glassesactive = false;
        hoodactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(true);
        GOjacket.SetActive(true);
        GOglasses.SetActive(true);
        jacketactive = true;
        glassesactive = true;
        hoodactive = true;
    }
    public void Menu()
    {
        allOptions = !allOptions;
    }

    public void Checkhood()
    {
        if (chest == 1)
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
        }
    }
    public void Glasseson()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }
    public void Jacketon()
    {
        jacketactive = !jacketactive;
        GOjacket.SetActive(jacketactive);
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
                hatactive = true;
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
        if (hair < GOhair.Length - 3) hair++;
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
        else hair = GOhair.Length - 3;
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
    public void Nextlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs < GOlegs.Length - 1) legs++;
        else legs = 0;
        GOlegs[legs].SetActive(true);
    }
    public void Prevlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs > 0) legs--;
        else legs = GOlegs.Length - 1;
        GOlegs[legs].SetActive(true);
    }
    public void Nextfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet < GOfeet.Length - 1) feet++;
        else feet = 0;
        GOfeet[feet].SetActive(true);
    }
    public void Prevfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet > 0) feet--;
        else feet = GOfeet.Length - 1;
        GOfeet[feet].SetActive(true);
    }
    public void Nude()
    {
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(false);
        GOjacket.SetActive(false);
        jacketactive = false;
        hoodactive = false;
        GOchest[0].SetActive(true);
        GOlegs[0].SetActive(true);
        GOfeet[0].SetActive(true);
        chest = 0;
        legs = 0;
        feet = 0;
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
        ChangeMaterials(MATHairF, todo);

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
        if (chest == 1)
        {
            ChangeMaterials(MATSweater, todo);
            ChangeMaterials(MATHoods, todo);
        }
        if (chest > 1) ChangeMaterials(MATTshirt, todo);
    }
    public void Nextjacketcolor(int todo)
    {
        ChangeMaterials(MATJacket, todo);
    }
    public void Nextlegscolor(int todo)
    {
        ChangeMaterials(MATLegs, todo);
    }
    public void Nextfeetcolor(int todo)
    {
        if (feet == 1) ChangeMaterials(MATFeetA, todo);
        if (feet == 2) ChangeMaterials(MATFeetB, todo);
        if (feet == 3 || feet == 4) ChangeMaterials(MATFeetC, todo);
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
        ChangeMaterials(MATJacket, 3);
        ChangeMaterials(MATSweater, 3);
        ChangeMaterials(MATLegs, 3);
        ChangeMaterials(MATFeetA, 3);
        ChangeMaterials(MATFeetB, 3);
        ChangeMaterials(MATFeetC, 3);
        Menu();
    }
    public void Randomize()
    {
        Deactivateall();
        ResetSkin();
        //models
        hair = Random.Range(0, 7);
        GOhair[hair].SetActive(true);
        if (hair > 4) hatactive = true;else hatactive = false;
        chest = Random.Range(1, GOchest.Length); GOchest[chest].SetActive(true);
        legs = Random.Range(1, GOlegs.Length); GOlegs[legs].SetActive(true);
        feet = Random.Range(1, GOfeet.Length); GOfeet[feet].SetActive(true);

        if (Random.Range(0, 4) > 2) jacketactive = true; else jacketactive = false;
        GOjacket.SetActive(jacketactive);

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
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nexthaircolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 13)); forAUX++) Nextfeetcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 25)); forAUX++) Nextlegscolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 13)); forAUX++) Nextjacketcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 12)); forAUX++) Nexthatcolor(0);
        for (int forAUX = 0; forAUX < (Random.Range(0, 17)); forAUX++) Nextchestcolor(0);        
        for (int forAUX = 0; forAUX < (Random.Range(0, 4)); forAUX++) Nextskincolor(0);  
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 34; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyTTPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 34; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<BoyTTPrefabMaker>());
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
        ChangeMaterial(GOheadsimple, MAT, todo);
        ChangeMaterial(GOjacket, MAT, todo);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) ChangeMaterial(GOhoods[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) ChangeMaterial(GOchest[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) ChangeMaterial(GOlegs[forAUX], MAT, todo);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) ChangeMaterial(GOfeet[forAUX], MAT, todo);

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
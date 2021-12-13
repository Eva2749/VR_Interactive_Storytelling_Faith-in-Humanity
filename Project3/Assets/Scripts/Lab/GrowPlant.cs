using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    //get the mesh renderer and the material
    //public MeshRenderer growPlants;
    //public Material growMaterials;

    public float timeToGrow = 5;
    public float refreashRate = 0.05f;

    //create a slider to control the min and max value
    [Range(-2,4)]
    public float minGrow = 0.2f;
    [Range(-2,4)]
    public float maxGrow = 0.97f;

    public bool startGrow;
    public bool fullyGrown;

    float growValue;

    public List<MeshRenderer> growVineMeshes;
    private List<Material> growVineMaterials = new List<Material>();

    public AudioSource growplantSound;



    private void Start()
    {
        //add the list of materials
        for (int i=0; i<growVineMeshes.Count; i += 1)
        {
            for(int j=0; j<growVineMeshes[i].materials.Length; j++)
            {
                if (growVineMeshes[i].materials[j].HasProperty("_Grow"))
                {
                    growVineMeshes[i].materials[j].SetFloat("_Grow", minGrow);
                    growVineMaterials.Add(growVineMeshes[i].materials[j]);
                }
            }
        }

        //set the bool to false to allow grow only once
        startGrow = false;


    }

    private void Update()
    {
   
        if (startGrow)
        {
            growplantSound.Play();

            for (int i = 0; i < growVineMaterials.Count; i++)
            {
                StartCoroutine(StartGrow(growVineMaterials[i]));
            }
            startGrow = false;
        }


        if (growValue >= maxGrow)
        {
            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;
        }
    }

    IEnumerator StartGrow(Material mat)
    {
        float growValue = mat.GetFloat("_Grow");
        if (!fullyGrown)
        {
            while (growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreashRate);
                mat.SetFloat("_Grow", growValue);
                yield return new WaitForSeconds(refreashRate);
            }
        }

    }


}

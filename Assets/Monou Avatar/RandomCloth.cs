using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using System.IO;

public class RandomCloth : MonoBehaviour
{

    private DynamicCharacterAvatar avatar;
    private bool beingHandled = false;
    private float probability = 0;
    private Dictionary<string, List<UMATextRecipe>> recipes;
    private float angle = 180f;

    // Start is called before the first frame update
    void Start() {
        avatar = GetComponent<DynamicCharacterAvatar>();
        recipes = UMAContextBase.Instance.GetRecipes("Monou");
        probability = 1.0f/recipes.Count;
        print(probability);
    }

    // Update is called once per frame
    void Update() {
        if( !beingHandled ) StartCoroutine( HandleIt() );
        //angle += 5f;
        //transform.rotation = Quaternion.Euler(0, angle, 0);
    }


    private IEnumerator HandleIt(){
        beingHandled = true;
        yield return new WaitForSeconds( 2.0f );
//        avatar.SetColor("Skin", new Color(UnityEngine.Random.Range(0f, 1f),UnityEngine.Random.Range(0f, 1f),UnityEngine.Random.Range(0f, 1f),1));
        avatar.ClearSlots();
        foreach(var cloths in recipes)
            foreach(var cloth in cloths.Value){
                var number = UnityEngine.Random.Range(0.0f, 1.0f);
                if(number > probability) avatar.SetSlot(cloth);
            }
/*        Dictionary<string, DnaSetter> AllDNA = avatar.GetDNA();
        AllDNA["Gender"].Set(UnityEngine.Random.Range(0.0f, 1.0f));
        AllDNA["Height"].Set(UnityEngine.Random.Range(0.0f, 1.0f));
        AllDNA["Weight"].Set(UnityEngine.Random.Range(0.0f, 1.0f));
        //foreach(KeyValuePair<string, DnaSetter> entry in AllDNA)
        //    Debug.Log(entry.Key);
*/
        avatar.BuildCharacter();    
        beingHandled = false;
    }
}

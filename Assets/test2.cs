using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UMA;
using UMA.CharacterSystem;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine(_loadInitialData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator _loadInitialData(){
        yield return new WaitForSeconds(5.01F);
        var avatar = GetComponent<DynamicCharacterAvatar>();
        avatar.UpdateSameRace();
    }
}

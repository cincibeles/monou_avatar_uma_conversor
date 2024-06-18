using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UMA;
using UMA.CharacterSystem;

public class test : MonoBehaviour
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
        yield return new WaitForSeconds(1.01F);
        var umaContext = UMAContext.Instance;
        var avatar = GetComponent<DynamicCharacterAvatar>();
        var ud = avatar.umaData; //GetComponent<UMAData>();
        foreach (SlotData slot in ud.umaRecipe.slotDataList) {
            if (!slot || slot.slotName!="MaleTorso") continue;
            //umaContext.overlayLibrary.InstantiateOverlay("Thracian Helmet", new Color(0.2f, 0.2f, 0.7f, 1f))
            var overlay = umaContext.InstantiateOverlay("testOverlay");
            slot.AddOverlay(overlay);
            //slot.SetOverlay(1, overlay);
            var list = slot.GetOverlayList();
            for(int c=0; c<list.Count; c++){
                print(list[c].asset.name);
                //list[c].asset.save();
            }
            print(slot.asset.name);
            //slot.asset.LoadFromIndex();
        }
    //GetComponent<DynamicCharacterAvatar>().SetSlot(slot);

        //GetComponent<DynamicCharacterAvatar>().ForceUpdate(false,false,falSetOverlay);
        yield return new WaitForSeconds(1.01F);
        avatar.UpdateSameRace();
    }
}

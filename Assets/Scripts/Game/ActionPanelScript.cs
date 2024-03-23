using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelScript : MonoBehaviour
{
    public GameObject[] slots;
    public GameObject[] targets;

    public int activeSlotIndex = -1;
    public List<int> targetsIndexes = new();

    public Sprite targetOffSprite, targetOnSprite;

    private void Awake() {
        foreach (var slot in slots) {
            slot.gameObject.SetActive(false);
        }
        foreach (var target in targets) {
            target.gameObject.SetActive(false);
        }
    }

    public void OpenSlot(int index){
        foreach (var slot in slots) {
            slot.gameObject.SetActive(false);
        }
        activeSlotIndex = index;
        slots[index].gameObject.SetActive(true);
    }

    public void CloseSlots(){
        foreach (var slot in slots) {
            slot.gameObject.SetActive(false);
        }
        activeSlotIndex = -1;
    }

    public void OpenTarget(int index){  
        print("open target" + index);
        targetsIndexes.Add(index);
        print(targets[index].gameObject.activeSelf);
        targets[index].gameObject.SetActive(true);
        print(targets[index].gameObject.activeSelf);

        targets[index].GetComponent<Image>().sprite = targetOffSprite;
    }

    public void ActivateTarget(int index){
        targets[index].GetComponent<Image>().sprite = targetOnSprite;
        targets[index].GetComponent<Animator>().SetTrigger("activate");
    }
}

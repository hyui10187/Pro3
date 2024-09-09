using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantSlot : MonoBehaviour
{
    public int slotNum; // 아이템을 가져온 인벤토리의 슬롯 번호
    public Item item;
    public Image itemImage;
    public TextMeshProUGUI itemCount; // 보유한 아이템 갯수
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{

    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D target;
    public float rad;

    private void FixedUpdate()
    {
        target = Physics2D.CircleCast(transform.position, scanRange, Vector2.zero, 0, targetLayer);
    }
    
    private void OnDrawGizmos() // 피격범위 박스를 에디터에서 표시하기 위한 메소드
    {
        Gizmos.color = Color.red; // 박스의 색상은 빨간색으로
        Gizmos.DrawWireSphere(transform.position, rad);
        //Gizmos.DrawWireCube(transform.position + dirVec, boxSize);
    }
    
}
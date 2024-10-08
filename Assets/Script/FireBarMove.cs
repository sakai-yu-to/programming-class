using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBarMove : MonoBehaviour
{
    public float rotationSpeed = 100f; // 回転速度（インスペクターで変更可能）
    public bool isClockwise = false;   // trueなら時計回り、falseなら半時計回り

    // Update is called once per frame
    void Update()
    {
        // 回転方向を切り替える
        float direction = isClockwise ? -1f : 1f;

        // 回転の実行
        transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
    }
}

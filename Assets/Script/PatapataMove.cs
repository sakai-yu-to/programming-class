using UnityEngine;

public class PatapataMovement : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public float activationDistance = 5f; // プレイヤーがこれ以上近づいたら動き始める距離
    public float speed = 2f; // 移動速度
    public float height = 2f; // 上下の移動範囲
    public Sprite upwardSprite; // 上方向移動時のスプライト
    public Sprite downwardSprite; // 下方向移動時のスプライト

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool movingUp = true;
    private bool isActivated = false; // 動き始めたかどうかのフラグ

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRendererコンポーネントを取得
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2Dコンポーネントを取得
        startPos = transform.position; // 初期位置を保存
        rb.gravityScale = 0; // 重力を無効化
    }

    void Update()
    {
        // プレイヤーとの距離を測定
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 一定距離以内にプレイヤーが近づいたら上下運動を開始
        if (distanceToPlayer <= activationDistance)
        {
            isActivated = true;
        }

        // 上下運動をする
        if (isActivated)
        {
            if (movingUp)
            {
                rb.velocity = new Vector2(0, speed); // 上方向に移動
                if (transform.position.y >= startPos.y + height)
                {
                    movingUp = false;
                }
            }
            else
            {
                rb.velocity = new Vector2(0, -speed); // 下方向に移動
                if (transform.position.y <= startPos.y - height)
                {
                    movingUp = true;
                }
            }

            // 移動方向に応じてスプライトを変更
            if (rb.velocity.y > 0)
            {
                spriteRenderer.sprite = upwardSprite; // 上方向移動中のスプライトに変更
            }
            else if (rb.velocity.y < 0)
            {
                spriteRenderer.sprite = downwardSprite; // 下方向移動中のスプライトに変更
            }
        }
    }
}

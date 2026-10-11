using TMPro;
using UnityEngine;

public class EnemyObari : EnemyBase
{
    [SerializeField] private TMP_Text m_debugText;
    //発動条件
    //そのステージのどこかを経由（できるだけ休憩エリアの近く）
    //最短ルートで行くようにはしない　できるだけ時間を設ける
    //メインは移動速度妨害
    //放置しすぎると死ぬ=>ごり押しをさける
    //ステージ移行時に引き継ぎ、かつ　Playerがおばりよんを持ってる間　敵は出現しない
    //オバリヨンは２体

    //必要なもの
    //OnTriggerで検知
    //Plaeyrを取得

    //Playerに新しくClassを持たせる？
    //TimerはPlayerのほうで
    //
    protected override void Init()
    {
        
    }

    private void Update()
    {
        m_debugText.text = $"{gameObject.name}";
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<PlayerController>();

        if(target == null) return;

        target.SetObri();
    }
}

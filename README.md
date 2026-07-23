# The Marble

Unity・C# で個人開発した、最大 8人対戦 のオンラインレースゲームです。
ビー玉(マーブル)を操作し、ギミックだらけの全 6ステージ を走り抜けて順位を競います。
企画・ステージ設計・実装・通信同期まで、すべて一人で制作しました。

## プレイ動画

<video src="Assets/Movie/themarble_demo.mp4" controls width="100%"></video>

※オンライン対戦のため、動作確認には複数クライアントの起動が必要です。まずは動画をご覧ください。

## プレイURL

https://unityroom.com/games/themarble

## スクリーンショット

### タイトル画面

<img width="960" height="540" alt="shotcut1" src="https://github.com/user-attachments/assets/f5ac52db-ece2-4478-8291-3957bc5d4dc2" />

### レース中

<img width="960" height="540" alt="shotcut3" src="https://github.com/user-attachments/assets/170d8b54-73e6-46cc-b804-c192d0b4d63c" />



## 主な機能

- オンライン対戦(最大8人): Photon PUN2 によるルームマッチング・リアルタイム同期
- 全6ステージのレース進行: 全プレイヤーのステージ遷移とレース開始タイミングを同期
- 多彩なギミック: ファン、ブースター、ローラー、コイン、ポイント移動式の障害物など
- リザルト・ランキング表示、ポーズ、BGM/SE 管理

## こだわった点

### 1. 全プレイヤーの「よーいドン」を揃える同期設計

複数ステージを進行する際、全プレイヤーのステージ遷移と開始タイミングを一致させる必要がありました。
各プレイヤーの状態をPhotonのカスタムプロパティで管理し、全員の完了を確認してからレースを開始する仕組みを、公式ドキュメントや技術記事を調べながら自作しています。

同期処理の中核: `Assets/Scripts/GameManager.cs`、`Assets/Scripts/Spawner.cs`、`Assets/Scripts/IntervalManager.cs` など

### 2. 破綻しない構造を先に考える

過去に設計をせずに書き始めてコードが破綻し、完成できなかった反省から、本作ではクラスの役割分担(単一責任)を意識して構造を先に設計してから実装しました。

- 役割別のマネージャー群: `Assets/Scripts/Manager/`(入力・音・カメラ演出・ポーズ等を分離)
- ギミックの共通化: `Assets/Scripts/Obstacles/`(インターフェース `I_VelocityAdder` による速度付与の抽象化)

## 使用技術

- Unity / C#
- Photon PUN2(オンライン同期)
- DOTween(UI・演出アニメーション)
- UniRx(リアクティブなUIイベント処理)

## 操作方法

| 操作 | キー / デバイス |
|---|---|
| 移動 | WASD または矢印キー |
| 視点操作 | マウス |
| ジャンプ | Space |
| ポーズ | ホーム画面の歯車ボタン(UIクリック) |

## クレジット

- BGM: [DOVA-SYNDROME](https://dova-s.jp/)
- SE: [効果音ラボ](https://soundeffect-lab.info/)

(利用規約に基づき、音源ファイルはリポジトリに含めていません)


4つの図は：

フローチャート (graph TD) — 全体構成・モジュール間の依存関係
クラス図 (classDiagram) — クラスの構造と関係
シーケンス図 (sequenceDiagram) — 時系列の処理の流れ（誰が誰を呼んで、何が返ってくるか）
状態遷移図 (stateDiagram) — 状態管理（どの状態からどの状態に遷移するか）

```{mermaid}
graph TD
    DunGen["DunGen<br/>迷宮生成"]
    ProBuilder["ProBuilder<br/>部屋ジオメトリ"]
    DirtSpawner["DirtSpawner<br/>汚れ配置"]
    Player["Player<br/>移動・操作"]
    CleaningSystem["CleaningSystem<br/>汚れ除去"]
    
    DunGen --> ProBuilder
    DunGen -->|"汚れ位置情報"| DirtSpawner
    DirtSpawner --> Player
    Player -->|"クリック検知"| CleaningSystem

```{mermaid}


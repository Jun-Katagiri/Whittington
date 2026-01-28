using UnityEngine;
using UnityEngine.InputSystem; // 新システムを使用
using System.Linq;

public class ObjectInteractionModern : MonoBehaviour
{
    public AudioSource soundEffect;
    public Animator animatorObject;
    public bool enableHighlight = false;
    public Color highlightColor = Color.yellow;
    public string animationName = "Interaction Animation";

    [Header("Interaction Settings")]
    public float cooldownDuration = 0f;
    private float lastInteractionTime = -999f;

    private Material[] materials;
    private bool isHovered = false;

    void Start()
    {
        // 全ての子要素からマテリアルを取得
        materials = GetComponentsInChildren<Renderer>().SelectMany(r => r.materials).ToArray();
        SetHighlight(false);
    }

    void Update()
    {
        if (Camera.main == null || Mouse.current == null)
        {
            if (isHovered) OnHoverExit();
            return;
        }

        // 1. マウス位置からレイ（光線）を飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 自分に当たっているか？
            if (hit.collider.gameObject == gameObject)
            {
                if (!isHovered)
                {
                    OnHoverEnter();
                }
                // 2. 左クリックされた瞬間を検知
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (Time.time >= lastInteractionTime + cooldownDuration)
                    {
                        PerformAction();
                        lastInteractionTime = Time.time;
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " はまだ準備中です...");
                    }
                }
            }
            else if (isHovered)
            {
                OnHoverExit();
            }
        }
        else if (isHovered)
        {
            OnHoverExit();
        }
    }

    void OnHoverEnter()
    {
        if (!enableHighlight) return;
        isHovered = true;
        SetHighlight(true);
    }

    void OnHoverExit()
    {
        if (!enableHighlight) return;
        isHovered = false;
        SetHighlight(false);
    }

    void PerformAction()
    {
        // アニメーション再生
        if (animatorObject != null)
        {
            animatorObject.Play(animationName, -1, 0f);
        }
        // 音再生（ピッチ変更付き）
        if (soundEffect != null)
        {
            soundEffect.pitch = Random.Range(0.9f, 1.1f);
            soundEffect.Play();
        }
    }

    void SetHighlight(bool active)
    {
        if (materials == null || materials.Length == 0) return;
        Color c = active ? highlightColor : Color.black;
        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", c);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class UrinalFlushController : MonoBehaviour, Interactable
{
    [Header("Interaction")]
    [SerializeField] float cooldown = 3f;
    [SerializeField] string label = "Flush";

    [Header("Feedback")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem flushParticles;
    [SerializeField] ParticleSystem drainageParticles;
    [SerializeField] Vector2 pitchRange = new(0.95f, 1.05f);

    float lastFlushTime = -Mathf.Infinity;
    List<Command> commands;

    // Lightweight Expression 1: Expression-bodied property for logic
    bool IsOnCooldown => Time.time < lastFlushTime + cooldown;

    void Awake()
    {
        lastFlushTime = -Mathf.Infinity; // Ensure it is ready to flush immediately
        EnsureCommands();
    }

    // Lightweight Expression 3: Expression-bodied methods for interface implementation
    public List<Command> GetCommands()
    {
        EnsureCommands();
        return commands;
    }

    public Command GetPrimaryCommand()
    {
        EnsureCommands();
        return commands[0];
    }

    public void ExecuteCommand(Command command)
    {
        // Lightweight Expression 4: Pattern matching (checks null, type, and property in one go)
        if (command is { kind: Command.Kind.Toggle } && !IsOnCooldown)
        {
            PerformFlush();
        }
    }

    void PerformFlush()
    {
        lastFlushTime = Time.time;

        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audioSource.Play();
        }

        if (flushParticles != null)
            flushParticles.Play();
        else
            Debug.LogWarning($"[UrinalFlushController] Flush particles missing on {name}. Right-click component to 'Create Flush Particles'.");

        if (drainageParticles != null)
            drainageParticles.Play();
    }

    void EnsureCommands()
    {
        if (commands == null || commands.Count == 0)
        {
            // Lightweight Expression 2: Collection initializer
            commands = new List<Command>
            {
                new Command { kind = Command.Kind.Toggle, label = label }
            };
        }
    }

    void Reset()
    {
        if (flushParticles == null)
            flushParticles = GetComponentInChildren<ParticleSystem>();
    }

    [ContextMenu("Create Flush Particles")]
    void CreateFlushParticles()
    {
        if (flushParticles != null && drainageParticles != null)
        {
            Debug.LogWarning("Particle Systems already assigned.");
            return;
        }

        // Shared Material Setup
        Material mat = null;
        var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shader != null)
        {
            mat = new Material(shader);
            mat.name = "Procedural_Water_Mat";
            mat.SetFloat("_Surface", 1.0f); // Transparent
            mat.SetFloat("_Blend", 0.0f);   // Alpha
            mat.SetFloat("_ZWrite", 0.0f);  // Off
            mat.renderQueue = 3000;
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.SetOverrideTag("RenderType", "Transparent");
        }

        if (flushParticles == null)
        {
            var go = new GameObject("Flush_FX");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, 1.2f, 0.15f); // High up, slightly forward
            go.transform.localRotation = Quaternion.Euler(90, 0, 0); // Downwards

            var ps = go.AddComponent<ParticleSystem>();
            var renderer = go.GetComponent<ParticleSystemRenderer>();
            if (mat != null) renderer.material = mat;

            // Fix for "Low Poly" / "Rough" look: Stretch particles to look like liquid streams
            renderer.renderMode = ParticleSystemRenderMode.Stretch;
            renderer.lengthScale = 3.0f;
            renderer.velocityScale = 0.1f;

            var main = ps.main;
            main.duration = 2.0f;
            main.loop = false;
            main.startLifetime = 0.8f;
            main.startSpeed = 3f;
            main.startSize = 0.04f;
            main.startColor = new Color(0.6f, 0.6f, 1f, 0.2f); // More transparent
            main.playOnAwake = false;

            var emission = ps.emission;
            emission.rateOverTime = 200;

            // Fix for "Spread"
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(0.25f, 0.01f, 0.01f); // Wide emission line

            flushParticles = ps;
        }

        if (drainageParticles == null)
        {
            var go = new GameObject("Drainage_Swirl_FX");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, 0.5f, 0.2f); // Lower, near drain
            go.transform.localRotation = Quaternion.Euler(-90, 0, 0); // Facing up

            var ps = go.AddComponent<ParticleSystem>();
            var renderer = go.GetComponent<ParticleSystemRenderer>();
            if (mat != null) renderer.material = mat;

            var main = ps.main;
            main.duration = 2.0f;
            main.loop = false;
            main.startLifetime = 1.0f;
            main.startSpeed = 0.0f; // Driven by orbital velocity
            main.startSize = 0.05f;
            main.startColor = new Color(0.6f, 0.6f, 1f, 0.3f);
            main.playOnAwake = false;

            var emission = ps.emission;
            emission.rateOverTime = 100;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;

            var vel = ps.velocityOverLifetime;
            vel.enabled = true;
            vel.orbitalZ = 10.0f; // Swirl effect

            drainageParticles = ps;
        }
    }
}
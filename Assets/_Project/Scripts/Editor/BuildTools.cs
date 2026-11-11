using System;
using System.IO;
using System.Linq;
using IAmAsIAm.Runtime;
using IAmAsIAm.Runtime.Mind;
using IAmAsIAm.Runtime.Story;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace IAmAsIAm.EditorTools
{
    /// <summary>
    /// Everything needed to go from a fresh checkout to an APK, with no hand-made assets:
    ///   IAmAsIAm → Compile Story       (Ink → Story/main.json, synchronously)
    ///   IAmAsIAm → Configure Project   (URP asset, Android IL2CPP ARM64, APK not AAB)
    ///   IAmAsIAm → Rebuild Boot Scene  (generated: never hand-edit Scenes/Boot.unity)
    ///   IAmAsIAm → Build Android APK   (all of the above, then build)
    /// CI (GitHub Actions → Build Android) calls BuildTools.BuildAndroid.
    /// </summary>
    public static class BuildTools
    {
        const string StoryDir = "Assets/_Project/Story";
        const string StoryJson = StoryDir + "/main.json";
        const string ScenePath = "Assets/_Project/Scenes/Boot.unity";
        const string SettingsDir = "Assets/_Project/Settings";
        const string PlaceholderDir = "Assets/_Project/Art/Placeholders";

        // ── Story ────────────────────────────────────────────────────────────────

        [MenuItem("IAmAsIAm/Compile Story")]
        public static void CompileStory()
        {
            string root = Path.GetFullPath(StoryDir);
            bool failed = false;
            var compiler = new Ink.Compiler(File.ReadAllText(Path.Combine(root, "main.ink")), new Ink.Compiler.Options
            {
                sourceFilename = "main.ink",
                fileHandler = new StoryFiles(root),
                errorHandler = (msg, type) =>
                {
                    if (type == Ink.ErrorType.Error) { failed = true; Debug.LogError("[Ink] " + msg); }
                    else Debug.LogWarning("[Ink] " + msg);
                },
            });
            var story = compiler.Compile();
            if (failed || story == null) throw new BuildFailedException("The Ink story has errors (see the Console).");
            File.WriteAllText(Path.GetFullPath(StoryJson), story.ToJson());
            AssetDatabase.ImportAsset(StoryJson, ImportAssetOptions.ForceUpdate);
            Debug.Log("[IAmAsIAm] Story compiled to " + StoryJson);
        }

        sealed class StoryFiles : Ink.IFileHandler
        {
            readonly string _root;
            public StoryFiles(string root) => _root = root;
            public string ResolveInkFilename(string includeName) => Path.Combine(_root, includeName);
            public string LoadInkFileContents(string fullFilename) => File.ReadAllText(fullFilename);
        }

        // ── Project ──────────────────────────────────────────────────────────────

        [MenuItem("IAmAsIAm/Configure Project")]
        public static void ConfigureProject()
        {
            Directory.CreateDirectory(SettingsDir);
            string rpPath = SettingsDir + "/URP.asset";
            var rp = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(rpPath);
            if (rp == null)
            {
                var renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
                AssetDatabase.CreateAsset(renderer, SettingsDir + "/URP_Renderer.asset");
                rp = UniversalRenderPipelineAsset.Create(renderer);
                AssetDatabase.CreateAsset(rp, rpPath);
            }
            rp.msaaSampleCount = 4; // Seven Witches found 4x MSAA necessary for clean edges.
            EditorUtility.SetDirty(rp);

            GraphicsSettings.defaultRenderPipeline = rp;
            int current = QualitySettings.GetQualityLevel();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                QualitySettings.SetQualityLevel(i, false);
                QualitySettings.renderPipeline = rp;
            }
            QualitySettings.SetQualityLevel(current, false);

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            EditorUserBuildSettings.buildAppBundle = false; // A sideloadable .apk.

            AssetDatabase.SaveAssets();
            Debug.Log("[IAmAsIAm] Project configured (URP, Android IL2CPP ARM64, landscape, APK).");
        }

        // ── Scene ────────────────────────────────────────────────────────────────

        [MenuItem("IAmAsIAm/Rebuild Boot Scene")]
        public static void RebuildBootScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cam = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener)) { tag = "MainCamera" };
            cam.transform.SetPositionAndRotation(new Vector3(0f, 1.6f, -3.2f), Quaternion.Euler(8f, 0f, 0f));
            cam.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            cam.GetComponent<Camera>().backgroundColor = new Color(0.08f, 0.07f, 0.1f);

            var sun = new GameObject("Window Light", typeof(Light));
            sun.GetComponent<Light>().type = LightType.Directional;
            sun.GetComponent<Light>().color = new Color(1f, 0.86f, 0.7f);
            sun.transform.rotation = Quaternion.Euler(40f, -30f, 0f);

            // A placeholder kitchen, coloured blocks until the Grab3D/Nomad models arrive.
            Block("Floor", new Vector3(0, -0.05f, 0), new Vector3(6, 0.1f, 6), "mat_floor", new Color(0.45f, 0.38f, 0.3f));
            Block("Back Wall", new Vector3(0, 1.4f, 3), new Vector3(6, 2.8f, 0.1f), "mat_wall", new Color(0.78f, 0.72f, 0.62f));
            Block("Counter", new Vector3(-1.4f, 0.45f, 2.4f), new Vector3(2.4f, 0.9f, 0.9f), "mat_wood", new Color(0.52f, 0.36f, 0.24f));
            Block("Kettle", new Vector3(-1.2f, 1.05f, 2.3f), new Vector3(0.25f, 0.3f, 0.25f), "mat_kettle", new Color(0.7f, 0.25f, 0.2f));
            Block("Table", new Vector3(1.1f, 0.38f, 1.2f), new Vector3(1.2f, 0.76f, 0.8f), "mat_wood", new Color(0.52f, 0.36f, 0.24f));

            var game = new GameObject("Game");
            game.AddComponent<GameBootstrap>();
            game.AddComponent<MindStateController>();
            var runner = game.AddComponent<StoryRunner>();
            var screen = game.AddComponent<StoryScreen>();

            var story = AssetDatabase.LoadAssetAtPath<TextAsset>(StoryJson);
            if (story == null) Debug.LogWarning("[IAmAsIAm] Story/main.json is missing. Run IAmAsIAm → Compile Story.");
            Set(runner, "compiledStory", story);
            Set(screen, "runner", runner);

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            Debug.Log("[IAmAsIAm] Boot scene rebuilt.");
        }

        static void Block(string name, Vector3 pos, Vector3 scale, string matName, Color color)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = PlaceholderMaterial(matName, color);
        }

        static Material PlaceholderMaterial(string name, Color color)
        {
            Directory.CreateDirectory(PlaceholderDir);
            string path = $"{PlaceholderDir}/{name}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null) return mat; // Never overwrite an existing material (the artist may have changed it).
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            mat = new Material(shader);
            mat.SetColor("_BaseColor", color);
            mat.color = color;
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        static void Set(UnityEngine.Object target, string field, UnityEngine.Object value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(field).objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // ── Builds ───────────────────────────────────────────────────────────────

        [MenuItem("IAmAsIAm/Build Android APK")]
        public static void BuildAndroidFromMenu() => Build(BuildTarget.Android, "build/Android/IAmAsIAm.apk", exitOnFinish: false);

        /// <summary>Entry point for CI (game-ci/unity-builder buildMethod).</summary>
        public static void BuildAndroid() => Build(BuildTarget.Android, Arg("-customBuildPath") ?? "build/Android/IAmAsIAm.apk", exitOnFinish: true);

        /// <summary>Entry point for CI: Windows.</summary>
        public static void BuildWindows() => Build(BuildTarget.StandaloneWindows64, Arg("-customBuildPath") ?? "build/Windows/IAmAsIAm.exe", exitOnFinish: true);

        static void Build(BuildTarget target, string path, bool exitOnFinish)
        {
            int code = 0;
            try
            {
                var issues = IAmAsIAm.Core.DataValidator.Validate(DataValidationMenu.LoadBundle());
                if (issues.Count > 0) throw new BuildFailedException("Content validation failed:\n" + string.Join("\n", issues));

                CompileStory();
                ConfigureProject();
                RebuildBootScene();

                if (target == BuildTarget.Android && !path.EndsWith(".apk")) path = Path.Combine(path, "IAmAsIAm.apk");
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
                var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                    locationPathName = path,
                    target = target,
                    options = BuildOptions.None,
                });
                Debug.Log($"[IAmAsIAm] Build {report.summary.result}: {path} ({report.summary.totalSize / (1024 * 1024)} MB)");
                if (report.summary.result != BuildResult.Succeeded) code = 1;
            }
            catch (Exception e)
            {
                Debug.LogError("[IAmAsIAm] Build failed: " + e);
                code = 1;
            }
            if (exitOnFinish) EditorApplication.Exit(code);
        }

        static string Arg(string name)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
    }
}

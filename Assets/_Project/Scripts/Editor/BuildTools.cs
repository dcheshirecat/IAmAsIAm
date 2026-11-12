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
        const string TarotArtDir = "Assets/_Project/Art/Tarot";

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
            SetAppIcon();

            AssetDatabase.SaveAssets();
            Debug.Log("[IAmAsIAm] Project configured (URP, Android IL2CPP ARM64, landscape, APK).");
        }

        /// <summary>
        /// Uses Art/UI/app_icon.png as the default icon for every platform. It's a temporary "TEST" icon; the artist
        /// replaces it by overwriting the file (same name, 1024 × 1024, keep the important part in the middle ~70%).
        /// </summary>
        static void SetAppIcon()
        {
            const string iconPath = "Assets/_Project/Art/UI/app_icon.png";
            if (!File.Exists(iconPath)) { Debug.LogWarning("[IAmAsIAm] No app icon at " + iconPath); return; }
            AssetDatabase.ImportAsset(iconPath, ImportAssetOptions.ForceUpdate);
            if (AssetImporter.GetAtPath(iconPath) is TextureImporter importer &&
                (importer.textureCompression != TextureImporterCompression.Uncompressed || importer.mipmapEnabled))
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.SaveAndReimport();
            }
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);
            if (icon == null) { Debug.LogWarning("[IAmAsIAm] Couldn't load the app icon."); return; }
            PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { icon }, IconKind.Any);

            // Android: set every icon kind (legacy, round, adaptive), so every launcher shows it instead of Unity's default.
            // Adaptive icons get the same picture on both layers: its important part is in the middle, inside the safe zone.
            // Never fails a build over the icon (as in Seven Witches' AppIconSetter).
            try
            {
#pragma warning disable CS0618
                foreach (var kind in PlayerSettings.GetSupportedIconKindsForPlatform(BuildTargetGroup.Android))
                {
                    var icons = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, kind);
                    foreach (var platformIcon in icons)
                    {
                        if (platformIcon.maxLayerCount >= 2) platformIcon.SetTextures(icon, icon);
                        else platformIcon.SetTexture(icon);
                    }
                    PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, kind, icons);
                }
#pragma warning restore CS0618
            }
            catch (Exception e)
            {
                Debug.LogWarning("[IAmAsIAm] Android icons not set (the default icon is used): " + e.Message);
            }
        }

        // ── Scene ────────────────────────────────────────────────────────────────

        [MenuItem("IAmAsIAm/Rebuild Boot Scene")]
        public static void RebuildBootScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var sun = new GameObject("Window Light", typeof(Light));
            sun.GetComponent<Light>().type = LightType.Directional;
            sun.GetComponent<Light>().color = new Color(1f, 0.86f, 0.7f);
            sun.GetComponent<Light>().shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

            // ── A placeholder flat (coloured blocks until the owner's models arrive) ──
            var flat = new GameObject("Flat (placeholder)").transform;
            Block("Floor", new Vector3(0, -0.05f, 0), new Vector3(9, 0.1f, 7), "mat_floor", new Color(0.45f, 0.38f, 0.3f), flat);
            Block("Wall North", new Vector3(0, 1.4f, 3.5f), new Vector3(9, 2.8f, 0.15f), "mat_wall", new Color(0.78f, 0.72f, 0.62f), flat);
            Block("Wall West", new Vector3(-4.5f, 1.4f, 0), new Vector3(0.15f, 2.8f, 7), "mat_wall", new Color(0.78f, 0.72f, 0.62f), flat);
            Block("Wall East (left of door)", new Vector3(4.5f, 1.4f, 1.9f), new Vector3(0.15f, 2.8f, 3.2f), "mat_wall", new Color(0.78f, 0.72f, 0.62f), flat);
            Block("Wall East (right of door)", new Vector3(4.5f, 1.4f, -2.4f), new Vector3(0.15f, 2.8f, 2.2f), "mat_wall", new Color(0.78f, 0.72f, 0.62f), flat);
            Block("Wall South", new Vector3(0, 1.4f, -3.5f), new Vector3(9, 2.8f, 0.15f), "mat_wall", new Color(0.78f, 0.72f, 0.62f), flat);

            var counter = Block("Kitchen Counter", new Vector3(-3.2f, 0.45f, 2.9f), new Vector3(2.4f, 0.9f, 0.9f), "mat_wood", new Color(0.52f, 0.36f, 0.24f), flat);
            var kettle = Block("Kettle", new Vector3(-3.0f, 1.05f, 2.8f), new Vector3(0.25f, 0.3f, 0.25f), "mat_kettle", new Color(0.7f, 0.25f, 0.2f), flat);
            Story(kettle, "Look", "test_kettle", false);
            var table = Block("Table", new Vector3(0.5f, 0.38f, 1.0f), new Vector3(1.4f, 0.76f, 0.9f), "mat_wood", new Color(0.52f, 0.36f, 0.24f), flat);
            var deck = Block("Tarot Deck", new Vector3(0.4f, 0.79f, 1.0f), new Vector3(0.12f, 0.05f, 0.2f), "mat_deck", new Color(0.25f, 0.2f, 0.45f), flat);
            Story(deck, "Draw a card", "test_tarot", false);
            var altar = Block("Altar Shelf", new Vector3(-4.1f, 0.6f, -1.5f), new Vector3(0.6f, 1.2f, 1.4f), "mat_altar", new Color(0.3f, 0.28f, 0.25f), flat);
            Block("Candle", new Vector3(-4.0f, 1.28f, -1.3f), new Vector3(0.08f, 0.16f, 0.08f), "mat_candle", new Color(0.95f, 0.9f, 0.75f), flat);
            Story(altar, "Sit at the altar", "test_altar", false);

            // The stairwell outside the door, with a trigger zone (it does nothing until triggers.json has a matching trigger).
            Block("Landing", new Vector3(6.5f, -0.05f, -0.4f), new Vector3(4, 0.1f, 2.4f), "mat_floor_landing", new Color(0.35f, 0.35f, 0.38f), flat);
            var zone = new GameObject("Cue Zone: raised-voices (upstairs flat)", typeof(BoxCollider));
            zone.transform.SetParent(flat);
            zone.transform.position = new Vector3(7f, 1f, -0.4f);
            zone.GetComponent<BoxCollider>().isTrigger = true;
            zone.GetComponent<BoxCollider>().size = new Vector3(2f, 2f, 2.4f);
            var cueZone = zone.AddComponent<IAmAsIAm.Runtime.Triggers.CueZone>();
            SetString(cueZone, "cue", "raised-voices");

            // A door that opens the Prologue's start screen (content warnings, who to start as, or a summary).
            var door = Block("Chapter Door: Prologue (test)", new Vector3(3.6f, 1.05f, 3.2f), new Vector3(1.0f, 2.1f, 0.12f), "mat_door", new Color(0.36f, 0.42f, 0.55f), flat);
            var chapterStart = door.AddComponent<ChapterStart>();
            SetString(chapterStart, "chapterId", "prologue-quaker");
            SetString(chapterStart, "prompt", "Begin the Prologue");

            // A placeholder guide, to test talking.
            var guide = StandIn("Guide (placeholder)", new Vector3(2.2f, 0, -1.2f), new Color(0.35f, 0.5f, 0.6f));
            Story(guide, "Talk", "test_guide", true);

            // ── The character ──
            var player = new GameObject("Player", typeof(CharacterController));
            player.transform.position = new Vector3(-1f, 0f, -1.5f);
            var cc = player.GetComponent<CharacterController>();
            cc.height = 1.7f; cc.radius = 0.28f; cc.center = new Vector3(0, 0.85f, 0);
            AttachVisual(player.transform);
            var motor = player.AddComponent<IAmAsIAm.Runtime.Player.PlayerMotor>();
            var interactor = player.AddComponent<IAmAsIAm.Runtime.Interaction.ProximityInteractor>();

            // ── The camera: close third person, over the shoulder (Pokémon Legends: Z-A style) ──
            var cam = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener)) { tag = "MainCamera" };
            var camera = cam.GetComponent<Camera>();
            camera.fieldOfView = 55f;
            camera.nearClipPlane = 0.05f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.08f, 0.07f, 0.1f);
            var rig = cam.AddComponent<IAmAsIAm.Runtime.CameraRig.ShoulderCamera>();
            Set(rig, "target", player.transform);
            Set(motor, "cameraRig", rig);

            // ── Game systems and the story screen ──
            var game = new GameObject("Game");
            game.AddComponent<GameBootstrap>();
            game.AddComponent<MindStateController>();
            game.AddComponent<DissociationPresentationDriver>();
            var runner = game.AddComponent<StoryRunner>();
            var screen = game.AddComponent<StoryScreen>();
            var saves = game.AddComponent<IAmAsIAm.Runtime.Save.SaveSlotsScreen>();
            Set(saves, "runner", runner);
            Set(saves, "storyScreen", screen);

            // The tarot loading screen (its own object: it outlives the game when switching save slots).
            var loading = new GameObject("Loading Screen").AddComponent<IAmAsIAm.Runtime.Loading.LoadingScreen>();
            WireTarotArt(loading);

            var story = AssetDatabase.LoadAssetAtPath<TextAsset>(StoryJson);
            if (story == null) Debug.LogWarning("[IAmAsIAm] Story/main.json is missing. Run IAmAsIAm → Compile Story.");
            Set(runner, "compiledStory", story);
            Set(screen, "runner", runner);
            Set(screen, "interactor", interactor);
            Set(screen, "cameraRig", rig);

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            Debug.Log("[IAmAsIAm] Boot scene rebuilt.");
        }

        /// <summary>
        /// Gives the loading screen the card art in Art/Tarot: tarot_back.png and tarot_<card-id>.png (ART_LIST.md, Part 6).
        /// Cards without art yet are drawn as placeholders, so any number of faces can arrive one at a time.
        /// </summary>
        static void WireTarotArt(IAmAsIAm.Runtime.Loading.LoadingScreen loading)
        {
            var so = new SerializedObject(loading);
            so.FindProperty("cardBack").objectReferenceValue = TarotSprite("tarot_back");

            var ids = new System.Collections.Generic.List<string>();
            var sprites = new System.Collections.Generic.List<Sprite>();
            var data = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/_Project/Resources/Data/tarot_major.json");
            var cards = data == null ? null : JsonUtility.FromJson<IAmAsIAm.Core.Practices.TarotData>(data.text)?.cards;
            foreach (var card in cards ?? new System.Collections.Generic.List<IAmAsIAm.Core.Practices.TarotCardEntry>())
            {
                var sprite = TarotSprite("tarot_" + card.id);
                if (sprite == null) continue;
                ids.Add(card.id);
                sprites.Add(sprite);
            }

            var idProp = so.FindProperty("faceIds");
            var spriteProp = so.FindProperty("faceSprites");
            idProp.arraySize = ids.Count;
            spriteProp.arraySize = sprites.Count;
            for (int i = 0; i < ids.Count; i++)
            {
                idProp.GetArrayElementAtIndex(i).stringValue = ids[i];
                spriteProp.GetArrayElementAtIndex(i).objectReferenceValue = sprites[i];
            }
            so.ApplyModifiedPropertiesWithoutUndo();
            Debug.Log($"[IAmAsIAm] Loading screen: {sprites.Count} tarot faces{(loading != null && so.FindProperty("cardBack").objectReferenceValue != null ? " and the card back" : "")} from {TarotArtDir}; the rest are placeholders.");
        }

        /// <summary>Loads Art/Tarot/&lt;name&gt;.png as a UI sprite (setting its import type if needed), or null if it isn't there.</summary>
        static Sprite TarotSprite(string name)
        {
            string path = $"{TarotArtDir}/{name}.png";
            if (!File.Exists(path)) return null;
            if (AssetImporter.GetAtPath(path) is TextureImporter importer && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        /// <summary>
        /// The character's look: the owner's Grab3D model if it's in Art/Models/body/model_body.glb,
        /// otherwise a stand-in made of shapes (with a nose, so you can see which way it faces).
        /// </summary>
        static void AttachVisual(Transform player)
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Art/Models/body/model_body.glb");
            if (model != null)
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(model, player);
                instance.name = "Model";
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
                foreach (var col in instance.GetComponentsInChildren<Collider>()) UnityEngine.Object.DestroyImmediate(col);
                return;
            }
            var standIn = StandIn("Stand-in", Vector3.zero, new Color(0.79f, 0.64f, 0.49f));
            standIn.transform.SetParent(player, false);
            foreach (var col in standIn.GetComponentsInChildren<Collider>()) UnityEngine.Object.DestroyImmediate(col);
        }

        /// <summary>A person-shaped stand-in: body, head and a nose that shows facing.</summary>
        static GameObject StandIn(string name, Vector3 feet, Color color)
        {
            var root = new GameObject(name);
            root.transform.position = feet;
            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(root.transform, false);
            body.transform.localPosition = new Vector3(0, 0.75f, 0);
            body.transform.localScale = new Vector3(0.5f, 0.75f, 0.4f);
            body.GetComponent<Renderer>().sharedMaterial = PlaceholderMaterial("mat_" + name.Split(' ')[0].ToLowerInvariant(), color);
            var head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.name = "Head";
            head.transform.SetParent(root.transform, false);
            head.transform.localPosition = new Vector3(0, 1.62f, 0);
            head.transform.localScale = Vector3.one * 0.3f;
            head.GetComponent<Renderer>().sharedMaterial = body.GetComponent<Renderer>().sharedMaterial;
            var nose = GameObject.CreatePrimitive(PrimitiveType.Cube);
            nose.name = "Nose (facing)";
            nose.transform.SetParent(root.transform, false);
            nose.transform.localPosition = new Vector3(0, 1.62f, 0.16f);
            nose.transform.localScale = new Vector3(0.06f, 0.06f, 0.08f);
            UnityEngine.Object.DestroyImmediate(nose.GetComponent<Collider>());
            return root;
        }

        static void Story(GameObject go, string prompt, string knot, bool person)
        {
            var s = go.AddComponent<StoryInteractable>();
            SetString(s, "prompt", prompt);
            SetString(s, "knot", knot);
            var so = new SerializedObject(s);
            so.FindProperty("isPerson").boolValue = person;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void SetString(UnityEngine.Object target, string field, string value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(field).stringValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static GameObject Block(string name, Vector3 pos, Vector3 scale, string matName, Color color, Transform parent)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = PlaceholderMaterial(matName, color);
            return go;
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
                if (target == BuildTarget.Android) ConfigureVersionAndSigning();

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

        /// <summary>
        /// So a new APK installs over the old one, with no uninstall (and the saves kept):
        ///  * every build gets a higher version code (CI passes its run number as -androidVersionCode), and
        ///  * every build is signed with the SAME key. CI gets it from the repo secrets ANDROID_KEYSTORE_BASE64 and
        ///    ANDROID_KEYSTORE_PASS (one key, made once and kept); game-ci writes the keystore
        ///    into the project folder and passes its name and passwords as arguments.
        /// Without a key it still builds, but with a throwaway debug key (then each update needs a reinstall).
        /// </summary>
        static void ConfigureVersionAndSigning()
        {
            string versionArg = Arg("-androidVersionCode") ?? Environment.GetEnvironmentVariable("ANDROID_VERSION_CODE");
            if (int.TryParse(versionArg, out int versionCode) && versionCode > 0)
            {
                PlayerSettings.Android.bundleVersionCode = versionCode;
                PlayerSettings.bundleVersion = "0.1." + versionCode;
            }
            Debug.Log($"[IAmAsIAm] Version {PlayerSettings.bundleVersion} (version code {PlayerSettings.Android.bundleVersionCode}).");

            string name = Arg("-androidKeystoreName");
            string pass = Arg("-androidKeystorePass");
            string alias = Arg("-androidKeyaliasName");
            string aliasPass = Arg("-androidKeyaliasPass");
            string path = string.IsNullOrEmpty(name) ? null : Path.GetFullPath(name);
            if (path != null && File.Exists(path) && !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(alias))
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = path;
                PlayerSettings.Android.keystorePass = pass;
                PlayerSettings.Android.keyaliasName = alias;
                PlayerSettings.Android.keyaliasPass = string.IsNullOrEmpty(aliasPass) ? pass : aliasPass;
                Debug.Log("[IAmAsIAm] Signing with the project's own key (updates install over the old app).");
            }
            else
            {
                PlayerSettings.Android.useCustomKeystore = false;
                Debug.LogWarning("[IAmAsIAm] No signing key given: using a throwaway debug key. Installing this build over an " +
                                 "older one will fail. Add the repo secrets ANDROID_KEYSTORE_BASE64 and ANDROID_KEYSTORE_PASS.");
            }
        }

        static string Arg(string name)
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, name);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
    }
}

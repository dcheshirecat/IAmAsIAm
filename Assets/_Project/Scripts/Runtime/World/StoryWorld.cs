using System;
using System.Collections.Generic;
using IAmAsIAm.Runtime.Interaction;
using UnityEngine;

namespace IAmAsIAm.Runtime.World
{
    /// <summary>
    /// The world the story is explored in (owner, 2026-11-12: "I want to explore the world in this game, not just interact
    /// with dialogue"). Every <c># place: &lt;id&gt;</c> becomes a space the character walks around in, and the story's
    /// explore choices become things IN it: <c>Look:</c> / <c>Use:</c> objects and <c>Talk:</c> people glow softly with a
    /// small label, and <c>Go:</c> is a doorway. The player walks up and uses them (E / A / the Use button).
    ///
    /// Places are PLACEHOLDERS built in code from the place id (indoors or out, small or large, a few props by keyword),
    /// so every chapter is explorable before the owner's art arrives. A thing keeps its spot for as long as the character
    /// stays in that place, and stays there as scenery (no glow) once it has nothing more to offer.
    /// Hidden memories follow the owner's rule by themselves: a thing only exists while the story offers it.
    /// </summary>
    public sealed class StoryWorld : MonoBehaviour
    {
        /// <summary>Places are built far from the test flat, so the two never overlap.</summary>
        static readonly Vector3 Origin = new Vector3(0f, 0f, 400f);

        Transform _player;
        Action _snapCamera;
        Material _template;
        readonly Dictionary<Color, Material> _materials = new Dictionary<Color, Material>();

        GameObject _root, _testFlat;
        string _place;
        Vector2 _half;
        bool _outdoors;
        Vector3 _returnPosition;
        Quaternion _returnRotation;
        bool _away;
        Color _cameraColourWas;

        readonly Dictionary<string, ChoiceSpot> _spots = new Dictionary<string, ChoiceSpot>();
        int _wallSlots, _innerSlots, _doorSlots;

        /// <summary>Is this place one of Maya's homes, with her altar (places.json <c>altar</c>)? Set by the story screen.</summary>
        public Func<string, bool> HasAltar = _ => false;
        /// <summary>Can the altar be used right now (while exploring)? And what happens when it is. Set by the story screen.</summary>
        public Func<bool> AltarUsable = () => false;
        public Action AltarUsed;

        /// <summary>The place the character is in, or null in the test flat.</summary>
        public string Place => _place;

        public void Init(Transform player, Action snapCamera)
        {
            _player = player;
            _snapCamera = snapCamera;
            _testFlat = GameObject.Find("Flat (placeholder)");
            // A material that's certainly in the build (the flat's own), copied per colour.
            var r = _testFlat != null ? _testFlat.GetComponentInChildren<MeshRenderer>() : FindAnyObjectByType<MeshRenderer>();
            _template = r != null ? r.sharedMaterial : null;
        }

        // ── Places ────────────────────────────────────────────────────────────

        /// <summary>Builds the place (if it isn't the one already here) and puts the character at its way in.</summary>
        public void Enter(string placeId)
        {
            if (string.IsNullOrEmpty(placeId) || placeId == _place) return;
            Clear();
            _place = placeId;
            if (!_away)
            {
                _away = true;
                if (_player != null) { _returnPosition = _player.position; _returnRotation = _player.rotation; }
                if (Camera.main != null) _cameraColourWas = Camera.main.backgroundColor;
            }
            if (_testFlat != null) _testFlat.SetActive(false);
            Build(placeId);
            if (placeId != "black" && HasAltar != null && HasAltar(placeId)) BuildAltar();
            Teleport(Origin + new Vector3(0f, 0.05f, -_half.y + 1.2f), Quaternion.identity);
        }

        /// <summary>Back to the flat (the story has ended).</summary>
        public void Leave()
        {
            if (!_away) return;
            Clear();
            _away = false;
            if (_testFlat != null) _testFlat.SetActive(true);
            if (Camera.main != null) Camera.main.backgroundColor = _cameraColourWas;
            Teleport(_returnPosition, _returnRotation);
        }

        void Clear()
        {
            if (_root != null) Destroy(_root);
            _root = null;
            _place = null;
            _spots.Clear();
            _wallSlots = _innerSlots = _doorSlots = 0;
        }

        void Teleport(Vector3 position, Quaternion rotation)
        {
            if (_player == null) return;
            var cc = _player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            _player.SetPositionAndRotation(position, rotation);
            if (cc != null) cc.enabled = true;
            _snapCamera?.Invoke();
        }

        static bool Has(string id, params string[] words)
        {
            foreach (var w in words) if (id.Contains(w)) return true;
            return false;
        }

        void Build(string id)
        {
            _root = new GameObject("Place: " + id);
            _root.transform.position = Origin;
            var rng = new System.Random(Hash(id));
            Color Tint(Color c, float amount) => Color.Lerp(c, new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()), amount);

            bool black = id == "black";
            _outdoors = Has(id, "park", "quad", "lake", "path", "march", "pride", "lawn", "street", "protest", "garden", "yard", "outside", "field", "cemetery", "parking", "stupa", "sacred_garden");
            bool large = Has(id, "church", "synagogue", "temple", "hall", "shul", "meeting", "chapel", "sanctuary", "ministry", "gallery", "museum", "renewal", "lecture", "school", "monastery", "airport", "mosque", "dhikr", "group_room");
            bool small = Has(id, "car", "bathroom", "closet", "guest_room");
            _half = black ? new Vector2(5f, 5f) : _outdoors ? new Vector2(12f, 12f) : large ? new Vector2(7f, 9f) : small ? new Vector2(2.6f, 3f) : new Vector2(4.5f, 4f);

            var cam = Camera.main;
            if (black)
            {
                if (cam != null) cam.backgroundColor = Color.black;
                Block("Floor", new Vector3(0, -0.05f, 0), new Vector3(_half.x * 2, 0.1f, _half.y * 2), new Color(0.03f, 0.03f, 0.04f));
                Bounds(4f);
                return;
            }

            if (_outdoors)
            {
                if (cam != null) cam.backgroundColor = Tint(new Color(0.62f, 0.72f, 0.84f), 0.12f);
                var ground = Has(id, "lake", "path", "street", "march", "protest", "parking") ? new Color(0.48f, 0.47f, 0.45f) : new Color(0.36f, 0.5f, 0.3f);
                Block("Ground", new Vector3(0, -0.05f, 0), new Vector3(_half.x * 2 + 8, 0.1f, _half.y * 2 + 8), Tint(ground, 0.1f));
                if (Has(id, "lake"))
                    Block("Lake", new Vector3(0, -0.02f, _half.y + 10f), new Vector3(60f, 0.06f, 20f), new Color(0.35f, 0.45f, 0.55f));
                for (int i = 0; i < 14; i++)
                {
                    float a = (float)(rng.NextDouble() * Math.PI * 2);
                    var p = new Vector3(Mathf.Cos(a) * (_half.x + 1.5f), 0, Mathf.Sin(a) * (_half.y + 1.5f));
                    Tree(p, rng);
                }
                Bounds(3f);
            }
            else
            {
                if (cam != null) cam.backgroundColor = new Color(0.08f, 0.07f, 0.1f);
                var wall = Tint(new Color(0.78f, 0.72f, 0.62f), 0.15f);
                var floor = Tint(new Color(0.45f, 0.38f, 0.3f), 0.15f);
                float h = large ? 4.5f : 2.8f;
                Block("Floor", new Vector3(0, -0.05f, 0), new Vector3(_half.x * 2, 0.1f, _half.y * 2), floor);
                Block("Wall N", new Vector3(0, h / 2, _half.y), new Vector3(_half.x * 2, h, 0.15f), wall);
                Block("Wall S", new Vector3(0, h / 2, -_half.y), new Vector3(_half.x * 2, h, 0.15f), wall);
                Block("Wall W", new Vector3(-_half.x, h / 2, 0), new Vector3(0.15f, h, _half.y * 2), wall);
                Block("Wall E", new Vector3(_half.x, h / 2, 0), new Vector3(0.15f, h, _half.y * 2), wall);
                var lamp = new GameObject("Room Light", typeof(Light));
                lamp.transform.SetParent(_root.transform, false);
                lamp.transform.localPosition = new Vector3(0, h - 0.4f, 0);
                var l = lamp.GetComponent<Light>();
                l.type = LightType.Point; l.range = Mathf.Max(_half.x, _half.y) * 2.4f; l.intensity = 1.4f;
                l.color = new Color(1f, 0.9f, 0.78f);

                // A few things by kind of place, so it reads as that place.
                if (Has(id, "church", "chapel", "meeting", "sanctuary", "synagogue", "shul", "temple", "renewal") && !Has(id, "fellowship", "social", "school"))
                {
                    var wood = new Color(0.45f, 0.31f, 0.2f);
                    for (int row = 0; row < 5; row++)
                    {
                        // Pews either side of a wide aisle (people and things to explore stand in the aisle and by the walls).
                        Block("Pew", new Vector3(-3.4f, 0.25f, -3.5f + row * 1.5f), new Vector3(2f, 0.5f, 0.5f), wood);
                        Block("Pew", new Vector3(3.4f, 0.25f, -3.5f + row * 1.5f), new Vector3(2f, 0.5f, 0.5f), wood);
                    }
                    Block("Front", new Vector3(0, 0.08f, _half.y - 2.2f), new Vector3(4f, 0.16f, 2f), Tint(wood, 0.2f)); // Low enough to step onto.
                }
                else if (Has(id, "museum", "gallery", "collections", "shop", "store", "circle", "barber", "cafe"))
                {
                    for (int i = -1; i <= 1; i++)
                        Block("Shelf", new Vector3(i * 2.6f, 0.9f, _half.y - 0.4f), new Vector3(2f, 1.8f, 0.5f), new Color(0.4f, 0.3f, 0.24f));
                    Block("Counter", new Vector3(-_half.x + 1.2f, 0.45f, 0), new Vector3(1.2f, 0.9f, 3f), new Color(0.5f, 0.36f, 0.25f));
                }
                else if (Has(id, "apartment", "dorm", "maya_room", "guest_room"))
                {
                    Block("Bed", new Vector3(_half.x - 1.1f, 0.25f, _half.y - 1.4f), new Vector3(1.6f, 0.5f, 2.2f), Tint(new Color(0.55f, 0.6f, 0.7f), 0.3f));
                    Block("Rug", new Vector3(0, 0.01f, 0), new Vector3(2.4f, 0.02f, 1.6f), Tint(new Color(0.6f, 0.35f, 0.3f), 0.3f));
                }
            }
        }

        void Tree(Vector3 at, System.Random rng)
        {
            Shape(PrimitiveType.Cylinder, "Tree", at + new Vector3(0, 1f, 0), new Vector3(0.3f, 1f, 0.3f), new Color(0.4f, 0.3f, 0.22f));
            float s = 1.6f + (float)rng.NextDouble();
            var top = Shape(PrimitiveType.Sphere, "Leaves", at + new Vector3(0, 2.2f + s * 0.3f, 0), Vector3.one * s, new Color(0.25f, 0.42f, 0.24f));
            Destroy(top.GetComponent<Collider>());
        }

        /// <summary>Invisible walls around an outdoor place (or the void), so the character can't walk off it.</summary>
        void Bounds(float height)
        {
            void Wall(Vector3 p, Vector3 s)
            {
                var go = new GameObject("Edge", typeof(BoxCollider));
                go.transform.SetParent(_root.transform, false);
                go.transform.localPosition = p;
                go.GetComponent<BoxCollider>().size = s;
            }
            Wall(new Vector3(0, height / 2, _half.y + 0.5f), new Vector3(_half.x * 2 + 2, height, 1));
            Wall(new Vector3(0, height / 2, -_half.y - 0.5f), new Vector3(_half.x * 2 + 2, height, 1));
            Wall(new Vector3(-_half.x - 0.5f, height / 2, 0), new Vector3(1, height, _half.y * 2 + 2));
            Wall(new Vector3(_half.x + 0.5f, height / 2, 0), new Vector3(1, height, _half.y * 2 + 2));
        }

        // ── Things to explore ─────────────────────────────────────────────────

        public enum SpotKind { Thing, Person, Way }

        /// <summary>One explore choice the story offers right now.</summary>
        public struct Offer
        {
            public int index;
            public string verb, target, label;
        }

        /// <summary>
        /// Shows the story's current explore choices in the place: makes the ones that are new, lights up the ones on
        /// offer, and dims the rest. <paramref name="choose"/> gets the Ink choice index.
        /// </summary>
        public void ShowOffers(List<Offer> offers, Action<int, Transform, string> choose)
        {
            foreach (var s in _spots.Values) if (s != null) s.SetLive(null, -1, null);
            if (_root == null) return;
            foreach (var o in offers)
            {
                string key = Key(o.target);
                if (!_spots.TryGetValue(key, out var spot) || spot == null)
                {
                    spot = MakeSpot(o.verb, o.target);
                    _spots[key] = spot;
                }
                var offer = o;
                spot.SetLive(offer.verb + ": " + offer.label, offer.index, () => choose(offer.index, spot.transform, offer.verb));
            }
        }

        /// <summary>
        /// Someone speaking here is standing here: makes their figure if it isn't in the place yet (the same figure a later
        /// <c>Talk:</c> uses), and returns it so the camera can turn to them. Null outside a place.
        /// </summary>
        public Transform Person(string name)
        {
            if (_root == null || string.IsNullOrEmpty(name)) return null;
            string key = Key(name);
            if (!_spots.TryGetValue(key, out var spot) || spot == null)
            {
                spot = MakeSpot("Talk", name);
                _spots[key] = spot;
            }
            return spot.transform;
        }

        /// <summary>
        /// The way on to the next scene (the story is heading somewhere else): a glowing doorway at the far end of the place.
        /// </summary>
        public void ShowExit(string destinationName, Action use)
        {
            if (_root == null) { use?.Invoke(); return; }
            string key = "exit:" + destinationName.ToLowerInvariant();
            if (!_spots.TryGetValue(key, out var spot) || spot == null)
            {
                spot = MakeSpot("Go", destinationName);
                _spots[key] = spot;
            }
            spot.SetLive("Go: " + destinationName, -1, use);
        }

        /// <summary>Nothing is on offer (a line is showing, or a choice was just made).</summary>
        public void Dim()
        {
            foreach (var s in _spots.Values) if (s != null) s.SetLive(null, -1, null);
        }

        ChoiceSpot MakeSpot(string verb, string target)
        {
            var kind = verb == "Talk" ? SpotKind.Person : verb == "Go" ? SpotKind.Way : SpotKind.Thing;
            var go = new GameObject("Spot: " + target);
            go.transform.SetParent(_root.transform, false);
            Vector3 p; Quaternion facing;
            NextSlot(kind, out p, out facing);
            go.transform.localPosition = p;
            go.transform.localRotation = facing;

            string t = target.ToLowerInvariant();
            var rng = new System.Random(Hash(target));
            Color hue = Color.HSVToRGB((float)rng.NextDouble(), 0.35f, 0.75f);
            float labelHeight;

            if (kind == SpotKind.Person)
            {
                Part(go, PrimitiveType.Capsule, new Vector3(0, 0.8f, 0), new Vector3(0.5f, 0.8f, 0.5f), hue);
                Part(go, PrimitiveType.Sphere, new Vector3(0, 1.75f, 0), Vector3.one * 0.32f, new Color(0.85f, 0.72f, 0.6f));
                labelHeight = 2.3f;
            }
            else if (kind == SpotKind.Way)
            {
                Part(go, PrimitiveType.Cube, new Vector3(-0.6f, 1.1f, 0), new Vector3(0.15f, 2.2f, 0.25f), new Color(0.35f, 0.28f, 0.22f));
                Part(go, PrimitiveType.Cube, new Vector3(0.6f, 1.1f, 0), new Vector3(0.15f, 2.2f, 0.25f), new Color(0.35f, 0.28f, 0.22f));
                Part(go, PrimitiveType.Cube, new Vector3(0, 2.25f, 0), new Vector3(1.35f, 0.15f, 0.25f), new Color(0.35f, 0.28f, 0.22f));
                Part(go, PrimitiveType.Cube, new Vector3(0, 1.1f, 0.05f), new Vector3(1.05f, 2.1f, 0.05f), new Color(0.95f, 0.9f, 0.75f));
                labelHeight = 2.6f;
            }
            else if (Has(t, "window"))
            {
                Part(go, PrimitiveType.Cube, new Vector3(0, 1.5f, 0), new Vector3(1.4f, 1.1f, 0.08f), new Color(0.7f, 0.8f, 0.9f));
                labelHeight = 2.3f;
            }
            else if (Has(t, "candle"))
            {
                Part(go, PrimitiveType.Cube, new Vector3(0, 0.4f, 0), new Vector3(0.7f, 0.8f, 0.5f), new Color(0.4f, 0.3f, 0.22f));
                Part(go, PrimitiveType.Cylinder, new Vector3(-0.12f, 0.95f, 0), new Vector3(0.06f, 0.15f, 0.06f), new Color(0.97f, 0.95f, 0.85f));
                Part(go, PrimitiveType.Cylinder, new Vector3(0.12f, 0.95f, 0), new Vector3(0.06f, 0.15f, 0.06f), new Color(0.97f, 0.95f, 0.85f));
                labelHeight = 1.6f;
            }
            else if (Has(t, "shelf", "books", "bookshelf", "boxes", "case", "ark", "corkboard", "noticeboard", "flags", "portrait", "photo", "list"))
            {
                Part(go, PrimitiveType.Cube, new Vector3(0, 1f, 0), new Vector3(1.4f, 2f, 0.4f), Color.Lerp(hue, new Color(0.4f, 0.3f, 0.24f), 0.6f));
                labelHeight = 2.4f;
            }
            else if (Has(t, "chair", "seat", "sofa", "bench", "pew", "circle", "cushion"))
            {
                Part(go, PrimitiveType.Cube, new Vector3(0, 0.25f, 0), new Vector3(1.2f, 0.5f, 0.6f), hue);
                Part(go, PrimitiveType.Cube, new Vector3(0, 0.7f, -0.25f), new Vector3(1.2f, 0.5f, 0.1f), hue);
                labelHeight = 1.5f;
            }
            else if (Has(t, "lake", "water", "ceiling", "quiet", "sky", "crowd", "people", "couples", "family", "children", "group"))
            {
                // Something you look AT rather than pick up: a soft marker on the ground.
                Part(go, PrimitiveType.Cylinder, new Vector3(0, 0.02f, 0), new Vector3(1.2f, 0.02f, 1.2f), Color.Lerp(hue, Color.white, 0.4f));
                labelHeight = 1.4f;
            }
            else
            {
                // A small thing on a little table.
                Part(go, PrimitiveType.Cube, new Vector3(0, 0.4f, 0), new Vector3(0.8f, 0.8f, 0.6f), new Color(0.42f, 0.32f, 0.24f));
                Part(go, PrimitiveType.Cube, new Vector3(0, 0.92f, 0), new Vector3(0.28f, 0.24f, 0.2f), hue);
                labelHeight = 1.5f;
            }

            // Reachable from a step away, whatever its shape.
            var reach = go.AddComponent<BoxCollider>();
            reach.isTrigger = true;
            reach.center = new Vector3(0, 1f, 0);
            reach.size = new Vector3(1.6f, 2f, 1.6f);

            var spot = go.AddComponent<ChoiceSpot>();
            spot.Build(kind == SpotKind.Person ? new Color(0.9f, 0.94f, 1f) : new Color(1f, 0.9f, 0.68f));
            spot.LabelHeight = labelHeight;
            return spot;
        }

        /// <summary>
        /// Where the next new thing goes: things along the walls (or the edge, outdoors), people nearer the middle, ways
        /// out at the far end. The first ones are where the character is facing.
        /// </summary>
        void NextSlot(SpotKind kind, out Vector3 p, out Quaternion facing)
        {
            float hx = _half.x - 0.9f, hz = _half.y - 0.9f;
            if (kind == SpotKind.Way)
            {
                float[] xs = { 0f, -hx * 0.6f, hx * 0.6f, -hx * 0.3f, hx * 0.3f };
                int i = _doorSlots++ % xs.Length;
                p = new Vector3(xs[i], 0, _half.y - 0.2f);
                facing = Quaternion.Euler(0, 180, 0);
                return;
            }
            if (kind == SpotKind.Person)
            {
                // A ring around the middle, starting in front.
                float a = 90f + 50f * ((_innerSlots + 1) / 2) * ((_innerSlots % 2 == 0) ? 1 : -1);
                _innerSlots++;
                float r = 0.45f;
                p = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad) * hx * r, 0, Mathf.Sin(a * Mathf.Deg2Rad) * hz * r + hz * 0.1f);
                facing = Quaternion.LookRotation(new Vector3(0, 0, -_half.y) - p);
                return;
            }
            // Along the walls: north first (ahead), then the sides, working back toward the way in.
            Vector3[] wall =
            {
                new Vector3(0, 0, hz), new Vector3(-hx * 0.55f, 0, hz), new Vector3(hx * 0.55f, 0, hz),
                new Vector3(-hx, 0, hz * 0.4f), new Vector3(hx, 0, hz * 0.4f),
                new Vector3(-hx, 0, -hz * 0.2f), new Vector3(hx, 0, -hz * 0.2f),
                new Vector3(-hx * 0.9f, 0, hz * 0.9f), new Vector3(hx * 0.9f, 0, hz * 0.9f),
                new Vector3(-hx, 0, -hz * 0.6f), new Vector3(hx, 0, -hz * 0.6f),
                new Vector3(-hx * 0.3f, 0, hz * 0.7f), new Vector3(hx * 0.3f, 0, hz * 0.7f),
            };
            int n = _wallSlots++;
            p = wall[n % wall.Length];
            if (n >= wall.Length) p *= 0.7f; // A second lap, further in.
            var toCentre = new Vector3(-p.x, 0, -p.z);
            facing = toCentre.sqrMagnitude > 0.01f ? Quaternion.LookRotation(toCentre) : Quaternion.identity;
        }

        /// <summary>
        /// Maya's altar: a low table against the left wall, a cloth, two candles, the deck and a small bowl. Art replaces it
        /// (ART_LIST: altar_cloth, candle, chalice, bowl). It grows over the years in the owner's art, not here.
        /// </summary>
        void BuildAltar()
        {
            var go = new GameObject("Altar");
            go.transform.SetParent(_root.transform, false);
            float hx = _half.x - 0.8f;
            go.transform.localPosition = new Vector3(-hx, 0, 0.2f);
            go.transform.localRotation = Quaternion.Euler(0, 90, 0);
            Part(go, PrimitiveType.Cube, new Vector3(0, 0.35f, 0), new Vector3(1.3f, 0.7f, 0.6f), new Color(0.36f, 0.26f, 0.2f));
            Part(go, PrimitiveType.Cube, new Vector3(0, 0.71f, 0), new Vector3(1.36f, 0.02f, 0.64f), new Color(0.35f, 0.22f, 0.42f));
            Part(go, PrimitiveType.Cylinder, new Vector3(-0.45f, 0.86f, 0.1f), new Vector3(0.07f, 0.14f, 0.07f), new Color(0.97f, 0.95f, 0.86f));
            Part(go, PrimitiveType.Cylinder, new Vector3(0.45f, 0.86f, 0.1f), new Vector3(0.07f, 0.14f, 0.07f), new Color(0.2f, 0.18f, 0.22f));
            Part(go, PrimitiveType.Cube, new Vector3(0, 0.76f, 0.05f), new Vector3(0.16f, 0.06f, 0.26f), new Color(0.25f, 0.2f, 0.45f));
            Part(go, PrimitiveType.Cylinder, new Vector3(0.18f, 0.75f, -0.15f), new Vector3(0.18f, 0.03f, 0.18f), new Color(0.75f, 0.75f, 0.8f));
            var reach = go.AddComponent<BoxCollider>();
            reach.isTrigger = true;
            reach.center = new Vector3(0, 1f, 0);
            reach.size = new Vector3(1.8f, 2f, 1.8f);
            var spot = go.AddComponent<AltarSpot>();
            spot.Usable = () => AltarUsable != null && AltarUsable();
            spot.Used = () => AltarUsed?.Invoke();
        }

        // ── Building blocks ───────────────────────────────────────────────────

        GameObject Block(string name, Vector3 local, Vector3 size, Color colour) =>
            Shape(PrimitiveType.Cube, name, local, size, colour);

        GameObject Shape(PrimitiveType type, string name, Vector3 local, Vector3 scale, Color colour)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(_root.transform, false);
            go.transform.localPosition = local;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = Mat(colour);
            return go;
        }

        void Part(GameObject parent, PrimitiveType type, Vector3 local, Vector3 scale, Color colour)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = type.ToString();
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = local;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = Mat(colour);
        }

        Material Mat(Color c)
        {
            if (_materials.TryGetValue(c, out var m) && m != null) return m;
            m = _template != null ? new Material(_template) : new Material(Shader.Find("Universal Render Pipeline/Lit"));
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
            if (m.HasProperty("_Color")) m.SetColor("_Color", c);
            if (m.HasProperty("_BaseMap")) m.SetTexture("_BaseMap", null);
            _materials[c] = m;
            return m;
        }

        /// <summary>One figure per person, whatever Maya calls them ("Mommy" and "Mom" are Sarah).</summary>
        static string Key(string name)
        {
            string k = name.Trim().ToLowerInvariant();
            return k == "mom" || k == "mommy" ? "sarah" : k;
        }

        static int Hash(string s)
        {
            unchecked
            {
                int h = 23;
                foreach (char ch in s) h = h * 31 + ch;
                return h;
            }
        }
    }

    /// <summary>
    /// One thing in a place that the story offers. While it's on offer the thing ITSELF glows (owner: on the objects, subtle
    /// but noticeable): its colour breathes slowly toward a warm light. Otherwise it's just scenery. No floating labels; the
    /// prompt at the bottom of the screen names it when the character is close.
    /// </summary>
    public sealed class ChoiceSpot : MonoBehaviour, IInteractable
    {
        string _prompt;
        Action _use;
        Color _glow;
        float _phase;
        Renderer[] _renderers;
        Color[] _base;
        MaterialPropertyBlock _block;
        bool _lit;

        /// <summary>Roughly the top of the thing (for the camera and future markers).</summary>
        public float LabelHeight { get; set; }

        public string Prompt => _prompt ?? "";
        public bool CanInteract => _use != null;
        public void Interact() => _use?.Invoke();

        public void Build(Color glowColour)
        {
            _glow = glowColour;
            _phase = UnityEngine.Random.value * 6f;
            _block = new MaterialPropertyBlock();
            _renderers = GetComponentsInChildren<Renderer>();
            _base = new Color[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                var m = _renderers[i].sharedMaterial;
                _base[i] = m == null ? Color.grey : m.HasProperty("_BaseColor") ? m.GetColor("_BaseColor") : m.HasProperty("_Color") ? m.GetColor("_Color") : Color.grey;
            }
            SetLive(null, -1, null);
        }

        public void SetLive(string prompt, int index, Action use)
        {
            _prompt = prompt;
            _use = use;
            if (use == null && _lit) Paint(0f);
        }

        void LateUpdate()
        {
            if (_use == null) return;
            // Breathes slowly (about every four seconds: far from any flashing rate), never fully white.
            float t = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * 1.5f + _phase);
            Paint(0.16f + 0.2f * t);
        }

        void Paint(float amount)
        {
            if (_renderers == null) return;
            _lit = amount > 0f;
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] == null) continue;
                var c = Color.Lerp(_base[i], _glow, amount);
                _renderers[i].GetPropertyBlock(_block);
                _block.SetColor("_BaseColor", c);
                _block.SetColor("_Color", c);
                _renderers[i].SetPropertyBlock(_block);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmAsIAm.Core.PluralSystem
{
    public enum SwitchResult
    {
        Switched,
        /// <summary>The member said "not now". This is respected and is never treated as a failure.</summary>
        DeclinedNotNow,
        /// <summary>The member isn't ready yet but offered to co-front instead.</summary>
        OfferedCoFront,
        /// <summary>This member doesn't front (by design), or is symbolic right now.</summary>
        NotAvailable,
        AlreadyFronting,
        UnknownMember,
    }

    public readonly struct FrontChange
    {
        public readonly string PreviousId;
        public readonly string NewId;
        /// <summary>False for story-driven switches. These usually come with time loss.</summary>
        public readonly bool Voluntary;

        public FrontChange(string previousId, string newId, bool voluntary)
        {
            PreviousId = previousId;
            NewId = newId;
            Voluntary = voluntary;
        }
    }

    /// <summary>
    /// The plural system: its members, who is fronting, who is co-conscious, and how much trust
    /// each member has. Consent is a mechanic: voluntary switches require the member's willingness.
    /// </summary>
    public sealed class SystemRoster
    {
        readonly Dictionary<string, SystemMemberDefinition> _definitions = new Dictionary<string, SystemMemberDefinition>();
        readonly Dictionary<string, SystemMemberState> _states = new Dictionary<string, SystemMemberState>();
        readonly HashSet<string> _coConscious = new HashSet<string>();

        public string FrontingId { get; private set; }
        public IReadOnlyCollection<string> CoConscious => _coConscious;
        public IEnumerable<SystemMemberDefinition> Members => _definitions.Values;

        public event Action<FrontChange> FrontChanged;
        public event Action<string, bool> CoConsciousChanged;
        public event Action<string, float> TrustChanged;

        public SystemRoster(IEnumerable<SystemMemberDefinition> members, string initialFrontId)
        {
            foreach (var def in members)
            {
                if (string.IsNullOrEmpty(def.id)) throw new ArgumentException("System member is missing an id.");
                _definitions.Add(def.id, def);
                _states.Add(def.id, new SystemMemberState
                {
                    id = def.id,
                    trust = 0f,
                    presentation = def.InitialPresentation,
                });
            }

            if (!_definitions.ContainsKey(initialFrontId))
                throw new ArgumentException($"Initial fronter '{initialFrontId}' is not a system member.");
            FrontingId = initialFrontId;
        }

        public SystemMemberDefinition GetDefinition(string id) =>
            _definitions.TryGetValue(id, out var def) ? def : null;

        public SystemMemberState GetState(string id) =>
            _states.TryGetValue(id, out var s) ? s : null;

        /// <summary>
        /// The player asks a member to front. The member decides.
        /// </summary>
        public SwitchResult RequestSwitch(string memberId)
        {
            if (!_definitions.TryGetValue(memberId, out var def)) return SwitchResult.UnknownMember;
            if (memberId == FrontingId) return SwitchResult.AlreadyFronting;

            var state = _states[memberId];
            if (!def.canFront || state.presentation == MemberPresentation.Symbolic) return SwitchResult.NotAvailable;
            if (state.restingByChoice) return SwitchResult.DeclinedNotNow;

            if (state.trust >= def.trustToFront)
            {
                SetFront(memberId, voluntary: true);
                return SwitchResult.Switched;
            }

            // Partway there: offer to be present alongside instead.
            if (state.trust >= def.trustToFront * 0.5f)
            {
                SetCoConscious(memberId, true);
                return SwitchResult.OfferedCoFront;
            }

            return SwitchResult.DeclinedNotNow;
        }

        /// <summary>
        /// A story-driven switch. This bypasses consent because it isn't the player's choice;
        /// that's the point. The Runtime layer pairs it with time loss and gap recovery, and
        /// honours the "ask before involuntary switches" comfort setting.
        /// </summary>
        public void ForceSwitch(string memberId)
        {
            if (!_definitions.ContainsKey(memberId)) throw new ArgumentException($"Unknown member '{memberId}'.");
            if (memberId == FrontingId) return;
            SetFront(memberId, voluntary: false);
        }

        public void SetCoConscious(string memberId, bool present)
        {
            if (!_definitions.ContainsKey(memberId) || memberId == FrontingId) return;
            bool changed = present ? _coConscious.Add(memberId) : _coConscious.Remove(memberId);
            if (changed) CoConsciousChanged?.Invoke(memberId, present);
        }

        public void AdjustTrust(string memberId, float delta)
        {
            if (!_states.TryGetValue(memberId, out var state)) return;
            float next = Math.Max(0f, Math.Min(1f, state.trust + delta));
            if (Math.Abs(next - state.trust) < 1e-6f) return;
            state.trust = next;
            TrustChanged?.Invoke(memberId, next);
        }

        public void SetPresentation(string memberId, MemberPresentation presentation)
        {
            if (_states.TryGetValue(memberId, out var state)) state.presentation = presentation;
        }

        public void SetResting(string memberId, bool resting)
        {
            if (_states.TryGetValue(memberId, out var state)) state.restingByChoice = resting;
        }

        /// <summary>True if the fronter or any co-conscious member has the given perception trait.</summary>
        public bool CanPerceive(string trait)
        {
            if (HasTrait(FrontingId, trait)) return true;
            return _coConscious.Any(id => HasTrait(id, trait));
        }

        public IEnumerable<SystemMemberState> ExportStates() => _states.Values;

        public void ImportStates(IEnumerable<SystemMemberState> states, string frontingId, IEnumerable<string> coConscious)
        {
            foreach (var s in states)
                if (_states.ContainsKey(s.id)) _states[s.id] = s;
            if (frontingId != null && _definitions.ContainsKey(frontingId)) FrontingId = frontingId;
            _coConscious.Clear();
            if (coConscious != null)
                foreach (var id in coConscious)
                    if (_definitions.ContainsKey(id) && id != FrontingId) _coConscious.Add(id);
        }

        bool HasTrait(string memberId, string trait) =>
            memberId != null && _definitions.TryGetValue(memberId, out var def) && def.perceptionTraits.Contains(trait);

        void SetFront(string memberId, bool voluntary)
        {
            string previous = FrontingId;
            FrontingId = memberId;
            _coConscious.Remove(memberId);
            FrontChanged?.Invoke(new FrontChange(previous, memberId, voluntary));
        }
    }
}

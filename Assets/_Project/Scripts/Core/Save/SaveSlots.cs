using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Save
{
    /// <summary>Turns save objects into text and back. Runtime uses JsonUtility; the .NET tests use System.Text.Json.</summary>
    public interface ISaveSerializer
    {
        string ToJson(object value);
        T FromJson<T>(string json);
    }

    /// <summary>The few facts the save screen shows for a slot, so it doesn't have to read the whole save.</summary>
    [Serializable]
    public sealed class SaveSlotHeader
    {
        /// <summary>1-based slot number.</summary>
        public int slot;
        /// <summary>The chapter being played when it saved, or empty for time between chapters.</summary>
        public string chapterId;
        public string chapterTitle;
        public string frontingId;
        public string frontingName;
        public int inGameDay;
        /// <summary>When it saved, as UTC ticks (0 = unknown).</summary>
        public long savedAtUtcTicks;
        public float playSeconds;
    }

    public enum SaveSlotStatus
    {
        Empty,
        Ready,
        /// <summary>The file is there but can't be read. The player may delete it; it is never overwritten silently.</summary>
        Unreadable,
        /// <summary>Made by a newer version of the game. Kept as it is.</summary>
        TooNew,
    }

    public sealed class SaveSlotSummary
    {
        public int Slot;
        public SaveSlotStatus Status;
        /// <summary>Null unless Status is Ready.</summary>
        public SaveSlotHeader Header;
        public bool IsEmpty => Status == SaveSlotStatus.Empty;
        public bool CanLoad => Status == SaveSlotStatus.Ready;
    }

    /// <summary>
    /// The save slots (owner, 2026-11-12). Each slot is a separate, complete game: progress, the journal and the
    /// chapter unlocks belong to that slot only. A slot is two keys in storage: the full save, and a small header
    /// for the save screen. The header is written last; if it's missing, it's rebuilt from the full save.
    /// </summary>
    public sealed class SaveSlots
    {
        public const int DefaultCount = 3;

        readonly ISaveStorage _storage;
        readonly ISaveSerializer _json;

        public int Count { get; }

        public SaveSlots(ISaveStorage storage, ISaveSerializer serializer, int count = DefaultCount)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _json = serializer ?? throw new ArgumentNullException(nameof(serializer));
            if (count < 1) throw new ArgumentException("There must be at least one save slot.", nameof(count));
            Count = count;
        }

        public static string DataKey(int slot) => "slot_" + slot;
        public static string HeaderKey(int slot) => "slot_" + slot + "_header";

        public bool IsValid(int slot) => slot >= 1 && slot <= Count;

        void Check(int slot)
        {
            if (!IsValid(slot)) throw new ArgumentOutOfRangeException(nameof(slot), $"Save slots are 1 to {Count}.");
        }

        public IReadOnlyList<SaveSlotSummary> List()
        {
            var list = new List<SaveSlotSummary>(Count);
            for (int i = 1; i <= Count; i++) list.Add(Describe(i));
            return list;
        }

        public SaveSlotSummary Describe(int slot)
        {
            Check(slot);
            var summary = new SaveSlotSummary { Slot = slot, Status = SaveSlotStatus.Empty };
            if (!_storage.Exists(DataKey(slot))) return summary;

            var header = TryParse<SaveSlotHeader>(_storage.Read(HeaderKey(slot)));
            if (header == null)
            {
                // No header (or a broken one): read the full save instead.
                var data = TryParse<SaveData>(_storage.Read(DataKey(slot)));
                if (data == null) { summary.Status = SaveSlotStatus.Unreadable; return summary; }
                if (data.version > SaveData.CurrentVersion) { summary.Status = SaveSlotStatus.TooNew; return summary; }
                header = data.header ?? new SaveSlotHeader();
            }
            header.slot = slot;
            summary.Header = header;
            summary.Status = SaveSlotStatus.Ready;
            return summary;
        }

        /// <summary>Writes a complete save into a slot (overwriting it). The header is stamped with the slot number.</summary>
        public void Write(int slot, SaveData data)
        {
            Check(slot);
            if (data == null) throw new ArgumentNullException(nameof(data));
            data.version = SaveData.CurrentVersion;
            data.header ??= new SaveSlotHeader();
            data.header.slot = slot;
            _storage.Write(DataKey(slot), _json.ToJson(data));
            _storage.Write(HeaderKey(slot), _json.ToJson(data.header));
        }

        /// <summary>Reads a slot's full save. False for an empty, unreadable or newer-version slot.</summary>
        public bool TryRead(int slot, out SaveData data)
        {
            Check(slot);
            data = TryParse<SaveData>(_storage.Read(DataKey(slot)));
            if (data == null) return false;
            if (data.version > SaveData.CurrentVersion) { data = null; return false; }
            data.header ??= new SaveSlotHeader();
            data.header.slot = slot;
            return true;
        }

        public void Delete(int slot)
        {
            Check(slot);
            _storage.Delete(DataKey(slot));
            _storage.Delete(HeaderKey(slot));
        }

        /// <summary>The slot saved most recently, or 0 if every slot is empty.</summary>
        public int MostRecent()
        {
            int best = 0;
            long bestTicks = long.MinValue;
            foreach (var s in List())
            {
                if (!s.CanLoad) continue;
                if (best == 0 || s.Header.savedAtUtcTicks > bestTicks) { best = s.Slot; bestTicks = s.Header.savedAtUtcTicks; }
            }
            return best;
        }

        /// <summary>The first empty slot, or 0 if all are used.</summary>
        public int FirstEmpty()
        {
            foreach (var s in List()) if (s.IsEmpty) return s.Slot;
            return 0;
        }

        T TryParse<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try { return _json.FromJson<T>(json); }
            catch (Exception) { return null; }
        }
    }

    /// <summary>Wording for the save screen, kept here so it can be tested.</summary>
    public static class SaveSlotText
    {
        /// <summary>"12 min", "1 h 05 min". Under a minute reads "under a minute".</summary>
        public static string PlayTime(float seconds)
        {
            if (seconds < 60f) return "under a minute";
            int minutes = (int)(seconds / 60f);
            if (minutes < 60) return minutes + " min";
            return $"{minutes / 60} h {minutes % 60:00} min";
        }

        /// <summary>Where the save is: the chapter's title, or "Between chapters".</summary>
        public static string Place(SaveSlotHeader h) =>
            h == null || string.IsNullOrEmpty(h.chapterId) ? "Between chapters"
            : string.IsNullOrEmpty(h.chapterTitle) ? h.chapterId : h.chapterTitle;
    }
}

using System;
using System.Collections.Generic;

namespace IAmAsIAm.Core.Story
{
    /// <summary>
    /// A place the story happens in (Resources/Data/places.json): the id used by Ink's <c># place:</c>, the name shown when
    /// heading there, and where it sits on its city's map (0..1 across and up). Places without a city aren't on a map
    /// (the car, the black screen).
    /// </summary>
    [Serializable]
    public sealed class PlaceDefinition
    {
        public string id;
        public string name;
        public string city;
        public float x;
        public float y;
        /// <summary>The chapter it first appears in (the art list orders places by this).</summary>
        public string chapterId;
        /// <summary>One of Maya's homes, with her altar in it (owner: an altar in every home, from the Prelude on).</summary>
        public bool altar;
    }

    [Serializable]
    public sealed class CityDefinition
    {
        public string id;
        public string name;
    }

    [Serializable]
    public sealed class PlaceData
    {
        public List<CityDefinition> cities = new List<CityDefinition>();
        public List<PlaceDefinition> places = new List<PlaceDefinition>();

        public PlaceDefinition Find(string id) => places.Find(p => p.id == id);
        public CityDefinition City(string id) => cities.Find(c => c.id == id);

        /// <summary>The place's name, or its id made readable if it isn't listed yet.</summary>
        public string NameOf(string id)
        {
            var p = Find(id);
            if (p != null && !string.IsNullOrEmpty(p.name)) return p.name;
            return string.IsNullOrEmpty(id) ? "" : char.ToUpperInvariant(id[0]) + id.Substring(1).Replace('_', ' ');
        }

    }
}

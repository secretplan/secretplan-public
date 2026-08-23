using Newtonsoft.Json;
using SecretPlanCore.Configuration;

namespace DATA_ASSEMBLY.DistributableConfig;

[SerializedTypeId("LocalizationIdTable")]
public class LocalizationIdTable : Config
{
    private readonly Dictionary<string, uint> _slugToId = new();
    private bool _hasBuiltSlugTable;

    [JsonProperty("ids_to_slugs")]
    private Dictionary<uint, string> _idToSlug = new();

    public uint AddNewSlug(string slug)
    {
        var id = ConfigServer.Instance.GenerateInstanceId();

        while (_idToSlug.ContainsKey(id))
        {
            id = ConfigServer.Instance.GenerateInstanceId();
        }

        _idToSlug[id] = slug;

        return id;
    }

    public void AddSlugAndId(uint id, string slug)
    {
        _idToSlug[id] = slug;
    }

    public bool HasKey(uint id)
    {
        return _idToSlug.ContainsKey(id);
    }

    public string GetSlug(uint id)
    {
        if (id == 0)
        {
            return "NULL";
        }

        return _idToSlug[id];
    }

    public uint GetId(string targetSlug)
    {
        BuildReverseTableIfNotAlready();

        return _slugToId.GetValueOrDefault(targetSlug);
    }

    private void BuildReverseTableIfNotAlready()
    {
        if (!_hasBuiltSlugTable)
        {
            foreach (var (id, slug) in _idToSlug)
            {
                _slugToId[slug] = id;
            }

            _hasBuiltSlugTable = true;
        }
    }

    public IEnumerable<uint> AllIds()
    {
        return _idToSlug.Keys;
    }

    public bool HasSlug(string slug)
    {
        BuildReverseTableIfNotAlready();
        return _slugToId.ContainsKey(slug);
    }

    public void Remove(uint id)
    {
        var slug = _idToSlug.GetValueOrDefault(id);
        if (slug != null)
        {
            _slugToId.Remove(slug);
        }

        _idToSlug.Remove(id);
    }

    public void SetSlug(uint id, string newSlug)
    {
        if (!HasKey(id))
        {
            return;
        }

        _slugToId.Remove(_idToSlug[id]);
        _slugToId[newSlug] = id;
        _idToSlug[id] = newSlug;
    }

    public IEnumerable<(uint id, string slug)> AllIdsAndSlugs()
    {
        foreach (var id in AllIds())
        {
            yield return new ValueTuple<uint, string>(id, _idToSlug[id]);
        }
    }
}
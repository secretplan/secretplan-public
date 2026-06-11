using System.Reflection;
using SecretPlanCore.Core;
using SecretPlanCore.Telemetry;

namespace SecretPlanCore.Analytics;

public abstract class BaseAnalytics
{
    private readonly List<TimestampedTelemetryEvent> _allEvents = new();

    protected void AddEventToSortedList(TelemetryRowDownload databaseEntry, TelemetryEvent? payload)
    {
        if (payload == null)
        {
            return;
        }

        _allEvents.Add(new TimestampedTelemetryEvent(databaseEntry.CreatedAtTimeStamp, payload));
        _allEvents.Sort((a, b) => a.TimeStamp.CompareTo(b.TimeStamp));
    }

    public IEnumerable<T> AllEventsOfType<T>() where T : TelemetryEvent
    {
        foreach (var telemetryEvent in _allEvents)
        {
            if (telemetryEvent.TelemetryEvent is T typedEvent)
            {
                yield return typedEvent;
            }
        }
    }

    public abstract void AddEvent(TelemetryRowDownload row, TelemetryEvent payload);

    public IEnumerable<object?> GetDbCells()
    {
        foreach (var (_, _, member) in GetColumnsAndMembers())
        {
            var info = new FieldOrPropertyInfo(member);
            yield return info.GetValue(this);
        }
    }

    public IEnumerable<(string, Type, bool)> GetDbHeader()
    {
        foreach (var (columnName, attribute, memberInfo) in GetColumnsAndMembers())
        {
            var associatedType = new FieldOrPropertyInfo(memberInfo).AssociatedType();
            yield return (columnName, associatedType ?? typeof(object), attribute?.IsPrimaryKey ?? false);
        }
    }

    private IEnumerable<(string, DatabaseColumnAttribute?, MemberInfo)> GetColumnsAndMembers()
    {
        foreach (var member in Reflection.GetAllMembersInTypeWithAttribute<DatabaseColumnAttribute>(GetType()))
        {
            var attribute = member.GetCustomAttribute<DatabaseColumnAttribute>()!;

            yield return (attribute.Name, attribute, member);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    protected class DatabaseColumnAttribute : Attribute
    {
        public bool IsPrimaryKey { get; }

        public DatabaseColumnAttribute(string name, bool isPrimaryKey = false)
        {
            IsPrimaryKey = isPrimaryKey;
            Name = name;
        }


        public string Name { get; }
    }
}
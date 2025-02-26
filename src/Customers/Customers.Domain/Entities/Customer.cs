using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Customers.Domain.Entities;

public class Customer : IEquatable<Customer>
{
    private readonly ConcurrentDictionary<string, object> _metadata = new();
    private readonly List<CustomerAuditLog> _auditLogs = new();
    private CustomerState _state;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public CustomerType CustomerType { get; private set; }
    public CustomerTier CustomerTier { get; private set; }
    public IReadOnlyDictionary<string, object> Metadata => _metadata;
    public IReadOnlyCollection<CustomerAuditLog> AuditLogs => _auditLogs;

    private Customer(string name, string email, string phone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        CustomerType = CustomerType.Standard;
        CustomerTier = CustomerTier.Bronze;
        _state = CustomerState.Created;
    }

    public static Customer Create(string name, string email, string phone)
    {
        ValidateInput(name, nameof(name));
        ValidateInput(email, nameof(email));
        ValidateInput(phone, nameof(phone));

        return new Customer(name, email, phone);
    }

    public void Update(string name, string email, string phone, [CallerMemberName] string updatedBy = null)
    {
        ValidateStateTransition(CustomerState.Updated);

        Name = name;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
        _state = CustomerState.Updated;

        AddAuditLog($"Customer updated by {updatedBy}", updatedBy);
    }

    public void Deactivate(string reason, [CallerMemberName] string deactivatedBy = null)
    {
        ValidateStateTransition(CustomerState.Deactivated);

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        _state = CustomerState.Deactivated;

        _metadata.AddOrUpdate("DeactivationReason", reason, (_, _) => reason);

        AddAuditLog($"Customer deactivated by {deactivatedBy}. Reason: {reason}", deactivatedBy);
    }

    public void AddMetadata(string key, object value, [CallerMemberName] string addedBy = null)
    {
        _metadata.AddOrUpdate(key, value, (_, _) => value);

        AddAuditLog($"Metadata '{key}' added/updated by {addedBy}", addedBy);
    }

    private void ValidateStateTransition(CustomerState newState)
    {
        bool isValidTransition = _state switch
        {
            CustomerState.Created => true,
            CustomerState.Updated => newState != CustomerState.Created,
            CustomerState.Deactivated => false,
            _ => throw new InvalidOperationException($"Unknown state: {_state}")
        };

        if (!isValidTransition)
            throw new InvalidOperationException($"Invalid state transition from {_state} to {newState}");
    }

    private void AddAuditLog(string message, string performedBy)
    {
        _auditLogs.Add(new CustomerAuditLog(Id, message, DateTime.UtcNow, performedBy));
    }

    private static void ValidateInput(string input, string paramName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"{paramName} is required", paramName);
    }

    public bool Equals(Customer other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Customer)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public enum CustomerState
{
    Created,
    Updated,
    Deactivated
}

public enum CustomerType
{
    Standard,
    Premium,
    Enterprise
}

public enum CustomerTier
{
    Bronze,
    Silver,
    Gold,
    Platinum
}

public class CustomerAuditLog
{
    public Guid CustomerId { get; }
    public string Message { get; }
    public DateTime Timestamp { get; }
    public string PerformedBy { get; }

    public CustomerAuditLog(Guid customerId, string message, DateTime timestamp, string performedBy)
    {
        CustomerId = customerId;
        Message = message;
        Timestamp = timestamp;
        PerformedBy = performedBy;
    }
}
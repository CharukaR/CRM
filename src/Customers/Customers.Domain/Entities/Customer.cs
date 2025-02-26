using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Customers.Domain.Entities;

public class Customer : IEquatable<Customer>
{
    private readonly ConcurrentDictionary<string, object> _metadata;
    private readonly List<CustomerAuditLog> _auditLogs;
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

    private Customer() 
    {
        _metadata = new ConcurrentDictionary<string, object>();
        _auditLogs = new List<CustomerAuditLog>();
    }

    private Customer(string name, string email, string phone) : this()
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
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));

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
        
        AddAuditLog(new CustomerAuditLog(
            Id,
            $"Customer updated by {updatedBy}",
            DateTime.UtcNow,
            updatedBy
        ));
    }

    public void Deactivate(string reason, [CallerMemberName] string deactivatedBy = null)
    {
        ValidateStateTransition(CustomerState.Deactivated);
        
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        _state = CustomerState.Deactivated;
        
        _metadata.AddOrUpdate("DeactivationReason", reason, (_, _) => reason);
        
        AddAuditLog(new CustomerAuditLog(
            Id,
            $"Customer deactivated by {deactivatedBy}. Reason: {reason}",
            DateTime.UtcNow,
            deactivatedBy
        ));
    }

    public void AddMetadata(string key, object value, [CallerMemberName] string addedBy = null)
    {
        _metadata.AddOrUpdate(key, value, (_, _) => value);
        
        AddAuditLog(new CustomerAuditLog(
            Id,
            $"Metadata '{key}' added/updated by {addedBy}",
            DateTime.UtcNow,
            addedBy
        ));
    }

    private void ValidateStateTransition(CustomerState newState)
    {
        var isValidTransition = _state switch
        {
            CustomerState.Created => true,
            CustomerState.Updated => newState != CustomerState.Created,
            CustomerState.Deactivated => false,
            _ => throw new InvalidOperationException($"Unknown state: {_state}")
        };

        if (!isValidTransition)
            throw new InvalidOperationException($"Invalid state transition from {_state} to {newState}");
    }

    private void AddAuditLog(CustomerAuditLog log)
    {
        _auditLogs.Add(log);
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
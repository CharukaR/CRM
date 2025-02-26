using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging; // Assuming the use of Microsoft.Extensions.Logging for logging

namespace Customers.Domain.Entities;

public class Customer : IEquatable<Customer>
{
    private readonly ConcurrentDictionary<string, object> _metadata = new();
    private readonly List<CustomerAuditLog> _auditLogs = new();
    private CustomerState _state;
    private readonly ILogger<Customer> _logger; // Logger instance

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

    private Customer(string name, string email, string phone, ILogger<Customer> logger)
    {
        _logger = logger;
        _logger.LogInformation("Creating a new customer with Name: {Name}, Email: {Email}, Phone: {Phone}", name, email, phone);

        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        CustomerType = CustomerType.Standard;
        CustomerTier = CustomerTier.Bronze;
        _state = CustomerState.Created;

        _logger.LogInformation("Customer created with ID: {Id}", Id);
    }

    public static Customer Create(string name, string email, string phone, ILogger<Customer> logger)
    {
        logger.LogInformation("Creating customer with Name: {Name}, Email: {Email}, Phone: {Phone}", name, email, phone);

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));

        return new Customer(name, email, phone, logger);
    }

    public void Update(string name, string email, string phone, [CallerMemberName] string updatedBy = null)
    {
        _logger.LogInformation("Updating customer ID: {Id} by {UpdatedBy}", Id, updatedBy);

        ValidateStateTransition(CustomerState.Updated);

        Name = name;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
        _state = CustomerState.Updated;

        AddAuditLog($"Customer updated by {updatedBy}", updatedBy);

        _logger.LogInformation("Customer ID: {Id} updated successfully", Id);
    }

    public void Deactivate(string reason, [CallerMemberName] string deactivatedBy = null)
    {
        _logger.LogInformation("Deactivating customer ID: {Id} by {DeactivatedBy}. Reason: {Reason}", Id, deactivatedBy, reason);

        ValidateStateTransition(CustomerState.Deactivated);

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        _state = CustomerState.Deactivated;

        _metadata.AddOrUpdate("DeactivationReason", reason, (_, _) => reason);

        AddAuditLog($"Customer deactivated by {deactivatedBy}. Reason: {reason}", deactivatedBy);

        _logger.LogInformation("Customer ID: {Id} deactivated successfully", Id);
    }

    public void AddMetadata(string key, object value, [CallerMemberName] string addedBy = null)
    {
        _logger.LogInformation("Adding/updating metadata for customer ID: {Id}. Key: {Key}, Value: {Value}, AddedBy: {AddedBy}", Id, key, value, addedBy);

        _metadata.AddOrUpdate(key, value, (_, _) => value);

        AddAuditLog($"Metadata '{key}' added/updated by {addedBy}", addedBy);

        _logger.LogInformation("Metadata for customer ID: {Id} updated successfully", Id);
    }

    private void ValidateStateTransition(CustomerState newState)
    {
        _logger.LogInformation("Validating state transition for customer ID: {Id} from {_state} to {NewState}", Id, _state, newState);

        bool isValidTransition = _state switch
        {
            CustomerState.Created => true,
            CustomerState.Updated => newState != CustomerState.Created,
            CustomerState.Deactivated => false,
            _ => throw new InvalidOperationException($"Unknown state: {_state}")
        };

        if (!isValidTransition)
        {
            _logger.LogError("Invalid state transition for customer ID: {Id} from {_state} to {NewState}", Id, _state, newState);
            throw new InvalidOperationException($"Invalid state transition from {_state} to {newState}");
        }

        _logger.LogInformation("State transition for customer ID: {Id} is valid", Id);
    }

    private void AddAuditLog(string message, string performedBy)
    {
        _logger.LogInformation("Adding audit log for customer ID: {Id}. Message: {Message}, PerformedBy: {PerformedBy}", Id, message, performedBy);

        _auditLogs.Add(new CustomerAuditLog(Id, message, DateTime.UtcNow, performedBy));

        _logger.LogInformation("Audit log added for customer ID: {Id}", Id);
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
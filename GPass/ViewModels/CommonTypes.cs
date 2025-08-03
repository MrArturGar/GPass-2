using GPass.Models;

namespace GPass.ViewModels;

public enum OperationType
{
    Add,
    Delete,
    Update
}

public class EditOperation<T>
{
    public OperationType OperationType { get; set; }
    public T Object { get; set; }
    public T? OriginalObject { get; set; }
}
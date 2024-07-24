using Reports.Domain.Interfaces.Model;
using System;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class SelectableEntity<T> where T : ISelectable
{
    /// <summary>
    /// Sets as false the property "IsSelected".
    /// </summary>
    /// <param name="item"></param>
    public void ClearSelection(T item)
    {
        item.IsSelected = false;
    }

    /// <summary>
    /// Sets as true the property "IsSelected".
    /// </summary>
    /// <param name="item"></param>
    public void SetSelectionAsTrue(T item)
    {
        item.IsSelected = true;
    }
}


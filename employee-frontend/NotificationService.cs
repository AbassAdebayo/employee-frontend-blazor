using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

public class NotificationService
{

    public event EventHandler<string> ?OnShow;
    public event EventHandler ?OnClear;

    public void ShowNotification(string message)
    {
        
        OnShow?.Invoke(this, message);
    }

    public void ClearNotification()
    {
        OnClear?.Invoke(this, EventArgs.Empty);
    }
}

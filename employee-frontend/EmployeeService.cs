using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

public class EmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly NotificationService _notificationService;

    public EmployeeService(HttpClient httpClient, NotificationService notificationService)
    {
        _httpClient = httpClient;
        _notificationService = notificationService;
    }

   
    public async Task<bool> CreateEmployee(EmployeeModel employee)
    {
       
        var jsonContent = JsonSerializer.Serialize(employee);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("http://localhost:5281/api/employee/createemployee", content);
        response.EnsureSuccessStatusCode();
        
        if(response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(resultString);
        
            if (jsonObject.TryGetProperty("status", out var statusProperty) && statusProperty.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            else
            {
                return false;
            }

        }   
        return false;
    }
    public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
    {
        
        var response = await _httpClient.GetAsync("http://localhost:5281/api/employee");
      
        if (!response.IsSuccessStatusCode)
        {
            // Employee was not found
            _notificationService.ShowNotification("Data can not be empty!");
        }

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<EmployeeModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid employeeId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5281/api/employee/{employeeId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmployeeModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task UpdateEmployeeAsync(Guid employeeId, UpdateEmployeeRequestModel model)
    {
        EmployeeModel employee = await GetEmployeeByIdAsync(employeeId);

        // Map the properties from the update model to the employee
        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;
        employee.PhoneNumber = model.PhoneNumber;
        employee.Address = model.Address;
        employee.Email = model.Email;
        employee.Gender = model.Gender;

        var jsonContent = JsonSerializer.Serialize(employee);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"http://localhost:5281/api/employee/{employeeId}", content);
        response.EnsureSuccessStatusCode();
        _notificationService.ShowNotification("Employee updated successfullly");
    
    }

    public async Task DeleteEmployee(Guid employeeId)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5281/api/employee/{employeeId}");
        response.EnsureSuccessStatusCode();
        _notificationService.ShowNotification("Employee deleted successfullly");
    }

    public async Task<bool> EmployeeExistsByEmail(string email)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5281/api/employee/checkemailexists?email={email}");
        response.EnsureSuccessStatusCode();
        if(response.IsSuccessStatusCode)
        {
            var resultString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(resultString);
        
            if (jsonObject.TryGetProperty("status", out var statusProperty) && statusProperty.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            return false;
        }
    
    }

}


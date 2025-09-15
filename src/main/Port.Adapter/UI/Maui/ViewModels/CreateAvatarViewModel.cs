using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using MetroLog.Maui;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class CreateAvatarViewModel : BaseViewModel
{
    private readonly IAvatarApplicationService avatarApplicationService;
    private readonly IProgressService progressService;
    private AvatarServerConfiguration avatarServerConfiguration = new(string.Empty);

    public CreateAvatarViewModel(IAvatarApplicationService avatarApplicationService, IProgressService progressService) 
    {
        AssertionConcern.AssertArgumentNotNull(avatarApplicationService, nameof(avatarApplicationService));
        AssertionConcern.AssertArgumentNotNull(progressService, nameof(progressService));

        this.avatarApplicationService = avatarApplicationService;
        this.progressService = progressService;

        this.progressService.ProgressChanged += this.ProgressService_ProgressChanged;
        this.progressService.DescriptionChanged += this.ProgressService_DescriptionChanged;
    }

    private void ProgressService_DescriptionChanged(object sender, EventArgs e)
    {
        this.LoadingText = this.progressService.Description;
    }

    private void ProgressService_ProgressChanged(object sender, EventArgs e)
    {
        this.CreationProgress = this.progressService.Progress;
    }

    // Server Properties
    [ObservableProperty]
    private string serverName = string.Empty;
    [ObservableProperty]
    private string destination = string.Empty;

    // Avatar Properties
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private string ownerName = string.Empty;
    [ObservableProperty]
    private string ownerUserId = string.Empty;

    // Orchestration Properties
    [ObservableProperty]
    private string avatarIp = string.Empty;
    [ObservableProperty]
    private string un8yIp = string.Empty;
    [ObservableProperty]
    private string avatarInPort = string.Empty;
    [ObservableProperty]
    private string un8yBlazorPort = string.Empty;
    [ObservableProperty]
    private string keysPath = string.Empty;

    // Un8y Properties
    [ObservableProperty]
    private string certificatePassword = string.Empty;
    [ObservableProperty]
    private string basePath = string.Empty;

    // Event Sourcing Properties
    [ObservableProperty]
    private bool encryptionEnabled = false;
    [ObservableProperty]
    private string inProcessPrivateKeyPath = string.Empty;
    [ObservableProperty]
    private string privateKeyPath = string.Empty;
    [ObservableProperty]
    private string encryptedEventsKey = string.Empty;

    // Configuration File
    [ObservableProperty]
    private string configPath = string.Empty;

    // Tab Navigation Properties
    [ObservableProperty]
    private bool isEditTabSelected = true;
    [ObservableProperty]
    private bool isLogTabSelected = false;

    // Log Properties (for the log tab)
    [ObservableProperty]
    private string editorLogs = string.Empty;
    [ObservableProperty]
    private double creationProgress = 0.0;
    [ObservableProperty]
    private string loadingText = "Ready to create avatar...";

    // Validation Properties
    [ObservableProperty]
    private string avatarIpError = string.Empty;
    [ObservableProperty]
    private string un8yIpError = string.Empty;
    [ObservableProperty]
    private string avatarInPortError = string.Empty;
    [ObservableProperty]
    private string un8yBlazorPortError = string.Empty;
    [ObservableProperty]
    private string keysPathError = string.Empty;
    [ObservableProperty]
    private string inProcessPrivateKeyPathError = string.Empty;
    [ObservableProperty]
    private string encryptedEventsKeyError = string.Empty;

    // Property change handlers for automatic configuration updates
    partial void OnNameChanged(string value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            this.avatarServerConfiguration.Avatars[0].Name = value;
        }
    }

    partial void OnOwnerNameChanged(string value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            this.avatarServerConfiguration.Avatars[0].OwnerName = value;
        }
    }

    partial void OnOwnerUserIdChanged(string value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            this.avatarServerConfiguration.Avatars[0].OwnerUserId = value;
        }
    }

    partial void OnServerNameChanged(string value)
    {
        this.avatarServerConfiguration.ServerName = value;
    }

    partial void OnDestinationChanged(string value)
    {
        this.avatarServerConfiguration.Destination = value;
    }

    partial void OnAvatarIpChanged(string value)
    {
        this.ValidateIpAddress(value, nameof(this.AvatarIpError));
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Orchestration != null)
        {
            this.avatarServerConfiguration.Avatars[0].Orchestration.AvatarIp = value;
        }
    }

    partial void OnUn8yIpChanged(string value)
    {
        this.ValidateIpAddress(value, nameof(this.Un8yIpError));
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Orchestration != null)
        {
            this.avatarServerConfiguration.Avatars[0].Orchestration.Un8yIp = value;
        }
    }

    partial void OnAvatarInPortChanged(string value)
    {
        this.ValidatePort(value, nameof(this.AvatarInPortError));
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Orchestration != null && int.TryParse(value, out int port))
        {
            this.avatarServerConfiguration.Avatars[0].Orchestration.AvatarInPort = port;
        }
    }

    partial void OnUn8yBlazorPortChanged(string value)
    {
        this.ValidatePort(value, nameof(this.Un8yBlazorPortError));
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Orchestration != null && int.TryParse(value, out int port))
        {
            this.avatarServerConfiguration.Avatars[0].Orchestration.Un8yBlazorPort = port;
        }
    }

    partial void OnKeysPathChanged(string value)
    {
        this.ValidateKeysPath(value);
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Orchestration != null)
        {
            this.avatarServerConfiguration.Avatars[0].Orchestration.KeysPath = value;
        }
    }

    partial void OnCertificatePasswordChanged(string value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Un8y != null)
        {
            this.avatarServerConfiguration.Avatars[0].Un8y.CertificatePassword = value;
        }
    }

    partial void OnBasePathChanged(string value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].Un8y != null)
        {
            this.avatarServerConfiguration.Avatars[0].Un8y.BasePath = value;
        }
    }

    partial void OnEncryptionEnabledChanged(bool value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].EventSourcing != null)
        {
            this.avatarServerConfiguration.Avatars[0].EventSourcing.EncryptionEnabled = value;
        }
        
        // Clear encryption fields when encryption is disabled
        if (!value)
        {
            this.KeysPath = string.Empty;
            this.InProcessPrivateKeyPath = string.Empty;
            this.PrivateKeyPath = string.Empty;
            this.EncryptedEventsKey = string.Empty;
        }
        
        // Re-validate encryption fields when encryption is enabled/disabled
        this.ValidateKeysPath(this.KeysPath);
        this.ValidateInProcessPrivateKeyPath(this.InProcessPrivateKeyPath);
        this.ValidateEncryptedEventsKey(this.EncryptedEventsKey);
    }

    partial void OnEncryptedEventsKeyChanged(string value)
    {
        this.ValidateEncryptedEventsKey(value);
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].EventSourcing != null)
        {
            this.avatarServerConfiguration.Avatars[0].EventSourcing.EncryptedEventsKey = value;
        }
    }

    partial void OnInProcessPrivateKeyPathChanged(string value)
    {
        this.ValidateInProcessPrivateKeyPath(value);
        
        if (!string.IsNullOrEmpty(value))
        {
            var fileName = Path.GetFileName(value);
            this.PrivateKeyPath = $"/C/keys/{fileName}";
        }
        else
        {
            this.PrivateKeyPath = string.Empty;
        }
        
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].EventSourcing != null)
        {
            this.avatarServerConfiguration.Avatars[0].EventSourcing.InProcessPrivateKeyPath = value;
        }
    }

    partial void OnPrivateKeyPathChanged(string value)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && this.avatarServerConfiguration.Avatars[0].EventSourcing != null)
        {
            this.avatarServerConfiguration.Avatars[0].EventSourcing.PrivateKeyPath = value;
        }
    }


    private void ValidateIpAddress(string ipAddress, string errorPropertyName)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            this.SetErrorProperty(errorPropertyName, "IP address is required");
            return;
        }

        // Use regex to validate IP format (0-255.0-255.0-255.0-255)
        var ipPattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        
        if (!Regex.IsMatch(ipAddress, ipPattern))
        {
            this.SetErrorProperty(errorPropertyName, "Invalid IP address format (e.g., 172.20.10.4)");
            return;
        }

        // Additional validation using IPAddress.TryParse
        if (!IPAddress.TryParse(ipAddress, out _))
        {
            this.SetErrorProperty(errorPropertyName, "Invalid IP address");
            return;
        }

        this.SetErrorProperty(errorPropertyName, string.Empty);
    }

    private void ValidatePort(string port, string errorPropertyName)
    {
        if (string.IsNullOrEmpty(port))
        {
            this.SetErrorProperty(errorPropertyName, "Port is required");
            return;
        }

        if (!int.TryParse(port, out int portNumber))
        {
            this.SetErrorProperty(errorPropertyName, "Port must be a valid number");
            return;
        }

        if (portNumber < 1 || portNumber > 65535)
        {
            this.SetErrorProperty(errorPropertyName, "Port must be between 1 and 65535");
            return;
        }

        this.SetErrorProperty(errorPropertyName, string.Empty);
    }

    private void ValidateKeysPath(string keysPath)
    {
        if (this.EncryptionEnabled)
        {
            if (string.IsNullOrEmpty(keysPath))
            {
                this.KeysPathError = "Keys path is required when encryption is enabled";
                return;
            }
        }

        this.KeysPathError = string.Empty;
    }

    private void ValidateInProcessPrivateKeyPath(string inProcessPrivateKeyPath)
    {
        if (this.EncryptionEnabled)
        {
            if (string.IsNullOrEmpty(inProcessPrivateKeyPath))
            {
                this.InProcessPrivateKeyPathError = "In Process Private Key Path is required when encryption is enabled";
                return;
            }

            // Validate file extension
            var extension = Path.GetExtension(inProcessPrivateKeyPath)?.ToLower();
            if (extension != ".key" && extension != ".pem" && extension != ".p12" && extension != ".pfx")
            {
                this.InProcessPrivateKeyPathError = "Private key file must have .key, .pem, .p12, or .pfx extension";
                return;
            }
        }

        this.InProcessPrivateKeyPathError = string.Empty;
    }

    private void ValidateEncryptedEventsKey(string encryptedEventsKey)
    {
        if (this.EncryptionEnabled)
        {
            if (string.IsNullOrEmpty(encryptedEventsKey))
            {
                this.EncryptedEventsKeyError = "Encrypted Events Key is required when encryption is enabled";
                return;
            }
        }

        this.EncryptedEventsKeyError = string.Empty;
    }

    private bool ValidateAllFields()
    {
        // Re-validate all fields
        this.ValidateIpAddress(this.AvatarIp, nameof(this.AvatarIpError));
        this.ValidateIpAddress(this.Un8yIp, nameof(this.Un8yIpError));
        this.ValidatePort(this.AvatarInPort, nameof(this.AvatarInPortError));
        this.ValidatePort(this.Un8yBlazorPort, nameof(this.Un8yBlazorPortError));
        this.ValidateKeysPath(this.KeysPath);
        this.ValidateInProcessPrivateKeyPath(this.InProcessPrivateKeyPath);
        this.ValidateEncryptedEventsKey(this.EncryptedEventsKey);

        // Check if any validation errors exist
        return string.IsNullOrEmpty(this.AvatarIpError) &&
               string.IsNullOrEmpty(this.Un8yIpError) &&
               string.IsNullOrEmpty(this.AvatarInPortError) &&
               string.IsNullOrEmpty(this.Un8yBlazorPortError) &&
               string.IsNullOrEmpty(this.KeysPathError) &&
               string.IsNullOrEmpty(this.InProcessPrivateKeyPathError) &&
               string.IsNullOrEmpty(this.EncryptedEventsKeyError);
    }

    private void SetErrorProperty(string propertyName, string errorMessage)
    {
        switch (propertyName)
        {
            case nameof(this.AvatarIpError):
                this.AvatarIpError = errorMessage;
                break;
            case nameof(this.Un8yIpError):
                this.Un8yIpError = errorMessage;
                break;
            case nameof(this.AvatarInPortError):
                this.AvatarInPortError = errorMessage;
                break;
            case nameof(this.Un8yBlazorPortError):
                this.Un8yBlazorPortError = errorMessage;
                break;
            case nameof(this.KeysPathError):
                this.KeysPathError = errorMessage;
                break;
            case nameof(this.InProcessPrivateKeyPathError):
                this.InProcessPrivateKeyPathError = errorMessage;
                break;
            case nameof(this.EncryptedEventsKeyError):
                this.EncryptedEventsKeyError = errorMessage;
                break;
        }
    }


    [RelayCommand]
    private async Task ChooseConfigurationAsync()
    {
        if (IsBusy)
            return;

        this.IsBusy = true;

        try
        {
            var configFile = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = Constants.Messages.ChooseConfig
            });

            if (configFile is null)
            {
                await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Cancelled,
                    Constants.Messages.ChooseConfig, Constants.Prompts.Ok);
                return;
            }

            if (!configFile.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
            {
                await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Invalid,
                    Constants.Messages.InvalidConfig, Constants.Prompts.Ok);
                return;
            }

            this.ConfigPath = configFile.FullPath;
            this.avatarServerConfiguration = await this.avatarApplicationService.ReadAvatarConfiguration(this.ConfigPath);
            
            AssertionConcern.AssertArgumentNotNull(this.avatarServerConfiguration.Avatars, nameof(this.avatarServerConfiguration.Avatars));
            AssertionConcern.AssertArgumentTrue(this.avatarServerConfiguration.Avatars.Count() > 0, "AvatarServerConfiguration must contain at least one avatar");
            
            // Populate all fields from configuration
            this.ServerName = this.avatarServerConfiguration.ServerName;
            this.Destination = this.avatarServerConfiguration.Destination;
            
            var avatar = this.avatarServerConfiguration.Avatars[0];
            this.Name = avatar.Name;
            this.OwnerName = avatar.OwnerName;
            this.OwnerUserId = avatar.OwnerUserId;
            
            if (avatar.Orchestration != null)
            {
                this.AvatarIp = avatar.Orchestration.AvatarIp;
                this.Un8yIp = avatar.Orchestration.Un8yIp;
                this.AvatarInPort = avatar.Orchestration.AvatarInPort.ToString();
                this.Un8yBlazorPort = avatar.Orchestration.Un8yBlazorPort.ToString();
                this.KeysPath = avatar.Orchestration.KeysPath;
            }
            
            if (avatar.Un8y != null)
            {
                this.CertificatePassword = avatar.Un8y.CertificatePassword;
                this.BasePath = avatar.Un8y.BasePath;
            }
            
            if (avatar.EventSourcing != null)
            {
                this.EncryptionEnabled = avatar.EventSourcing.EncryptionEnabled;
                this.InProcessPrivateKeyPath = avatar.EventSourcing.InProcessPrivateKeyPath;
                this.PrivateKeyPath = avatar.EventSourcing.PrivateKeyPath;
                this.EncryptedEventsKey = avatar.EventSourcing.EncryptedEventsKey;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ChooseDestinationAsync()
    {
        try
        {
            var folder = await FolderPicker.PickAsync(default);
            if (folder.Folder != null)
            {
                this.Destination = folder.Folder.Path;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
    }

    [RelayCommand]
    private async Task ChooseKeysPathAsync()
    {
        try
        {
            var folder = await FolderPicker.PickAsync(default);
            if (folder.Folder != null)
            {
                this.KeysPath = folder.Folder.Path;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
    }

    [RelayCommand]
    private async Task ChooseInProcessPrivateKeyPathAsync()
    {
        try
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select Private Key File"
            });
            if (file != null)
            {
                this.InProcessPrivateKeyPath = file.FullPath;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
    }

    [RelayCommand]
    private void SelectEditTab()
    {
        this.IsEditTabSelected = true;
        this.IsLogTabSelected = false;
    }

    [RelayCommand]
    private void SelectLogTab()
    {
        this.IsEditTabSelected = false;
        this.IsLogTabSelected = true;
    }

    [RelayCommand]
    private async Task CreateAvatarAsync()
    {
        if (this.IsBusy)
            return;

        this.IsBusy = true;

        try
        {
            if (string.IsNullOrEmpty(this.ConfigPath))
            {
                await Shell.Current.DisplayAlert(Constants.Statuses.Invalid, Constants.Messages.ChooseConfig, Constants.Prompts.Ok);
                return;
            }

            // Validate all fields before creating avatar
            if (!this.ValidateAllFields())
            {
                await Shell.Current.DisplayAlert(Constants.Statuses.Invalid, "Please fix validation errors before creating avatar", Constants.Prompts.Ok);
                return;
            }

            // Switch to log tab to show progress
            this.SelectLogTab();
            
            // Start avatar creation directly
            await this.StartAvatarCreationAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    private async Task StartAvatarCreationAsync()
    {
        try
        {
            this.EditorLogs = string.Empty;
            this.LoadingText = "Starting avatar creation...";

            await this.avatarApplicationService.CreateAvatarAsync(this.avatarServerConfiguration);
            
            this.LoadingText = "Avatar creation completed successfully!";
            await Shell.Current.DisplayAlert(Constants.Statuses.Success, Constants.Messages.AvatarInstalled, Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            this.LoadingText = "Avatar creation failed!";
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
        finally
        {
            // Update logs
            var logController = new LogController();
            var logList = await logController.GetLogList();
            logList!.Reverse();
            this.EditorLogs = string.Join(Environment.NewLine, logList);
        }
    }

}

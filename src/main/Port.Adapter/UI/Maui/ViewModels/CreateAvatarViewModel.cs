using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Views;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;
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

        // Initialize ValidatableObject properties
        this.InitializeValidation();
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

    // Orchestration Properties (with validation)
    public ValidatableObject<string> AvatarIp { get; private set; }
    public ValidatableObject<string> Un8yIp { get; private set; }
    public ValidatableObject<string> AvatarInPort { get; private set; }
    public ValidatableObject<string> Un8yBlazorPort { get; private set; }
    public ValidatableObject<string> KeysPath { get; private set; }

    // Un8y Properties
    [ObservableProperty]
    private string certificatePassword = string.Empty;
    [ObservableProperty]
    private string basePath = string.Empty;

    // Event Sourcing Properties
    [ObservableProperty]
    private bool encryptionEnabled = false;
    public ValidatableObject<string> InProcessPrivateKeyPath { get; private set; }
    [ObservableProperty]
    private string privateKeyPath = string.Empty;
    public ValidatableObject<string> EncryptedEventsKey { get; private set; }

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

    // Note: Validation errors are now handled by ValidatableObject<T>.Errors property

    // Initialize validation for ValidatableObject properties
    private void InitializeValidation()
    {
        // Initialize ValidatableObject properties
        this.AvatarIp = new ValidatableObject<string>();
        this.Un8yIp = new ValidatableObject<string>();
        this.AvatarInPort = new ValidatableObject<string>();
        this.Un8yBlazorPort = new ValidatableObject<string>();
        this.KeysPath = new ValidatableObject<string>();
        this.InProcessPrivateKeyPath = new ValidatableObject<string>();
        this.EncryptedEventsKey = new ValidatableObject<string>();

        // Add validation rules
        this.AddValidationRules();

        // Subscribe to property changes for configuration updates
        this.SubscribeToValidationPropertyChanges();
    }

    private void AddValidationRules()
    {
        // IP Address validation rules
        this.AvatarIp.Validations.Add(new IsNotNullOrEmptyRule<string> 
        { 
            ValidationMessage = "Avatar IP address is required" 
        });
        this.AvatarIp.Validations.Add(new IpAddressRule<string> 
        { 
            ValidationMessage = "Invalid IP address format (e.g., 172.20.10.4)" 
        });

        this.Un8yIp.Validations.Add(new IsNotNullOrEmptyRule<string> 
        { 
            ValidationMessage = "Un8y IP address is required" 
        });
        this.Un8yIp.Validations.Add(new IpAddressRule<string> 
        { 
            ValidationMessage = "Invalid IP address format (e.g., 172.20.10.4)" 
        });

        // Port validation rules
        this.AvatarInPort.Validations.Add(new IsNotNullOrEmptyRule<string> 
        { 
            ValidationMessage = "Avatar port is required" 
        });
        this.AvatarInPort.Validations.Add(new PortNumberRule<string> 
        { 
            ValidationMessage = "Port must be between 1 and 65535" 
        });

        this.Un8yBlazorPort.Validations.Add(new IsNotNullOrEmptyRule<string> 
        { 
            ValidationMessage = "Un8y Blazor port is required" 
        });
        this.Un8yBlazorPort.Validations.Add(new PortNumberRule<string> 
        { 
            ValidationMessage = "Port must be between 1 and 65535" 
        });

        // Conditional validation for encryption-related fields
        this.KeysPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new IsNotNullOrEmptyRule<string> 
            { 
                ValidationMessage = "Keys path is required when encryption is enabled" 
            }
        ));

        this.InProcessPrivateKeyPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new IsNotNullOrEmptyRule<string> 
            { 
                ValidationMessage = "In Process Private Key Path is required when encryption is enabled" 
            }
        ));
        this.InProcessPrivateKeyPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new FileExtensionRule<string>(".key", ".pem", ".p12", ".pfx") 
            { 
                ValidationMessage = "Private key file must have .key, .pem, .p12, or .pfx extension" 
            }
        ));

        this.EncryptedEventsKey.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new IsNotNullOrEmptyRule<string> 
            { 
                ValidationMessage = "Encrypted Events Key is required when encryption is enabled" 
            }
        ));
    }

    private void SubscribeToValidationPropertyChanges()
    {
        this.AvatarIp.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.AvatarIp.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, o => o.AvatarIp = this.AvatarIp.Value);
            }
        };

        this.Un8yIp.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.Un8yIp.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, o => o.Un8yIp = this.Un8yIp.Value);
            }
        };

        this.AvatarInPort.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.AvatarInPort.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, (o, port) => o.AvatarInPort = port, this.AvatarInPort.Value, v => int.TryParse(v, out int port) ? port : 0);
            }
        };

        this.Un8yBlazorPort.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.Un8yBlazorPort.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, (o, port) => o.Un8yBlazorPort = port, this.Un8yBlazorPort.Value, v => int.TryParse(v, out int port) ? port : 0);
            }
        };

        this.KeysPath.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.KeysPath.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, o => o.KeysPath = this.KeysPath.Value);
            }
        };

        this.InProcessPrivateKeyPath.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.InProcessPrivateKeyPath.Validate();
                if (!string.IsNullOrEmpty(this.InProcessPrivateKeyPath.Value))
                {
                    var fileName = Path.GetFileName(this.InProcessPrivateKeyPath.Value);
                    this.PrivateKeyPath = $"/C/keys/{fileName}";
                }
                else
                {
                    this.PrivateKeyPath = string.Empty;
                }
                this.UpdateAvatarIfExists(a => a.EventSourcing, e => e.InProcessPrivateKeyPath = this.InProcessPrivateKeyPath.Value);
            }
        };

        this.EncryptedEventsKey.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.EncryptedEventsKey.Validate();
                this.UpdateAvatarIfExists(a => a.EventSourcing, e => e.EncryptedEventsKey = this.EncryptedEventsKey.Value);
            }
        };
    }

    // Helper methods to eliminate code redundancy
    private void UpdateAvatarIfExists<TSection>(Func<AvatarConfigurationItem, TSection> sectionSelector, Action<TSection> updateAction)
        where TSection : class
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            var avatar = this.avatarServerConfiguration.Avatars[0];
            var section = sectionSelector(avatar);
            if (section != null)
            {
                updateAction(section);
            }
        }
    }

    private void UpdateAvatarIfExists<TSection, TValue>(Func<AvatarConfigurationItem, TSection> sectionSelector, Action<TSection, TValue> updateAction, string value, Func<string, TValue> valueConverter)
        where TSection : class
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            var avatar = this.avatarServerConfiguration.Avatars[0];
            var section = sectionSelector(avatar);
            if (section != null)
            {
                var convertedValue = valueConverter(value);
                updateAction(section, convertedValue);
            }
        }
    }

    private void UpdateAvatarDirectProperty(Action<AvatarConfigurationItem> updateAction)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            updateAction(this.avatarServerConfiguration.Avatars[0]);
        }
    }

    // Property change handlers for automatic configuration updates
    partial void OnNameChanged(string value)
    {
        this.UpdateAvatarDirectProperty(a => a.Name = value);
    }

    partial void OnOwnerNameChanged(string value)
    {
        this.UpdateAvatarDirectProperty(a => a.OwnerName = value);
    }

    partial void OnOwnerUserIdChanged(string value)
    {
        this.UpdateAvatarDirectProperty(a => a.OwnerUserId = value);
    }

    partial void OnServerNameChanged(string value)
    {
        this.avatarServerConfiguration.ServerName = value;
    }

    partial void OnDestinationChanged(string value)
    {
        this.avatarServerConfiguration.Destination = value;
    }

    partial void OnCertificatePasswordChanged(string value)
    {
        this.UpdateAvatarIfExists(a => a.Un8y, u => u.CertificatePassword = value);
    }

    partial void OnBasePathChanged(string value)
    {
        this.UpdateAvatarIfExists(a => a.Un8y, u => u.BasePath = value);
    }

    partial void OnEncryptionEnabledChanged(bool value)
    {
        this.UpdateAvatarIfExists(a => a.EventSourcing, e => e.EncryptionEnabled = value);
        
        // Clear encryption fields when encryption is disabled
        if (!value)
        {
            this.KeysPath.Value = string.Empty;
            this.InProcessPrivateKeyPath.Value = string.Empty;
            this.PrivateKeyPath = string.Empty;
            this.EncryptedEventsKey.Value = string.Empty;
        }

        // Re-validate encryption fields since the condition has changed
        this.KeysPath.Validate();
        this.InProcessPrivateKeyPath.Validate();
        this.EncryptedEventsKey.Validate();
    }

    partial void OnPrivateKeyPathChanged(string value)
    {
        this.UpdateAvatarIfExists(a => a.EventSourcing, e => e.PrivateKeyPath = value);
    }


    private bool ValidateAllFields()
    {
        // Validate all ValidatableObject properties
        bool isAvatarIpValid = this.AvatarIp.Validate();
        bool isUn8yIpValid = this.Un8yIp.Validate();
        bool isAvatarInPortValid = this.AvatarInPort.Validate();
        bool isUn8yBlazorPortValid = this.Un8yBlazorPort.Validate();
        bool isKeysPathValid = this.KeysPath.Validate();
        bool isInProcessPrivateKeyPathValid = this.InProcessPrivateKeyPath.Validate();
        bool isEncryptedEventsKeyValid = this.EncryptedEventsKey.Validate();

        return isAvatarIpValid && isUn8yIpValid && isAvatarInPortValid && isUn8yBlazorPortValid &&
               isKeysPathValid && isInProcessPrivateKeyPathValid && isEncryptedEventsKeyValid;
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
                this.AvatarIp.Value = avatar.Orchestration.AvatarIp;
                this.Un8yIp.Value = avatar.Orchestration.Un8yIp;
                this.AvatarInPort.Value = avatar.Orchestration.AvatarInPort.ToString();
                this.Un8yBlazorPort.Value = avatar.Orchestration.Un8yBlazorPort.ToString();
                this.KeysPath.Value = avatar.Orchestration.KeysPath;
            }
            
            if (avatar.Un8y != null)
            {
                this.CertificatePassword = avatar.Un8y.CertificatePassword;
                this.BasePath = avatar.Un8y.BasePath;
            }
            
            if (avatar.EventSourcing != null)
            {
                this.EncryptionEnabled = avatar.EventSourcing.EncryptionEnabled;
                this.InProcessPrivateKeyPath.Value = avatar.EventSourcing.InProcessPrivateKeyPath;
                this.PrivateKeyPath = avatar.EventSourcing.PrivateKeyPath;
                this.EncryptedEventsKey.Value = avatar.EventSourcing.EncryptedEventsKey;
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

    // Helper method to eliminate folder picking code duplication
    private async Task<string> PickFolderAsync()
    {
        string result = string.Empty;
        
        try
        {
            var folder = await FolderPicker.PickAsync(default);
            result = folder.Folder?.Path ?? string.Empty;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert(Constants.Statuses.Error, ex.ToString(), Constants.Prompts.Ok);
        }
        
        return result;
    }

    [RelayCommand]
    private async Task ChooseDestinationAsync()
    {
        var folderPath = await this.PickFolderAsync();
        
        if (string.IsNullOrEmpty(folderPath))
            return;
            
        this.Destination = folderPath;
    }

    [RelayCommand]
    private async Task ChooseKeysPathAsync()
    {
        var folderPath = await this.PickFolderAsync();
        
        if (string.IsNullOrEmpty(folderPath))
            return;
            
        this.KeysPath.Value = folderPath;
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
            
            if (file == null)
                return;
                
            this.InProcessPrivateKeyPath.Value = file.FullPath;
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

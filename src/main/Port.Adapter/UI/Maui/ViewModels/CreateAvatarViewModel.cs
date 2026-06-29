using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ei8.Avatar.Installer.Application;
using ei8.Avatar.Installer.Application.Avatar;
using ei8.Avatar.Installer.Common;
using ei8.Avatar.Installer.Domain.Model.Configuration;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation;
using ei8.Avatar.Installer.Port.Adapter.UI.Maui.Validation.Rules;
using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using neurUL.Common.Domain.Model;
using System.Diagnostics;

namespace ei8.Avatar.Installer.Port.Adapter.UI.Maui.ViewModels;

public partial class CreateAvatarViewModel : BaseViewModel
{
    #region Constants
    public static class CreateAvatarConstants
    {
        public const string ReadyToCreateAvatar = "Ready to create avatar...";
        public const string StartingAvatarCreation = "Starting avatar creation...";
        public const string AvatarCreationCompleted = "Avatar creation completed successfully!";
        public const string AvatarCreationFailed = "Avatar creation failed!";
        public const string SelectPrivateKeyFile = "Select Private Key File";
        public const string FixValidationErrors = "Please fix validation errors before creating avatar!";
        public const string AvatarServerConfigurationMustContainAvatar = "AvatarServerConfiguration must contain at least one avatar";
        public const string PrivateKeyPathTemplate = "/C/keys/{0}";
        public const string JsonFileExtension = "json";

        // Validation Messages
        public const string AvatarNameRequired = "Avatar Name is required";
        public const string OwnerNameRequired = "Owner Name is required";
        public const string OwnerUserIdRequired = "Owner User ID is required";
        public const string TunnelLocalPortRequired = "Tunnel Local Port is required";
        public const string InvalidIpFormat = "Invalid IP address format (e.g., 172.20.10.4)";
        public const string PortRangeInvalid = "Port must be between 1 and 65535";
        public const string GraphPersistencePortRequired = "Graph persistence port is required";
        public const string KeysPathRequiredWhenEncryptionEnabled = "Keys path is required when encryption is enabled";
        public const string InProcessPrivateKeyPathRequired = "In Process Private Key Path is required when encryption is enabled";
        public const string PrivateKeyFileExtensionInvalid = "Private key file must have .key, .pem, .p12, or .pfx extension";
        public static readonly string[] PrivateKeyFileExtensions = { ".key", ".pem", ".p12", ".pfx" };
        public const string EncryptedEventsKeyRequired = "Encrypted Events Key is required when encryption is enabled";
        public const string CertificatePasswordRequired = "Certificate password is required when certificate path is set";
        public const string InProcessPrivateKeyFileNotFound = "In Process Private Key file was not found at resolved avatar path";
    }
    #endregion

    #region Fields
    private readonly IAvatarApplicationService avatarApplicationService;
    private readonly IProgressService progressService;
    private readonly ILogger<CreateAvatarViewModel> logger;
    private AvatarServerConfiguration avatarServerConfiguration = new();
    #endregion

    #region Constructors
    public CreateAvatarViewModel(
        IAvatarApplicationService avatarApplicationService,
        IProgressService progressService,
        ILogger<CreateAvatarViewModel> logger
    )
    {
        AssertionConcern.AssertArgumentNotNull(avatarApplicationService, nameof(avatarApplicationService));
        AssertionConcern.AssertArgumentNotNull(progressService, nameof(progressService));

        this.avatarApplicationService = avatarApplicationService;
        this.progressService = progressService;
        this.logger = logger ?? NullLogger<CreateAvatarViewModel>.Instance;

        this.progressService.ProgressChanged += this.ProgressService_ProgressChanged;
        this.progressService.DescriptionChanged += this.ProgressService_DescriptionChanged;

        // Initialize ValidatableObject properties
        this.InitializeValidation();
    }
    #endregion

    #region Properties
    [ObservableProperty]
    private string destination = string.Empty;

    // Avatar Properties
    public ValidatableObject<string> OwnerName { get; private set; }
    public ValidatableObject<string> OwnerUserId { get; private set; }

    // Orchestration Properties (with validation)
    public ValidatableObject<string> AvatarName { get; private set; }
    public ValidatableObject<string> TunnelLocalPort { get; private set; }
    public ValidatableObject<string> GraphPersistencePort { get; private set; }
    public ValidatableObject<string> KeysPath { get; private set; }

    // Un8y Properties
    public ValidatableObject<string> CertificatePassword { get; private set; }
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
    private string loadingText = CreateAvatarConstants.ReadyToCreateAvatar;

    // Note: Validation errors are now handled by ValidatableObject<T>.Errors property
    #endregion

    #region Methods
    private void ProgressService_DescriptionChanged(object sender, EventArgs e)
    {
        this.LoadingText = this.progressService.Description;
    }

    private void ProgressService_ProgressChanged(object sender, EventArgs e)
    {
        this.CreationProgress = this.progressService.Progress;
    }

    // Initialize validation for ValidatableObject properties
    private void InitializeValidation()
    {
        // Initialize ValidatableObject properties
        this.OwnerName = new ValidatableObject<string>();
        this.OwnerUserId = new ValidatableObject<string>();
        this.AvatarName = new ValidatableObject<string>();
        this.TunnelLocalPort = new ValidatableObject<string>();
        this.GraphPersistencePort = new ValidatableObject<string>();
        this.KeysPath = new ValidatableObject<string>();
        this.InProcessPrivateKeyPath = new ValidatableObject<string>();
        this.EncryptedEventsKey = new ValidatableObject<string>();
        this.CertificatePassword = new ValidatableObject<string>();

        // Add validation rules
        this.AddValidationRules();

        // Subscribe to property changes for configuration updates
        this.SubscribeToValidationPropertyChanges();
    }

    private void AddValidationRules()
    {
        this.OwnerName.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = CreateAvatarConstants.OwnerNameRequired
        });
        this.OwnerUserId.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = CreateAvatarConstants.OwnerUserIdRequired
        });
        this.AvatarName.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = CreateAvatarConstants.AvatarNameRequired
        });
        this.TunnelLocalPort.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = CreateAvatarConstants.TunnelLocalPortRequired
        });
        this.TunnelLocalPort.Validations.Add(new PortNumberRule<string>
        {
            ValidationMessage = CreateAvatarConstants.PortRangeInvalid
        });

        // Graph persistence port validation
        this.GraphPersistencePort.Validations.Add(new IsNotNullOrEmptyRule<string> 
        { 
            ValidationMessage = CreateAvatarConstants.GraphPersistencePortRequired 
        });
        this.GraphPersistencePort.Validations.Add(new PortNumberRule<string> 
        { 
            ValidationMessage = CreateAvatarConstants.PortRangeInvalid 
        });

        // Conditional validation for encryption-related fields
        this.KeysPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new IsNotNullOrEmptyRule<string> 
            { 
                ValidationMessage = CreateAvatarConstants.KeysPathRequiredWhenEncryptionEnabled 
            }
        ));

        this.InProcessPrivateKeyPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new IsNotNullOrEmptyRule<string> 
            { 
                ValidationMessage = CreateAvatarConstants.InProcessPrivateKeyPathRequired 
            }
        ));
        this.InProcessPrivateKeyPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new FileExtensionRule<string>(CreateAvatarConstants.PrivateKeyFileExtensions) 
            { 
                ValidationMessage = CreateAvatarConstants.PrivateKeyFileExtensionInvalid 
            }
        ));
        this.InProcessPrivateKeyPath.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new FuncValidationRule<string>(_ =>
            {
                var resolvedPath = this.ResolveInProcessPrivateKeyPathForValidation();
                return string.IsNullOrWhiteSpace(resolvedPath) || File.Exists(resolvedPath);
            })
            {
                ValidationMessage = CreateAvatarConstants.InProcessPrivateKeyFileNotFound
            }
        ));

        this.EncryptedEventsKey.Validations.Add(new ConditionalRule<string>(
            () => this.EncryptionEnabled,
            new IsNotNullOrEmptyRule<string> 
            { 
                ValidationMessage = CreateAvatarConstants.EncryptedEventsKeyRequired 
            }
        ));

        this.CertificatePassword.Validations.Add(new ConditionalRule<string>(
            () => this.IsCertificatePathConfigured(),
            new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = CreateAvatarConstants.CertificatePasswordRequired
            }
        ));
    }

    private bool IsCertificatePathConfigured() =>
        this.avatarServerConfiguration?.Avatars?.Count() > 0 &&
        !string.IsNullOrWhiteSpace(this.avatarServerConfiguration.Avatars[0].Un8y?.CertificatePath);

    private IEnumerable<(string Label, ValidatableObject<string> Property)> GetValidatableFields() =>
        new (string Label, ValidatableObject<string> Property)[]
        {
            ("Owner Name", this.OwnerName),
            ("Owner User ID", this.OwnerUserId),
            ("Avatar IP", this.AvatarName),
            ("Tunnel Local Port", this.TunnelLocalPort),
            ("Graph Persistence Port", this.GraphPersistencePort),
            ("Keys Path", this.KeysPath),
            ("In Process Private Key Path", this.InProcessPrivateKeyPath),
            ("Encrypted Events Key", this.EncryptedEventsKey),
            ("Certificate Password", this.CertificatePassword),
        };

    private void SubscribeToValidationPropertyChanges()
    {
        this.OwnerName.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.OwnerName.Validate();
                this.UpdateAvatarIfExists(a => a.OwnerName = this.OwnerName.Value);
            }
        };

        this.OwnerUserId.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.OwnerUserId.Validate();
                this.UpdateAvatarIfExists(a => a.OwnerUserId = this.OwnerUserId.Value);
            }
        };

        this.AvatarName.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.AvatarName.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, o => o.AvatarName = this.AvatarName.Value);
            }
        };

        this.TunnelLocalPort.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.TunnelLocalPort.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, (o, port) => o.TunnelLocalPort = port, this.TunnelLocalPort.Value, v => int.TryParse(v, out int port) ? port : 0);
            }
        };

        this.GraphPersistencePort.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.GraphPersistencePort.Validate();
                this.UpdateAvatarIfExists(a => a.Orchestration, (o, port) => o.GraphPersistencePort = port, this.GraphPersistencePort.Value, v => int.TryParse(v, out int port) ? port : 0);
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
                    this.PrivateKeyPath = string.Format(CreateAvatarConstants.PrivateKeyPathTemplate, fileName);
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

        this.CertificatePassword.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ValidatableObject<string>.Value))
            {
                this.CertificatePassword.Validate();
                this.UpdateAvatarIfExists(a => a.Un8y, u => u.CertificatePassword = this.CertificatePassword.Value);
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

    private void UpdateAvatarIfExists(Action<AvatarConfigurationItem> updateAction)
    {
        if (this.avatarServerConfiguration?.Avatars?.Count() > 0)
        {
            updateAction(this.avatarServerConfiguration.Avatars[0]);
        }
    }

    // Property change handlers for automatic configuration updates
    partial void OnDestinationChanged(string value)
    {
        this.avatarServerConfiguration.Destination = value;
        this.InProcessPrivateKeyPath.Validate();
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
        this.CertificatePassword.Validate();
    }

    partial void OnPrivateKeyPathChanged(string value)
    {
        this.UpdateAvatarIfExists(a => a.EventSourcing, e => e.PrivateKeyPath = value);
    }

    private string GetAvatarDirectoryForValidation()
    {
        var avatarDirectory = string.Empty;

        if (this.avatarServerConfiguration?.Avatars?.Count() > 0 && !string.IsNullOrWhiteSpace(this.Destination))
        {
            var avatarName = this.avatarServerConfiguration.Avatars[0].Orchestration?.AvatarName;
            if (!string.IsNullOrWhiteSpace(avatarName))
            {
                avatarDirectory = Path.Combine(this.Destination, avatarName);
            }
        }

        return avatarDirectory;
    }

    private string ResolveInProcessPrivateKeyPathForValidation() =>
        PathHelper.ResolveInProcessPrivateKeyPath(
            this.InProcessPrivateKeyPath.Value,
            this.GetAvatarDirectoryForValidation()
        );


    private bool ValidateAllFields() =>
        this.GetValidatableFields().All(field => field.Property.Validate());

    private List<string> GetValidationFailures()
    {
        var failures = new List<string>();

        foreach (var (label, property) in this.GetValidatableFields())
        {
            if (!property.IsValid)
            {
                failures.Add($"{label}: {string.Join("; ", property.Errors)}");
            }
        }

        return failures;
    }


    [RelayCommand]
    private async Task ChooseConfigurationAsync()
    {
        if (!this.IsBusy)
        {
            this.IsBusy = true;

            try
            {
                var configFile = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = ei8.Avatar.Installer.Common.Constants.Messages.ChooseConfig
                });

                if (configFile is not null)
                {
                    if (configFile.FileName.EndsWith(CreateAvatarConstants.JsonFileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        this.ConfigPath = configFile.FullPath;
                        this.avatarServerConfiguration = await this.avatarApplicationService.ReadAvatarConfiguration(this.ConfigPath);
                        
                        AssertionConcern.AssertArgumentNotNull(this.avatarServerConfiguration.Avatars, nameof(this.avatarServerConfiguration.Avatars));
                        AssertionConcern.AssertArgumentTrue(this.avatarServerConfiguration.Avatars.Count() > 0, CreateAvatarConstants.AvatarServerConfigurationMustContainAvatar);
                        
                        // Populate all fields from configuration
                        this.Destination = this.avatarServerConfiguration.Destination;
                        
                        var avatar = this.avatarServerConfiguration.Avatars[0];
                        this.OwnerName.Value = avatar.OwnerName;
                        this.OwnerUserId.Value = avatar.OwnerUserId;
                        
                        if (avatar.Orchestration != null)
                        {
                            this.AvatarName.Value = avatar.Orchestration.AvatarName;
                            this.TunnelLocalPort.Value = avatar.Orchestration.TunnelLocalPort.ToString();
                            this.GraphPersistencePort.Value = avatar.Orchestration.GraphPersistencePort.ToString();
                            this.KeysPath.Value = avatar.Orchestration.KeysPath;
                        }
                        
                        if (avatar.Un8y != null)
                        {
                            this.CertificatePassword.Value = avatar.Un8y.CertificatePassword;
                            this.BasePath = avatar.Un8y.BasePath;
                        }
                        
                        if (avatar.EventSourcing != null)
                        {
                            this.EncryptionEnabled = avatar.EventSourcing.EncryptionEnabled;
                            this.InProcessPrivateKeyPath.Value = avatar.EventSourcing.InProcessPrivateKeyPath;
                            this.PrivateKeyPath = avatar.EventSourcing.PrivateKeyPath;
                            this.EncryptedEventsKey.Value = avatar.EventSourcing.EncryptedEventsKey;
                        }

                        foreach (var (_, property) in this.GetValidatableFields())
                        {
                            property.Validate();
                        }
                    }
                    else
                    {
                        await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Invalid,
                            Constants.Messages.InvalidConfig, Constants.Prompts.Ok);
                    }
                }
                else
                {
                    await Shell.Current.CurrentPage.DisplayAlert(Constants.Statuses.Cancelled,
                        Constants.Messages.ChooseConfig, Constants.Prompts.Ok);
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
        
        if (!string.IsNullOrEmpty(folderPath))
        {
            this.Destination = folderPath;
        }
    }

    [RelayCommand]
    private async Task ChooseKeysPathAsync()
    {
        var folderPath = await this.PickFolderAsync();
        
        if (!string.IsNullOrEmpty(folderPath))
        {
            this.KeysPath.Value = folderPath;
        }
    }

    [RelayCommand]
    private async Task ChooseInProcessPrivateKeyPathAsync()
    {
        try
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = CreateAvatarConstants.SelectPrivateKeyFile
            });
            
            if (file is not null)
            {
                this.InProcessPrivateKeyPath.Value = file.FullPath;
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
        if (!this.IsBusy)
        {
            this.IsBusy = true;

            try
            {
                if (!string.IsNullOrEmpty(this.ConfigPath))
                {
                    // Validate all fields before creating avatar
                    if (this.ValidateAllFields())
                    {
                        // Switch to log tab to show progress
                        this.SelectLogTab();
                        
                        // Start avatar creation directly
                        await this.StartAvatarCreationAsync();
                    }
                    else
                    {
                        var failures = this.GetValidationFailures();
                        if (failures.Any())
                        {
                            this.logger.LogWarning(
                                "Create avatar validation failed. Missing/invalid fields: {failures}",
                                string.Join(" | ", failures)
                            );
                        }

                        var message = failures.Any()
                            ? $"{CreateAvatarConstants.FixValidationErrors}{Environment.NewLine}- {string.Join($"{Environment.NewLine}- ", failures)}"
                            : CreateAvatarConstants.FixValidationErrors;

                        await Shell.Current.DisplayAlert(Constants.Statuses.Invalid, message, Constants.Prompts.Ok);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert(Constants.Statuses.Invalid, Constants.Messages.ChooseConfig, Constants.Prompts.Ok);
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
    }

    private async Task StartAvatarCreationAsync()
    {
        try
        {
            this.EditorLogs = string.Empty;
            this.LoadingText = CreateAvatarConstants.StartingAvatarCreation;

            await this.avatarApplicationService.CreateAvatarAsync(this.avatarServerConfiguration);
            
            this.LoadingText = CreateAvatarConstants.AvatarCreationCompleted;
            await Shell.Current.DisplayAlert(Constants.Statuses.Success, Constants.Messages.AvatarInstalled, Constants.Prompts.Ok);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            this.LoadingText = CreateAvatarConstants.AvatarCreationFailed;
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
    #endregion
}

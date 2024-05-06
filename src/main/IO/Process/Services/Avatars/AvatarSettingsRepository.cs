﻿using ei8.Avatar.Installer.Domain.Model.Avatars.Settings;
using ei8.Avatar.Installer.Domain.Model;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ei8.Avatar.Installer.Domain.Model.Avatars;
using ei8.Avatar.Installer.Common;

namespace ei8.Avatar.Installer.IO.Process.Services.Avatars;

public class AvatarSettingsRepository : IAvatarSettingsRepository
{
    private readonly IAvatarContextService avatarContextService;

    public AvatarSettingsRepository(IAvatarContextService avatarContextService)
    {
        AssertionConcern.AssertArgumentNotNull(avatarContextService, nameof(avatarContextService));

        this.avatarContextService = avatarContextService;
    }

    public async Task<AvatarSettings> GetAsync()
    {
        var path = Path.Combine(this.avatarContextService.Avatar.Id, Constants.Filenames.VariablesEnv);
        var avatarSettings = new AvatarSettings();

        var envFile = await File.ReadAllLinesAsync(path);

        foreach (var line in envFile)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                continue;

            var index = line.IndexOf('=');

            // If no "=" found, go to the next line
            if (index == -1) continue;

            var key = line[..index].Trim();
            var value = line[(index + 1)..].Trim();

            switch (key.ToUpper())
            {
                #region EventSourcingSettings
                case Constants.EventSourcingSettingsEnv.DatabasePath:
                    avatarSettings.EventSourcing.DatabasePath = value;
                    break;
                case Constants.EventSourcingSettingsEnv.DisplayErrorTraces:
                    bool displayErrorTraces;
                    if (bool.TryParse(value, out displayErrorTraces))
                        avatarSettings.EventSourcing.DisplayErrorTraces = displayErrorTraces;
                    break;
                #endregion

                #region CortexGraphSettings
                case Constants.CortexGraphSettingsEnv.PollInterval:
                    int pollInterval;
                    if (int.TryParse(value, out pollInterval))
                        avatarSettings.CortexGraph.PollInterval = pollInterval;
                    break;
                case Constants.CortexGraphSettingsEnv.DbName:
                    avatarSettings.CortexGraph.DbName = value;
                    break;
                case Constants.CortexGraphSettingsEnv.DbUsername:
                    avatarSettings.CortexGraph.DbUsername = value;
                    break;
                case Constants.CortexGraphSettingsEnv.DbPassword:
                    avatarSettings.CortexGraph.DbPassword = value;
                    break;
                case Constants.CortexGraphSettingsEnv.DbUrl:
                    avatarSettings.CortexGraph.DbUrl = value;
                    break;
                case Constants.CortexGraphSettingsEnv.DefaultRelativeValues:
                    int defaultRelativeValues;
                    if (int.TryParse(value, out defaultRelativeValues))
                        avatarSettings.CortexGraph.DefaultRelativeValues = defaultRelativeValues;
                    break;
                case Constants.CortexGraphSettingsEnv.DefaultNeuronActiveValues:
                    int defaultNeuronActiveValues;
                    if (int.TryParse(value, out defaultNeuronActiveValues))
                        avatarSettings.CortexGraph.DefaultRelativeValues = defaultNeuronActiveValues;
                    break;
                case Constants.CortexGraphSettingsEnv.DefaultTerminalActiveValues:
                    int defaultTerminalActiveValues;
                    if (int.TryParse(value, out defaultTerminalActiveValues))
                        avatarSettings.CortexGraph.DefaultRelativeValues = defaultTerminalActiveValues;
                    break;
                case Constants.CortexGraphSettingsEnv.DefaultPageSize:
                    int defaultPageSize;
                    if (int.TryParse(value, out defaultPageSize))
                        avatarSettings.CortexGraph.DefaultRelativeValues = defaultPageSize;
                    break;
                case Constants.CortexGraphSettingsEnv.DefaultPage:
                    int defaultPage;
                    if (int.TryParse(value, out defaultPage))
                        avatarSettings.CortexGraph.DefaultRelativeValues = defaultPage;
                    break;
                case Constants.CortexGraphSettingsEnv.ArangoRootPassword:
                    avatarSettings.CortexGraph.ArangoRootPassword = value;
                    break;
                #endregion

                #region AvatarApiSettings
                case Constants.AvatarApiSettingsEnv.ResourceDatabasePath:
                    avatarSettings.AvatarApi.ResourceDatabasePath = value;
                    break;
                case Constants.AvatarApiSettingsEnv.RequireAuthentication:
                    bool requireAuthentication;
                    if (bool.TryParse(value, out requireAuthentication))
                        avatarSettings.AvatarApi.RequireAuthentication = requireAuthentication;
                    break;
                case Constants.AvatarApiSettingsEnv.AnonymousUserId:
                    Guid anonymousUserId;
                    if (Guid.TryParse(value, out anonymousUserId))
                        avatarSettings.AvatarApi.AnonymousUserId = anonymousUserId;
                    break;
                case Constants.AvatarApiSettingsEnv.ProxyUserId:
                    Guid proxyUserId;
                    if (Guid.TryParse(value, out proxyUserId))
                        avatarSettings.AvatarApi.ProxyUserId = proxyUserId;
                    break;
                case Constants.AvatarApiSettingsEnv.TokenIssuerUrl:
                    avatarSettings.AvatarApi.TokenIssuerUrl = value;
                    break;
                case Constants.AvatarApiSettingsEnv.ApiName:
                    avatarSettings.AvatarApi.ApiName = value;
                    break;
                case Constants.AvatarApiSettingsEnv.ApiSecret:
                    avatarSettings.AvatarApi.ApiSecret = value;
                    break;
                case Constants.AvatarApiSettingsEnv.ValidateServerCertificate:
                    bool validateServerCertificate;
                    if (bool.TryParse(value, out validateServerCertificate))
                        avatarSettings.AvatarApi.ValidateServerCertificate = validateServerCertificate;
                    break;
                #endregion

                #region IdentityAccessSettings
                case Constants.IdentityAccessSettingsEnv.UserDatabasePath:
                    avatarSettings.IdentityAccess.UserDatabasePath = value;
                    break;
                #endregion

                #region CortexLibrarySettings
                case Constants.CortexLibrarySettingsEnv.NeuronsUrl:
                    avatarSettings.CortexLibrary.NeuronsUrl = value;
                    break;
                case Constants.CortexLibrarySettingsEnv.TerminalsUrl:
                    avatarSettings.CortexLibrary.TerminalsUrl = value;
                    break;
                #endregion

                #region CortexDiaryNucleusSettings
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsDatabasePath:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsDatabasePath = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPollingIntervalSecs:
                    int subscriptionsPollingIntervalSecs;
                    if (int.TryParse(value, out subscriptionsPollingIntervalSecs))
                        avatarSettings.CortexDiaryNucleus.SubscriptionsPollingIntervalSecs = subscriptionsPollingIntervalSecs;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPushOwner:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsPushOwner = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPushPublicKey:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsPushPublicKey = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPushPrivateKey:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsPushPrivateKey = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpServerAddress:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpServerAddress = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpPort:
                    int subscriptionsSmtpPort;
                    if (int.TryParse(value, out subscriptionsSmtpPort))
                        avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpPort = subscriptionsSmtpPort;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpUseSsl:
                    bool subscriptionsSmtpUseSsl;
                    if (bool.TryParse(value, out subscriptionsSmtpUseSsl))
                        avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpUseSsl = subscriptionsSmtpUseSsl;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderName:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderName = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderAddress:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderAddress = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderUsername:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderUsername = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderPassword:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderPassword = value;
                    break;
                case Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsCortexGraphOutBaseUrl:
                    avatarSettings.CortexDiaryNucleus.SubscriptionsCortexGraphOutBaseUrl = value;
                    break;
                #endregion

                default:
                    break;
            }
        }

        return avatarSettings;
    }

    public async Task SaveAsync(AvatarSettings avatarSettings)
    {
        AssertionConcern.AssertArgumentNotNull(avatarSettings, nameof(avatarSettings));

        var path = Path.Combine(this.avatarContextService.Avatar.Id, Constants.Filenames.VariablesEnv);
        var envVariables = new Dictionary<string, string>();

        var envFile = await File.ReadAllLinesAsync(path);

        foreach (var line in envFile)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                continue;

            var index = line.IndexOf('=');

            // If no "=" found, go to the next line
            if (index == -1) continue;

            var key = line[..index].Trim();
            var value = line[(index + 1)..].Trim();

            envVariables[key] = value;
        }

        // Update key-value pairs
        #region EventSourcingSettings
        envVariables[Constants.EventSourcingSettingsEnv.DatabasePath] = avatarSettings.EventSourcing.DatabasePath;
        envVariables[Constants.EventSourcingSettingsEnv.DisplayErrorTraces] = avatarSettings.EventSourcing.DisplayErrorTraces.ToString();
        #endregion

        #region CortexGraphSettingsEnv
        envVariables[Constants.CortexGraphSettingsEnv.PollInterval] = avatarSettings.CortexGraph.PollInterval.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DbName] = avatarSettings.CortexGraph.DbName.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DbUsername] = avatarSettings.CortexGraph.DbUsername.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DbPassword] = avatarSettings.CortexGraph.DbPassword.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DbUrl] = avatarSettings.CortexGraph.DbUrl.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DefaultRelativeValues] = avatarSettings.CortexGraph.DefaultRelativeValues.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DefaultNeuronActiveValues] = avatarSettings.CortexGraph.DefaultNeuronActiveValues.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DefaultTerminalActiveValues] = avatarSettings.CortexGraph.DefaultTerminalActiveValues.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DefaultPageSize] = avatarSettings.CortexGraph.DefaultPageSize.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.DefaultPage] = avatarSettings.CortexGraph.DefaultPage.ToString();
        envVariables[Constants.CortexGraphSettingsEnv.ArangoRootPassword] = avatarSettings.CortexGraph.ArangoRootPassword.ToString();
        #endregion

        #region AvatarApiSettingsEnv
        envVariables[Constants.AvatarApiSettingsEnv.ResourceDatabasePath] = avatarSettings.AvatarApi.ResourceDatabasePath.ToString();
        envVariables[Constants.AvatarApiSettingsEnv.RequireAuthentication] = avatarSettings.AvatarApi.RequireAuthentication.ToString();
        envVariables[Constants.AvatarApiSettingsEnv.AnonymousUserId] = avatarSettings.AvatarApi.AnonymousUserId.ToString();
        envVariables[Constants.AvatarApiSettingsEnv.TokenIssuerUrl] = avatarSettings.AvatarApi.TokenIssuerUrl.ToString();
        envVariables[Constants.AvatarApiSettingsEnv.ApiName] = avatarSettings.AvatarApi.ApiName.ToString();
        envVariables[Constants.AvatarApiSettingsEnv.ApiSecret] = avatarSettings.AvatarApi.ApiSecret.ToString();
        envVariables[Constants.AvatarApiSettingsEnv.ValidateServerCertificate] = avatarSettings.AvatarApi.ValidateServerCertificate.ToString();
        #endregion

        #region IdentityAccessSettings
        envVariables[Constants.IdentityAccessSettingsEnv.UserDatabasePath] = avatarSettings.IdentityAccess.UserDatabasePath.ToString();
        #endregion

        #region CortexLibrarySettings
        envVariables[Constants.CortexLibrarySettingsEnv.NeuronsUrl] = avatarSettings.CortexLibrary.NeuronsUrl.ToString();
        envVariables[Constants.CortexLibrarySettingsEnv.TerminalsUrl] = avatarSettings.CortexLibrary.TerminalsUrl.ToString();
        #endregion

        #region CortexDiaryNucleusSettings
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsDatabasePath] = avatarSettings.CortexDiaryNucleus.SubscriptionsDatabasePath.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPollingIntervalSecs] = avatarSettings.CortexDiaryNucleus.SubscriptionsPollingIntervalSecs.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPushOwner] = avatarSettings.CortexDiaryNucleus.SubscriptionsPushOwner.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPushPublicKey] = avatarSettings.CortexDiaryNucleus.SubscriptionsPushPublicKey.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsPushPrivateKey] = avatarSettings.CortexDiaryNucleus.SubscriptionsPushPrivateKey.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpServerAddress] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpServerAddress.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpPort] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpPort.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpUseSsl] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpUseSsl.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderName] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderName.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderAddress] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderAddress.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderUsername] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderUsername.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsSmtpSenderPassword] = avatarSettings.CortexDiaryNucleus.SubscriptionsSmtpSenderPassword.ToString();
        envVariables[Constants.CortexDiaryNucleusSettingsEnv.SubscriptionsCortexGraphOutBaseUrl] = avatarSettings.CortexDiaryNucleus.SubscriptionsCortexGraphOutBaseUrl.ToString();
        #endregion

        // Write changes to the file
        using var writer = new StreamWriter(path);

        foreach (var keyValuePair in envVariables)
        {
            writer.WriteLine($"{keyValuePair.Key}={keyValuePair.Value}");
        }
    }
}

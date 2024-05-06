using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Avatar.Installer.Domain.Model.Avatars.Settings;

public class CortexDiaryNucleusSettings
{
    public string SubscriptionsDatabasePath { get; set; }
    public int SubscriptionsPollingIntervalSecs { get; set; }
    public string SubscriptionsPushOwner { get; set; }
    public string SubscriptionsPushPublicKey { get; set; }
    public string SubscriptionsPushPrivateKey { get; set; }
    public string SubscriptionsSmtpServerAddress { get; set; }
    public int SubscriptionsSmtpPort { get; set; }
    public bool SubscriptionsSmtpUseSsl { get; set; }
    public string SubscriptionsSmtpSenderName { get; set; }
    public string SubscriptionsSmtpSenderAddress { get; set; }
    public string SubscriptionsSmtpSenderUsername { get; set; }
    public string SubscriptionsSmtpSenderPassword { get; set; }
    public string SubscriptionsCortexGraphOutBaseUrl { get; set; }
}

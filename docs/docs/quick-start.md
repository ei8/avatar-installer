# Quick Start

The **Quick Start Guide** will help you set up **ei8 Avatar** quickly with step-by-step instructions.  
Follow the installation steps carefully to ensure everything runs smoothly on your system.

<br>

### 📚 Prerequisites

Before installing **ei8 Avatar**, ensure you have the following:

- **Operating System**: Windows 10 or higher (64-bit).
- **Virtualization**: Enabled in BIOS (required for Docker).
- [**Docker Desktop**](https://www.docker.com): Latest version installed and running.
- **CPU**: At least **quad-core** (Intel i5 / Ryzen 5 or better recommended).
- **RAM**: Minimum **8GB** (16GB recommended for better performance).
- **Storage**: At least **20GB of free disk space** (SSD recommended).
- **Internet**: Stable connection for downloading dependencies.
- **Admin Privileges**: Required for installation and running certain commands.

<br>

### 🚀 Installation Steps

This section provides a step-by-step guide to installing **ei8 Avatar** and setting it up for the first time.

#### 1️⃣ Enable File Extensions on Windows

Before proceeding, enable file extensions to ensure correct file handling:

1. Open the `Settings` app on Windows.
2. Search for `Show file extensions` in the file explorer and click on the first entry on the dropdown.

<img src="../images/search-for-show-file-extensions.png" alt="Search for Show File Extensions" style="display: block; margin: auto; width: 50%; max-width:100%;">

3. Turn on the `Show file extensions` setting as shown.

<img src="../images/turn-on-show-file-extensions.png" alt="Turn on Show File Extensions" style="display: block; margin: auto; max-width:100%;">

<br>

#### 2️⃣ Create Configuration File

1. Open `Notepad`
2. Copy and paste the following JSON content:

```json
{
  "server_name": "myserver",
  "avatars": [
    {
      "name": "sample",
      "owner_name": "John Doe",
      "owner_user_id": "john@sample.com",
      "un8y": 
      {
        "oidc_authority_url": "https://192.168.1.100.nip.io:65102",
        "client_id": "un8y-sample",
        "requested_scopes": "openid,profile,email,avatarapi-sample,offline_access",
        "base_path": "",
        "certificate_password": ""
      },
      "orchestration": 
      {
        "avatar_ip": "192.168.1.100",
        "un8y_ip": "192.168.1.100",
        "avatar_in_port": 65101,
        "un8y_blazor_port": 65103,
        "keys_path": "/e/ei8/keys/sample"
      },
      "event_sourcing": 
      {
        "private_key_path": "/C/keys/private.key",
        "in_process_private_key_path": "e:\\ei8\\keys\\sample\\private.key",
        "encryption_enabled": true,
        "encrypted_events_key": ""
      }
    }
  ],
  "destination": "%USERPROFILE%\\Documents\\ei8\\Avatar",
  "template_url": "https://github.com/ei8/avatar-template.git"
}
```
> [!NOTE]
> More sample config files available [here](https://github.com/ei8/avatar-installer/tree/9a9aed8d23eb9fd5c63d919924af05945ad91b0a/src/test/Domain.Model/Configuration).

3. Save the file as `ei8_sample_config.json` in your `Downloads` folder
   - Make sure to select `All Files` in the `Save as type` dropdown
   - Ensure the filename ends with `.json` and not `.txt`

<img src="../images/save-sample-config.png" alt="Save sample config" style="display: block; margin: auto; max-width:100%;">

4. If a PFX certificate needs to be generated, run the following in a command prompt:
```
dotnet dev-certs https -ep [file] -p [password]
```
Where:
- "file" is the absolute path where the certificate will be generated, eg: "C:\Users\john\Documents\ei8\Avatar\sample"
- "password" is the certificate password. This should be copied to the JSON config node at "avatars\un8y\certificate_password".

5. If using encryption at rest:
    1. Set JSON config node at "avatars\event_sourcing\encryption_enabled" to true.
    2. Generate an RSA key pair (using a [tool](https://raskeyconverter.azurewebsites.net/)).
    3. Save the keys in two separate files (ie. public.key, private.key) in an avatar-specific folder.
       > [!NOTE]
       > Ideally in a secure location that can be physically detached from the server (eg. external USB drive):
       > "e:\ei8\keys\sample"
    4. Copy the path in the previous step to the JSON config nodes at (use the format in the sample above, ie. slashes, drive separator etc.):
        - "avatars\encryption\keys_path"
        - "avatars\event_sourcing\in_process_private_key_path"
    5. Update JSON config node at "avatars\event_sourcing\private_key_path" to use the correct relative path of private key file.
    6. Update JSON config node at "avatars\event_sourcing\encrypted_events_key" to use the AES key that was encrypted using the public key generated in a previous step.

<br>

#### 3️⃣ Download the Avatar Installer

1. Download and open the Avatar Installer app.
2. On the home page, click on `New` and choose a configuration file.
3. Inside the `New` page, click on `Choose File`. Choose the downloaded `ei8_sample_config.json` and click open.

<img src="../images/choose-sample-config.png" alt="Choose sample config" style="display: block; margin: auto; max-width:100%;">

4. After choosing a configuration file, click `Create` and wait for it to finish.
5. Once completed, you can now run your very own **Avatar Server**. Your saved files are in `Documents/ei8/Avatar`.
6. If you intend to use Un8y:
    1. Download the [applicable plugins version](https://drive.google.com/drive/folders/1OdmTfYoyUtVXbF9-22Q7SOHS8R8k4FNy?usp=sharing).
    2. Extract its contents to [avatar]\un8y\plugins\

> [!NOTE]
> If Avatar is being setup for local testing, ensure that [avatar]\un8y\variables.env > BASE_PATH is set to an empty string.

<br>

### ✅ Next Steps

Now that you have installed **ei8 Avatar**, you can start configuring and using it.  
Check out the [**User Manual**](configuration.md) for more advanced settings and customization options.

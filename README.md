# Azure Keyvault Explorer

A small Windows app to browse Azure Key Vaults and quickly look up and copy secrets.

- Lists your subscriptions and all their Key Vaults (via Azure Resource Graph, so it's fast even with many subscriptions)
- Filter vaults and secrets by name
- Copy secret values to the clipboard; the clipboard is cleared automatically after a configurable delay
- Shows a live TOTP code for a secret with a configurable name (default `TOTP`)
- The window is hidden from screenshots and screen sharing
- Remembers your sign-in and your last used subscription
- Updates itself automatically (ClickOnce)

## Install

Open this link in Edge (or another browser that supports ClickOnce) and run the downloaded file:

**https://rootphish.github.io/Azure-Keyvault-Explorer/AzureKeyvaultExplorer.application**

The installer adds the .NET 8 Desktop Runtime if needed. Each time you start the app, it checks for a new version and offers to install it.

The manifests are not code-signed, so Windows SmartScreen may warn you the first time you install.

## Setup

### 1. App registration in Microsoft Entra ID

The app signs you in through an app registration in your own tenant.

1. Create an **App registration**:
   - Name: e.g. `AzureKeyvaultExplorer`
   - Supported account types: *Accounts in this organizational directory only*
2. Under **Authentication**, add the platform **Mobile and desktop applications** with redirect URI `http://localhost`.
3. Under **API permissions**, add these delegated permissions (and grant admin consent if your tenant requires it):
   - Azure Service Management → `user_impersonation`
   - Azure Key Vault → `user_impersonation`
4. Note the **Application (client) ID** and **Directory (tenant) ID**.

### 2. Configure the app

Start the app and open **File → Preferences**:

| Setting | Description |
|---|---|
| Tenant ID | Directory (tenant) ID of the app registration |
| Client ID | Application (client) ID of the app registration |
| Clear Clipboard After (seconds) | How long a copied value stays on the clipboard |
| TOTP Secret Name | Secrets with this name are shown as a TOTP code |
| Connect On Startup | Connect automatically and reselect the last used subscription |

Settings are stored in `%APPDATA%\AzureKeyvaultExplorer\settings.json`. The sign-in token cache is stored encrypted (DPAPI) in `%LOCALAPPDATA%\azurekeyvaultexplorer.msalcache`.

### 3. Permissions in Azure

Your account needs:
- **Read access to the vault resource** (e.g. the *Reader* role) so the vault appears in the list
- **Access to the secrets**: the *Key Vault Secrets User* role (RBAC vaults), or an access policy with *Get* and *List* secret permissions

## Usage

Click **Connect** (or let the app connect on startup), pick a subscription, a vault and a secret.

| Key | Action |
|---|---|
| Ctrl+C | Copy the secret value (or the TOTP code when shown) |
| F5 | Refresh vaults and secrets |
| Down | From a filter box: jump into the list |
| Esc | In a filter box: clear the filter |

The secrets filter appears for vaults with 20 or more secrets. Disabled secrets are greyed out.

## Command-line tool

`AzureKeyvaultCLI` prints a single secret value, which is handy in scripts:

```
AzureKeyvaultCLI --tenantid <tenant-id> --clientid <client-id> --keyvault <vault-name> --secret <secret-name>
```

It uses the device code flow: on first use it prints a code and a URL to sign in with. It shares the token cache with the GUI.

The CLI is not part of the installer; build it from source (see below).

## Building

Requirements: Windows with the .NET 8 SDK.

```
dotnet build AzureKeyvaultExplorer.sln -c Release
```

The ClickOnce publish (`release.ps1`) uses MSBuild from Visual Studio 2022 (located with `vswhere`). Run `./release.ps1 -OnlyBuild` to publish locally without deploying.

## Releasing

Push a version tag:

```
git tag v0.0.13
git push origin v0.0.13
```

This starts two workflows:
- **Release** (`release.yml`) runs `release.ps1`, which builds the ClickOnce package and pushes it to the `gh-pages` branch. GitHub Pages serves it at the install URL above, and installed copies pick it up the next time they start. The version comes from the tag, so every new tag must be higher than the previous one.
- **Build application for Windows** (`build_action.yml`) builds a single-file `.exe` (requires the .NET 8 Desktop Runtime, no auto-update) and attaches it to a GitHub Release.

Every push to `master` and every pull request is built by the **CI** workflow (`ci.yml`).

# Azure-Keyvault-Explorer

A simple Windows Forms application to explore and manage Azure Key Vault secrets.

Currently still in development.

## Requirements

### App registration in Azure
- Create an App registration in Azure AD.
    - Name: AzureKeyvaultExplorer
    - Supported account types: Accounts in this organizational directory only
    - Redirect URI: http://localhost
- Assign the App registration the necessary API permissions to access the Key Vault 
    - Azure Service Management (user_impersonation)
	- Azure Keyvault (user_impersonation)

Note the Application (client) ID and Directory (tenant) ID of the App registration. You can enter these in the application settings later.

### Microsoft Visual Studio Installer Projects 2022
Install the [Microsoft Visual Studio Installer Projects 2022](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2022InstallerProjects) extension from the Visual Studio Marketplace.
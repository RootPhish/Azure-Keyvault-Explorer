# Azure-Keyvault-Explorer

A simple Windows Forms application to explore and manage Azure Key Vault secrets.

Currently still in development.

## Requirements

### App registration in Azure
- Create an app registration in Azure AD.
    - Name: AzureKeyvaultExplorer
    - Supported account types: Accounts in this organizational directory only
    - Redirect URI: http://localhost
- Assign the app registration the necessary API permissions to access the Key Vault 
    - Azure Service Management (user_impersonation)
	- Azure Keyvault (user_impersonation)

Note the Application (client) ID and Directory (tenant) ID of the app registration.

# DailyReport
Daily reporting and Work visualization service. (IN DEVELOPMENT)

## Getting Started

### Prerequisites

- [Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/)
- [SQL Server 2019 Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - Server authentication: SQL Server and Windows authentication mode

### Initialize Database

- Microsoft SQL Server Management Studio
  - Connect to localhost server  
    ![ssms](./docs/img/readme/ssms.png)
  - Execute `\schema\setup.sql`
- Visual Studio
  - Open `WaterTrans.DailyReport.sln`
  - Set as Startup Project: `WaterTrans.DailyReport.Web.Api`
  - Start Debugging
  - Execute `/api/v1/debug/database/initialize`  
    ![initialize](./docs/img/readme/initialize.png)
  - Execute `​/api​/v1​/debug​/database​/loadInitialData`  
    ![loadInitialData](./docs/img/readme/loadInitialData.png)

### Get Access Token

- Visual Studio
  - Start Debugging
  - Execute `/api/v1/token`  
    ![token](./docs/img/readme/token.png)
    - grant_type: client_credentials
    - client_id: owner
    - owner-secret: owner-secret
  - Copy `access_token`  
    ![accessToken](./docs/img/readme/accessToken.png)
  - Click `Authorize` and Paste `access_token`  
    ![authorize](./docs/img/readme/authorize.png)
  - You can execute all APIs

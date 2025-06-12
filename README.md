# 過程記錄

## 步驟 1 - 起手式

```bash
# 建立 .editorconfig 檔案來控制 coding styles
dotnet new editorconfig -o ./src

# 建立 global.json 檔案來控制 SDK 版本
dotnet new globaljson -o ./src --sdk-version 8.0.100 --roll-forward latestMajor

# 建立 .config/dotnet-tools.json 檔案來管理專案的本地工具
dotnet new tool-manifest -o ./src

# 建立專案專用的 NuGet 設定檔
dotnet new nugetconfig -o ./src

# 建立類別庫
dotnet new classlib --name MyLib --output ./src/MyLib

# 建立測試專案
dotnet new mstest --name MyLib.UnitTests --output ./src/MyLib.UnitTests

# 新增專案參考
dotnet add ./src/MyLib.UnitTests/MyLib.UnitTests.csproj reference ./src/MyLib/MyLib.csproj

# 建立 web 專案
dotnet new web --name MyWeb --output ./src/MyWeb

# 新增專案參考
dotnet add ./src/MyWeb/MyWeb.csproj reference ./src/MyLib/MyLib.csproj

# 建立方案
dotnet new sln -n MySln -o ./src

# 將專案加入方案中
dotnet sln ./src/MySln.sln add ./src/MyWeb/MyWeb.csproj
dotnet sln ./src/MySln.sln add ./src/MyLib/MyLib.csproj
dotnet sln ./src/MySln.sln add ./src/MyLib.UnitTests/MyLib.UnitTests.csproj

# Git 版控
dotnet new gitignore
git init
git add .
git commit -m 'Initial commit'
```

## 步驟 2 - 實作專案

## 步驟 3 - 設定 CI

* build.yml

    ```yml
    name: Build
    
    on:
      push:
        branches: [ "**" ]
      pull_request:
        branches: [ "**" ]
    
    jobs:
      build:
        runs-on: ubuntu-latest
    
        steps:
          - name: Checkout code
            uses: actions/checkout@v3
    
          - name: Setup .NET SDK from global.json
            uses: actions/setup-dotnet@v4
            with:
              global-json-file: ./src/global.json
    
          - name: Restore dependencies
            run: dotnet restore ./src/MySln.sln
    
          - name: Build
            run: dotnet build ./src/MySln.sln --configuration Release --no-restore
    ```

* test.yml

    ```yml
    name: Test
    
    on:
      push:
        branches: [ "**" ]
      pull_request:
        branches: [ "**" ]
    
    jobs:
      build:
        runs-on: ubuntu-latest
    
        steps:
          - name: Checkout code
            uses: actions/checkout@v3
    
          - name: Setup .NET SDK from global.json
            uses: actions/setup-dotnet@v4
            with:
              global-json-file: ./src/global.json
    
          - name: Restore dependencies
            run: dotnet restore ./src/MySln.sln
    
          - name: Build
            run: dotnet build ./src/MySln.sln --configuration Release --no-restore
    
          - name: Test
            run: dotnet test ./src/MyLib.UnitTests/MyLib.UnitTests.csproj --no-build --configuration Release --verbosity normal
    ```

## 步驟 4 - 設定 CD

1. 建立 Azure Service Principal 讓 GitHub 有權限部署
    * 使用 Azure CLI 建立 Azure Service Principal

    ```bash
    az login --use-device-code
    az ad sp create-for-rbac --name "github-deploy" --role contributor --scopes /subscriptions/<subscription-id>/resourceGroups/<resource-group-name> --sdk-auth
    ```

    * 命令回傳的 JSON 內容用於後續使用

2. 設定 GitHub Secrets
    * 將先前的 JSON 內容存到 GitHub Repository 的 Secrets
      * Settings > Secrets and variables > Actions > New repository secret
      * Name 輸入 `AZURE_CREDENTIALS`
      * Secret 輸入 JSON 內容

3. 新增 CD 的 YAML 檔

    * deploy.yml

        ```yml
        name: Deploy MyWeb to Azure App Service
        
        on:
          push:
            branches:
              - main
        
        jobs:
          build-and-deploy:
            runs-on: ubuntu-latest
        
            steps:
              - name: Checkout code
                uses: actions/checkout@v3
        
              - name: Setup .NET SDK from global.json
                uses: actions/setup-dotnet@v4
                with:
                  global-json-file: ./src/global.json
        
              - name: Restore dependencies
                run: dotnet restore ./src/MySln.sln
        
              - name: Build
                run: dotnet build ./src/MySln.sln --configuration Release --no-restore
        
              - name: Test
                run: dotnet test ./src/MyLib.UnitTests/MyLib.UnitTests.csproj --configuration Release --no-build --verbosity normal
        
              - name: Publish MyWeb project
                run: dotnet publish ./src/MyWeb/MyWeb.csproj --configuration Release --output ./publish
        
              - name: Azure Login
                uses: azure/login@v2
                with:
                  creds: ${{ secrets.AZURE_CREDENTIALS }}
        
              - name: Deploy to Azure App Service
                uses: azure/webapps-deploy@v2
                with:
                  app-name: 'myweb-punk-appservice' # 改成你建立 App Service 時的 App Name
                  package: ./publish
        ```

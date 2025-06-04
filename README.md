# 歷程記錄

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

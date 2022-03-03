Param ([string]$name)

dotnet ef migrations add $name -p .\UserService.Data\UserService.Data\UserService.Data.csproj -s .\UserService.Main\UserService.Main\UserService.Main.csproj --verbose -c DatabaseContext
dotnet ef migrations add $name --output-dir CacheMigrations -p .\UserService.Data\UserService.Data\UserService.Data.csproj -s .\UserService.Main\UserService.Main\UserService.Main.csproj --verbose -c CacheContext



rm -rf obj
rm -rf bin

dotnet build RF5_Harem.csproj -f netstandard2.1 -c Release

zip -j 'RF5_Harem-v577_v1.2.0.zip' './bin/Release/netstandard2.1/RF5_Harem.dll' './bin/Release/netstandard2.1/RF5_Harem.cfg'

dotnet build RF5_Harem.csproj -f net6.0 -c Release

zip -j 'RF5_Harem.1.2.0.zip' './bin/Release/net6.0/RF5_Harem.dll' './bin/Release/net6.0/RF5_Harem.cfg'

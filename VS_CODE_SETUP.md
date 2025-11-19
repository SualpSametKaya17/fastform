# Visual Studio Code Kurulum ve Sorun Giderme

## 🚀 Hızlı Başlangıç

### 1. Projeyi Clone Edin
```bash
git clone https://github.com/SualpSametKaya17/fastform.git
cd fastform
```

### 2. VS Code'da Açın
```bash
code .
```

### 3. Cache Sorunlarını Çözün

**Windows:**
```bash
clean-build.bat
```

**Linux/Mac:**
```bash
./clean-build.sh
```

### 4. VS Code'da Build
- **Ctrl + Shift + B** → "build" seçin
- Veya **F5** ile debug başlatın

---

## ⚙️ Gerekli Extension'lar

VS Code'da Extensions panelinden yükleyin:

### Zorunlu
- ✅ **C# Dev Kit** (Microsoft)
- ✅ **C#** (Microsoft)
- ✅ **.NET Extension Pack**

### Önerilen
- 📝 **XAML** (XAML Complete)
- 📦 **NuGet Package Manager**
- 🗂️ **GitLens**
- 🗄️ **SQLTools**

---

## 🔧 Build Sorunları ve Çözümleri

### ❌ Sorun 1: "net6.0-windows is out of support"

**Neden:** Cache'de eski proje dosyası var

**Çözüm:**
```bash
# Terminal'de
cd FastForm
rm -rf bin/ obj/
dotnet clean
dotnet build --no-incremental
```

### ❌ Sorun 2: "SixLabors.ImageSharp 3.1.0 has vulnerabilities"

**Neden:** Eski paket cache'de

**Çözüm:**
```bash
# NuGet cache temizle
dotnet nuget locals all --clear
dotnet restore --force --no-cache
dotnet build
```

### ❌ Sorun 3: "Key cannot be null - Line 277"

**Neden:** XAML parser hatası veya eski build cache

**Çözüm:**
```bash
# Tam temizlik
cd FastForm
rm -rf bin/ obj/
dotnet clean
dotnet restore --force
dotnet build --no-incremental --force
```

### ❌ Sorun 4: "Type or namespace could not be found"

**Neden:** Paketler restore edilmemiş

**Çözüm:**
```bash
cd FastForm
dotnet restore
```

---

## 📁 VS Code Dosya Yapısı

Proje root'unda `.vscode` klasörü otomatik oluşturuldu:

```
fastform/
├── .vscode/
│   ├── launch.json      # Debug konfigürasyonu
│   └── tasks.json       # Build görevleri
├── clean-build.sh       # Linux/Mac temizlik
├── clean-build.bat      # Windows temizlik
└── FastForm/
```

---

## 🐛 Debug Nasıl Yapılır?

### Yöntem 1: F5 ile Debug
1. VS Code'da **F5** tuşuna basın
2. "FastForm (Debug)" seçin
3. Uygulama debug modda başlar

### Yöntem 2: Terminal
```bash
cd FastForm
dotnet run
```

### Breakpoint Koyma
1. Kod satırının soluna tıklayın (kırmızı nokta)
2. **F5** ile başlatın
3. Kod o satıra gelince durur

---

## 🗄️ SQL Server Kurulumu

### LocalDB (Önerilen - Geliştirme için)

**Windows:**
```bash
# SQL Server Express LocalDB indir
# https://aka.ms/ssmsfullsetup
```

**Kontrol:**
```bash
sqllocaldb info
```

### Docker ile SQL Server

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=FastForm2024!" \
  -p 1433:1433 \
  --name fastform-sql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

---

## 🔌 Connection String

`FastForm/appsettings.json` dosyasını düzenleyin:

### LocalDB için:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FastFormDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Docker SQL Server için:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=FastFormDb;User Id=sa;Password=FastForm2024!;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Remote SQL Server için:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=FastFormDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

---

## 💾 Veritabanı Oluşturma

### Yöntem 1: SSMS ile
1. SQL Server Management Studio açın
2. `Database/schema_extended.sql` dosyasını açın
3. Execute edin (F5)

### Yöntem 2: Terminal ile
```bash
# LocalDB
sqlcmd -S "(localdb)\mssqllocaldb" -i Database/schema_extended.sql

# Docker
sqlcmd -S localhost -U sa -P FastForm2024! -i Database/schema_extended.sql
```

### Yöntem 3: Azure Data Studio
1. Azure Data Studio açın
2. Bağlantı ekleyin
3. SQL dosyasını çalıştırın

---

## 🎯 VS Code Kısayolları

| Kısayol | Açıklama |
|---------|----------|
| **F5** | Debug başlat |
| **Ctrl + Shift + B** | Build |
| **Ctrl + .** | Quick fix |
| **F12** | Go to definition |
| **Shift + F12** | Find references |
| **Ctrl + /** | Yorum satırı |
| **Ctrl + K + D** | Format document |
| **Ctrl + Space** | IntelliSense |

---

## 📊 Build Komutları

```bash
# Temiz build
dotnet clean && dotnet build

# Paket restore
dotnet restore

# Cache temizle
dotnet nuget locals all --clear

# Force rebuild
dotnet build --no-incremental --force

# Release build
dotnet build -c Release

# Publish
dotnet publish -c Release -o ./publish
```

---

## 🔍 Log Dosyaları

Uygulama çalıştığında log dosyaları:
```
FastForm/logs/
├── fastform-20250119.txt
├── fastform-20250120.txt
└── ...
```

Hataları buradan kontrol edebilirsiniz.

---

## ✅ Kontrol Listesi

Build öncesi kontrol:

- [ ] .NET 8.0 SDK yüklü (`dotnet --version`)
- [ ] SQL Server çalışıyor
- [ ] Connection string doğru
- [ ] NuGet paketleri restore edilmiş (`dotnet restore`)
- [ ] bin/obj klasörleri temizlenmiş
- [ ] VS Code extension'ları yüklü

---

## 🆘 Yardım

### Hala sorun mu var?

1. **Log dosyalarını kontrol edin:**
   ```bash
   cat FastForm/logs/fastform-*.txt | tail -100
   ```

2. **Detaylı build log:**
   ```bash
   dotnet build --verbosity detailed
   ```

3. **Tüm cache'i temizle:**
   ```bash
   rm -rf ~/.nuget/packages/
   rm -rf FastForm/bin/ FastForm/obj/
   dotnet restore
   ```

4. **VS Code'u yeniden başlat**
   - Ctrl + Shift + P → "Reload Window"

---

## 📚 Faydalı Linkler

- [.NET 8.0 Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [VS Code C# Docs](https://code.visualstudio.com/docs/languages/csharp)
- [EF Core Docs](https://learn.microsoft.com/ef/core/)
- [Material Design XAML](http://materialdesigninxaml.net/)

---

**Başarılar! 🚀**

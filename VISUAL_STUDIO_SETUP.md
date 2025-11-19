# Visual Studio 2022 Kurulum Rehberi

## 🚀 Hızlı Başlangıç

### 1. Projeyi Aç
```
FastForm.sln dosyasına çift tıklayın
veya
Visual Studio 2022 → File → Open → Project/Solution → FastForm.sln
```

### 2. İlk Build Öncesi Temizlik (Önemli!)

Cache sorunlarını önlemek için:

**Yöntem 1: Otomatik (Önerilen)**
- Solution Explorer'da `FastForm.sln` sağ tık
- "Clean Solution" tıklayın
- Sonra "Rebuild Solution" tıklayın

**Yöntem 2: Manuel**
```
1. Visual Studio'yu kapatın
2. FastForm/.vs/ klasörünü silin
3. FastForm/FastForm/bin/ klasörünü silin
4. FastForm/FastForm/obj/ klasörünü silin
5. Visual Studio'yu açın
```

**Yöntem 3: PowerShell Script (En İyi)**
```powershell
# Visual Studio'yu kapatın, sonra PowerShell'de:
cd C:\...\fastform
.\clean-build.bat
```

### 3. NuGet Paketlerini Restore Et

**Otomatik:** Solution açıldığında otomatik restore olur.

**Manuel gerekirse:**
- Solution Explorer'da solution sağ tık
- "Restore NuGet Packages"
- Veya: Tools → NuGet Package Manager → Package Manager Console
  ```
  Update-Package -reinstall
  ```

### 4. Build
- **Build → Rebuild Solution** (Ctrl + Shift + B)
- İlk build biraz uzun sürebilir (NuGet paketleri indiriliyor)

### 5. Çalıştır
- **F5** veya **Debug → Start Debugging**
- Veya **Ctrl + F5** (debug olmadan)

---

## ⚙️ Visual Studio Gereksinimleri

### Minimum Sürüm
- **Visual Studio 2022** v17.8 veya üzeri
- Community, Professional veya Enterprise

### Gerekli Workload'lar
Visual Studio Installer'da yüklü olmalı:

✅ **.NET Desktop Development**
- .NET 8.0 SDK
- WPF Designer
- Entity Framework tools

✅ **Data Storage and Processing** (SQL için)
- SQL Server Express LocalDB
- SQL Server Data Tools

### Kontrol
```
Help → About Microsoft Visual Studio
→ .NET SDK: 8.0.x görünmeli
```

---

## 🗄️ SQL Server Kurulumu

### LocalDB (Önerilen)

Visual Studio 2022 ile birlikte gelir:

**Kontrol:**
```cmd
sqllocaldb info
```

Çıktı: `MSSQLLocalDB` görünmeli

**Yoksa:**
- Visual Studio Installer → Modify
- "Data storage and processing" → SQL Server Express LocalDB işaretle

### Veritabanını Oluştur

**Yöntem 1: Visual Studio SQL Server Object Explorer**
1. View → SQL Server Object Explorer (Ctrl + \, Ctrl + S)
2. (localdb)\MSSQLLocalDB → sağ tık → New Query
3. `Database/schema_extended.sql` dosyasının içeriğini yapıştırın
4. Execute (Ctrl + Shift + E)

**Yöntem 2: Package Manager Console**
```powershell
# Tools → NuGet Package Manager → Package Manager Console
Invoke-Sqlcmd -ServerInstance "(localdb)\MSSQLLocalDB" -InputFile "Database\schema_extended.sql"
```

**Yöntem 3: CMD**
```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "Database\schema_extended.sql"
```

---

## 🔌 Connection String Ayarları

`FastForm/appsettings.json` dosyasını düzenleyin:

### LocalDB için (Varsayılan):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FastFormDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### SQL Server Express için:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=FastFormDb;Trusted_Connection=True;MultipleActiveResultSets=true"
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

**Önemli:** appsettings.json dosyası .gitignore'da değil, bu yüzden hassas bilgi koymayın!

---

## 🐛 Build Sorunları ve Çözümleri

### ❌ "The target framework 'net6.0-windows' is out of support"

**Neden:** Cache'de eski proje dosyası

**Çözüm:**
1. Visual Studio'yu kapatın
2. `.vs`, `bin`, `obj` klasörlerini silin
3. VS'yi açın ve Rebuild Solution

### ❌ "SixLabors.ImageSharp 3.1.0 has vulnerabilities"

**Neden:** Eski paket cache'de

**Çözüm - Tools → NuGet Package Manager → Package Manager Console:**
```powershell
# Cache temizle
dotnet nuget locals all --clear

# Force restore
Update-Package -reinstall -ProjectName FastForm
```

### ❌ "Key cannot be null - Line 277"

**Neden:** XAML designer hatası

**Çözüm:**
1. Build → Clean Solution
2. Build → Rebuild Solution
3. Hala varsa: XAML designer'ı kapat/aç veya VS'yi restart

### ❌ "Could not load file or assembly"

**Çözüm:**
```powershell
# Package Manager Console
Update-Package -reinstall
```

### ❌ "Database connection failed"

**Kontrol listesi:**
1. SQL Server çalışıyor mu?
   ```cmd
   sqllocaldb info
   sqllocaldb start MSSQLLocalDB
   ```
2. Connection string doğru mu?
3. Database oluşturuldu mu?
4. Firewall engel mi?

---

## 🔧 NuGet Package Manager

### Package Manager Console
**Tools → NuGet Package Manager → Package Manager Console**

Faydalı komutlar:
```powershell
# Tüm paketleri güncelle
Update-Package

# Belirli paketi yeniden yükle
Update-Package SixLabors.ImageSharp -reinstall

# Cache temizle
dotnet nuget locals all --clear

# Paket listesi
Get-Package

# Paket ara
Find-Package MaterialDesign
```

### Package Manager UI
**Tools → NuGet Package Manager → Manage NuGet Packages for Solution**

- Updates sekmesinde güncellemeler
- Installed sekmesinde yüklüler
- Browse'da yeni paket ara

---

## 🎨 XAML Designer Sorunları

### Designer yüklenmiyor

**Çözüm 1:**
- XAML dosyasını kapat
- Build → Rebuild Solution
- XAML'i tekrar aç

**Çözüm 2:**
- Tools → Options → XAML Designer
- "Enable XAML Designer" işaretli olmalı
- "Disable project code" işaretini kaldırın

**Çözüm 3:**
- Visual Studio Installer → Modify
- ".NET desktop development" → Repair

### MaterialDesign temaları görünmüyor

Normal! Sadece runtime'da görünür. Build edin ve çalıştırın.

---

## 🚀 İlk Çalıştırma

### Adım 1: Build
```
Build → Rebuild Solution (Ctrl + Shift + B)
```

Çıktıda:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```
görünmeli.

### Adım 2: Çalıştır
```
Debug → Start Debugging (F5)
```

### Adım 3: Giriş Yap
İlk çalıştırmada varsayılan kullanıcı:
- **Kullanıcı:** admin
- **Şifre:** Admin123!

---

## 🎯 Debug Ayarları

### Breakpoint Koyma
1. Kod satırının soluna tıklayın (kırmızı nokta)
2. **F5** ile debug başlatın
3. Kod o satıra gelince durur

### Debug Komutları
- **F5** - Continue
- **F10** - Step Over
- **F11** - Step Into
- **Shift + F11** - Step Out
- **Shift + F5** - Stop Debugging

### Watch Window
- Debug → Windows → Watch → Watch 1
- Değişkenleri izleyin

### Locals Window
- Debug → Windows → Locals
- Tüm local değişkenler

### Output Window
- View → Output (Ctrl + Alt + O)
- Serilog logları burada görünür

---

## ⚡ Visual Studio Kısayolları

### Build
- **Ctrl + Shift + B** - Build Solution
- **Ctrl + B** - Build Project

### Debug
- **F5** - Start Debugging
- **Ctrl + F5** - Start Without Debugging
- **F9** - Toggle Breakpoint
- **Ctrl + Shift + F9** - Delete All Breakpoints

### Kod Düzenleme
- **Ctrl + K, D** - Format Document
- **Ctrl + K, C** - Comment
- **Ctrl + K, U** - Uncomment
- **Ctrl + .** - Quick Actions
- **F12** - Go to Definition
- **Ctrl + -** - Navigate Backward

### Solution Explorer
- **Ctrl + Alt + L** - Açar
- **Ctrl + ;** - Search

### Diğer
- **Ctrl + ,** - Go to All (dosya ara)
- **Ctrl + T** - Go to Type
- **Ctrl + Shift + F** - Find in Files

---

## 📊 Proje Yapısı (Solution Explorer)

```
FastForm (Solution)
└── FastForm (Project)
    ├── Dependencies
    │   ├── Frameworks (.NET 8.0)
    │   └── Packages (NuGet)
    ├── Properties
    ├── Data
    │   └── FastFormDbContext.cs
    ├── Models
    │   ├── User.cs
    │   ├── FormTemplate.cs
    │   └── ...
    ├── Services
    │   ├── FormTemplateService.cs
    │   └── ...
    ├── ViewModels
    │   ├── MainViewModel.cs
    │   └── ...
    ├── Views
    │   └── MainWindow.xaml
    ├── App.xaml
    └── appsettings.json
```

---

## 🔍 Log Dosyaları

Uygulama çalışırken log dosyaları:
```
FastForm/bin/Debug/net8.0-windows/logs/
└── fastform-20250119.txt
```

Hata durumunda buraya bakın:
```
View → Output → Show output from: Debug
```

---

## 📦 Publish (Yayınlama)

Release build oluşturmak için:

### Yöntem 1: Visual Studio
1. Solution Explorer → FastForm (project) sağ tık
2. Publish
3. Folder seçin
4. Target Location: `bin\Release\Publish`
5. Configuration: Release | Any CPU
6. Target Framework: net8.0-windows
7. Publish

### Yöntem 2: CLI
```cmd
dotnet publish -c Release -o ./publish
```

Çıktı: `FastForm/publish/` klasöründe

---

## 🧪 Test (Gelecekte Eklenecek)

Test projesi eklemek için:
1. Solution sağ tık → Add → New Project
2. "xUnit Test Project" seçin
3. Name: FastForm.Tests
4. Add

---

## ⚙️ Önerilen Extension'lar

Visual Studio Marketplace'ten:

- **Productivity Power Tools** - Geliştirici araçları
- **XAML Styler** - XAML formatlama
- **ResXManager** - Resource dosya yönetimi
- **CodeMaid** - Kod temizleme
- **GitFlow** - Git workflow

---

## 🆘 Sorun Giderme Kontrol Listesi

Build hatası alıyorsanız:

1. ✅ Visual Studio 2022 v17.8+
2. ✅ .NET 8.0 SDK yüklü
3. ✅ `.vs`, `bin`, `obj` klasörleri silindi
4. ✅ NuGet paketleri restore edildi
5. ✅ Rebuild Solution yapıldı
6. ✅ SQL Server çalışıyor
7. ✅ Connection string doğru
8. ✅ Database oluşturuldu

Hala sorun varsa:
- Output penceresini kontrol edin
- Error List'e bakın (View → Error List)
- Log dosyalarını inceleyin

---

## 📞 Yardım

### Visual Studio Komut Satırı
```cmd
# Visual Studio'yu komut satırından aç
devenv FastForm.sln

# Clean
devenv FastForm.sln /clean

# Rebuild
devenv FastForm.sln /rebuild
```

### Developer PowerShell
Visual Studio Developer PowerShell kullanın:
```powershell
# Start menüden: Developer PowerShell for VS 2022
cd C:\...\fastform
dotnet build
```

---

**Başarılar! 🚀**

Sorun yaşarsanız `logs` klasörüne veya Visual Studio Output penceresine bakın.

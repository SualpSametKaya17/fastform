# FastForm - Hızlı Başlangıç Rehberi

## 🎯 3 Dakikada Başla

### IDE'nizi Seçin

**Visual Studio 2022 kullanıyorsanız:**
→ [VISUAL_STUDIO_SETUP.md](VISUAL_STUDIO_SETUP.md)

**VS Code kullanıyorsanız:**
→ [VS_CODE_SETUP.md](VS_CODE_SETUP.md)

---

## ⚡ Hızlı Kurulum

### 1. Kodu Çek
```bash
git clone https://github.com/SualpSametKaya17/fastform.git
cd fastform
```

### 2. Cache Temizliği

**Windows (Visual Studio):**
```cmd
clean-vs.bat
```

**Windows (VS Code):**
```cmd
clean-build.bat
```

**Linux/Mac:**
```bash
./clean-build.sh
```

### 3. SQL Server Hazırla

**LocalDB (Önerilen):**
```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -i Database\schema_extended.sql
```

**Docker:**
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=FastForm2024!" \
  -p 1433:1433 --name fastform-sql \
  -d mcr.microsoft.com/mssql/server:2022-latest

# Veritabanı oluştur
sqlcmd -S localhost -U sa -P FastForm2024! -i Database/schema_extended.sql
```

### 4. Çalıştır

**Visual Studio:**
- FastForm.sln dosyasına çift tıkla
- F5'e bas

**VS Code:**
- `code .`
- F5'e bas

**Terminal:**
```bash
cd FastForm
dotnet run
```

### 5. Giriş Yap

**İlk Giriş:**
- Kullanıcı: `admin`
- Şifre: `Admin123!`

---

## 🔧 Gereksinimler

### Zorunlu
- ✅ **.NET 8.0 SDK** - [İndir](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ **SQL Server** - LocalDB, Express veya Full

### IDE (Birini seçin)
- **Visual Studio 2022** (v17.8+) - [İndir](https://visualstudio.microsoft.com/)
- **VS Code** + C# Dev Kit - [İndir](https://code.visualstudio.com/)

---

## ❓ Hata Alıyorum

### "net6.0-windows out of support"
```bash
# Cache temizle
clean-vs.bat        # veya clean-build.bat
```

### "ImageSharp 3.1.0 vulnerability"
```bash
dotnet nuget locals all --clear
dotnet restore --force
```

### "Database connection failed"
```cmd
# SQL Server'ı başlat
sqllocaldb start MSSQLLocalDB

# Database'i oluştur
sqlcmd -S "(localdb)\MSSQLLocalDB" -i Database\schema_extended.sql
```

### "Key cannot be null"
```bash
# Tam temizlik
cd FastForm
rm -rf bin/ obj/
dotnet clean
dotnet build --no-incremental
```

---

## 📚 Detaylı Dokümantasyon

- **Visual Studio Kurulum**: [VISUAL_STUDIO_SETUP.md](VISUAL_STUDIO_SETUP.md)
- **VS Code Kurulum**: [VS_CODE_SETUP.md](VS_CODE_SETUP.md)
- **Kullanım Kılavuzu**: [KULLANIM_KILAVUZU.md](KULLANIM_KILAVUZU.md)
- **Katkıda Bulunma**: [CONTRIBUTING.md](CONTRIBUTING.md)
- **Genel README**: [README.md](README.md)

---

## 🎓 İlk Adımlar

### 1. Form Şablonu Oluştur
1. "Yeni Şablon" butonuna tıkla
2. Form resmini yükle (PNG/JPG)
3. "Alan Ekle" ile alanları tanımla
4. Kaydet

### 2. Form Doldur
1. "Yeni Form" butonuna tıkla
2. Şablon seç
3. Alanları doldur
4. Önizle ve kaydet

### 3. Export Et
1. Form seç
2. "Dışa Aktar" → Format seç (PDF/PNG)
3. Kaydet

---

## 🏗️ Proje Yapısı

```
fastform/
├── FastForm.sln              # Visual Studio solution
├── clean-vs.bat              # VS cache temizleyici
├── clean-build.bat           # Genel temizleyici
├── Database/
│   └── schema_extended.sql   # Veritabanı
├── FastForm/                 # Ana proje
│   ├── Models/              # Veri modelleri
│   ├── Services/            # İş mantığı
│   ├── ViewModels/          # MVVM
│   └── Views/               # UI
├── QUICK_START.md           # Bu dosya
├── VISUAL_STUDIO_SETUP.md   # VS rehberi
└── VS_CODE_SETUP.md         # VS Code rehberi
```

---

## 🔑 Önemli Dosyalar

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FastFormDb;..."
  }
}
```

Connection string'i ortamınıza göre düzenleyin.

---

## 💡 İpuçları

1. **Build Sorunları**: Cache temizleyici script çalıştırın
2. **SQL Bağlantısı**: LocalDB en kolay seçenek
3. **İlk Build**: Yavaş olabilir (paketler indiriliyor)
4. **XAML Hatası**: Sadece runtime'da çalışır, build edin
5. **Loglar**: `FastForm/bin/Debug/net8.0-windows/logs/`

---

## 🚀 Performans

İlk çalıştırmada:
- Build: ~30-60 saniye (paket indirme)
- Startup: ~3-5 saniye

Sonraki çalıştırmalarda:
- Build: ~5-10 saniye
- Startup: ~1-2 saniye

---

## 📞 Destek

- **GitHub Issues**: [Issues](https://github.com/SualpSametKaya17/fastform/issues)
- **Dokümantasyon**: Bu repo'daki .md dosyaları
- **Loglar**: `logs/fastform-*.txt` dosyaları

---

**Kolay gelsin! 🎉**

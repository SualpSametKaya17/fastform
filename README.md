# FastForm - Form Doldurma Sistemi

Modern, kullanıcı dostu masaüstü form doldurma ve yönetim sistemi.

## Özellikler

### 📋 Form Şablon Yönetimi
- Form resimlerini yükle ve şablon oluştur
- Görsel arayüzde alanları tanımla (pozisyon, boyut, stil)
- Şablon konfigürasyonlarını JSON formatında kaydet/yükle
- Version kontrolü ve değişiklik geçmişi
- Şablon kategorileri ve etiketleme

### ✍️ Form Doldurma
- Tanımlı alanlara kolayca veri gir
- Canlı önizleme ve düzenleme
- Otomatik kaydetme özelliği
- Alan validasyonu ve doğrulama kuralları
- Farklı alan tipleri:
  - Metin
  - Sayı
  - Tarih
  - Dropdown (Açılır liste)
  - Checkbox (Onay kutusu)
  - İmza
  - Barkod

### 📊 Dashboard ve Raporlama
- İstatistikler ve özet bilgiler
- Son aktiviteler
- Bekleyen formlar
- Hızlı erişim menüleri

### 🔐 Kullanıcı Yönetimi
- Rol tabanlı yetkilendirme (Admin, Manager, User, Viewer)
- Kullanıcı bazlı form izinleri
- Audit log (işlem geçmişi)
- Oturum yönetimi

### 📤 Export Özellikleri
- Birden fazla format desteği:
  - PDF
  - PNG
  - JPEG
  - DOCX (ileride)
- Toplu export işlemleri
- Özelleştirilebilir kalite ayarları

### 🗄️ Veritabanı
- Microsoft SQL Server (MSSQL)
- Entity Framework Core ORM
- Otomatik migration desteği
- Configuration history ve versiyonlama

### 🎨 Modern Arayüz
- Material Design temalar
- Responsive tasarım
- Koyu/Açık tema desteği
- Türkçe ve İngilizce dil desteği

## Teknolojiler

- **Framework:** .NET 8.0 WPF
- **UI:** MaterialDesignThemes 5.1
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core 8.0
- **MVVM:** CommunityToolkit.Mvvm
- **Image Processing:** SixLabors.ImageSharp 3.1.5
- **Logging:** Serilog 4.1
- **Serialization:** Newtonsoft.Json

## Kurulum

### Gereksinimler

1. **.NET 8.0 SDK veya üzeri**
   - [Download .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

2. **Microsoft SQL Server**
   - SQL Server 2019 veya üzeri
   - SQL Server LocalDB (geliştirme için - önerilen)
   - SQL Server Express (ücretsiz)

3. **IDE** (birini seçin)
   - **Visual Studio 2022** (v17.8+)
     - Community Edition veya üzeri
     - Workload: ".NET desktop development"
   - **Visual Studio Code**
     - C# Dev Kit extension
     - .NET Extension Pack
     - Detaylar: [VS_CODE_SETUP.md](VS_CODE_SETUP.md)

### Adımlar

1. **Projeyi klonlayın**
   ```bash
   git clone https://github.com/SualpSametKaya17/fastform.git
   cd fastform
   ```

   **VS Code kullanıyorsanız:** [VS_CODE_SETUP.md](VS_CODE_SETUP.md) dosyasını okuyun.

2. **Veritabanını oluşturun**

   SQL Server Management Studio (SSMS) veya sqlcmd ile:
   ```sql
   -- Database/schema_extended.sql dosyasını çalıştırın
   ```

3. **Connection string'i yapılandırın**

   `FastForm/appsettings.json` dosyasını düzenleyin:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=FastFormDb;Trusted_Connection=True;"
     }
   }
   ```

4. **Projeyi derleyin ve çalıştırın**
   ```bash
   cd FastForm
   dotnet restore
   dotnet build
   dotnet run
   ```

   veya Visual Studio'da:
   - Solution'ı açın (FastForm.sln)
   - F5 ile çalıştırın

## Kullanım

### 1. İlk Giriş

Varsayılan admin kullanıcısı:
- **Kullanıcı adı:** admin
- **Şifre:** Admin123!

> **Not:** İlk girişten sonra şifreyi değiştirin!

### 2. Form Şablonu Oluşturma

1. **Yeni Şablon** butonuna tıklayın
2. Form resmini yükleyin (PNG, JPG)
3. **Alan Ekle** ile yeni alan tanımlayın:
   - Alana tıklayıp sürükleyerek konumlandırın
   - Özellikleri düzenleyin (font, boyut, renk, vb.)
   - Validasyon kuralları ekleyin
4. **Kaydet** ile şablonu kaydedin

### 3. Form Doldurma

1. **Yeni Form** butonuna tıklayın
2. Şablonu seçin
3. Alanları doldurun
4. **Önizleme** ile kontrol edin
5. **Kaydet** veya **Tamamla**
6. İstenirse **Export** edin

### 4. Konfigürasyon İmport/Export

**Export:**
```
Form Şablonları → Şablon Seç → Dışa Aktar → config.json kaydet
```

**Import:**
```
Form Şablonları → Şablon Seç → İçe Aktar → config.json seç
```

## Veritabanı Şeması

### Ana Tablolar

- **Users** - Kullanıcılar
- **FormTemplates** - Form şablonları
- **FieldDefinitions** - Alan tanımlamaları
- **FormInstances** - Doldurulmuş formlar
- **FieldValues** - Alan değerleri
- **ConfigurationHistory** - Konfigürasyon geçmişi
- **AuditLogs** - İşlem kayıtları
- **Settings** - Uygulama ayarları
- **BatchJobs** - Toplu işlemler
- **Translations** - Çeviri tablosu

## Proje Yapısı

```
FastForm/
├── FastForm.sln                    # Solution dosyası
├── Database/
│   ├── schema.sql                  # Temel veritabanı şeması
│   └── schema_extended.sql         # Genişletilmiş şema
├── FastForm/
│   ├── App.xaml                    # Uygulama başlangıç
│   ├── appsettings.json            # Yapılandırma
│   ├── Data/
│   │   └── FastFormDbContext.cs    # EF Core DbContext
│   ├── Models/                     # Veri modelleri
│   │   ├── User.cs
│   │   ├── FormTemplate.cs
│   │   ├── FieldDefinition.cs
│   │   ├── FormInstance.cs
│   │   └── ...
│   ├── Services/                   # Business logic
│   │   ├── IFormTemplateService.cs
│   │   ├── FormTemplateService.cs
│   │   ├── IFormDataService.cs
│   │   ├── FormDataService.cs
│   │   ├── IExportService.cs
│   │   ├── ExportService.cs
│   │   └── ...
│   ├── ViewModels/                 # MVVM ViewModels
│   │   ├── MainViewModel.cs
│   │   ├── FormEditorViewModel.cs
│   │   └── FormFillViewModel.cs
│   └── Views/                      # XAML görünümler
│       └── MainWindow.xaml
└── README.md
```

## Yapılandırma

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FastFormDb;..."
  },
  "AppSettings": {
    "Language": "tr",              // tr, en
    "Theme": "Light",              // Light, Dark
    "AutoSave": true,
    "AutoSaveIntervalSeconds": 60
  },
  "Export": {
    "DefaultFormat": "PDF",        // PDF, PNG, JPEG
    "Quality": 95,
    "DefaultOutputFolder": "output"
  }
}
```

## Gelişmiş Özellikler

### Alan Validasyonu

```json
{
  "FieldName": "Email",
  "ValidationRegex": "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$",
  "ValidationMessage": "Geçerli bir e-posta adresi giriniz",
  "IsRequired": true
}
```

### Toplu İşlemler

1. CSV/Excel dosyası hazırlayın
2. **Toplu İşlem** → **Yeni İş** oluşturun
3. Veri kaynağını seçin
4. Mapping yapın
5. İşlemi başlatın

## Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit edin (`git commit -m 'Add amazing feature'`)
4. Push edin (`git push origin feature/amazing-feature`)
5. Pull Request açın

## Lisans

Bu proje [MIT License](LICENSE) altında lisanslanmıştır.

## İletişim

Proje Sahibi - [@yourusername](https://github.com/yourusername)

Proje Linki: [https://github.com/yourusername/fastform](https://github.com/yourusername/fastform)

## Teşekkürler

- [MaterialDesignInXAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet)
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)
- [Serilog](https://github.com/serilog/serilog)

## Gelecek Özellikler

- [ ] OCR desteği (otomatik alan algılama)
- [ ] Bulut senkronizasyonu
- [ ] Mobil uygulama
- [ ] Gelişmiş raporlama
- [ ] Email entegrasyonu
- [ ] REST API
- [ ] Webhook desteği
- [ ] QR/Barkod okuma
- [ ] Dijital imza doğrulama
- [ ] Template marketplace

## Sık Sorulan Sorular (SSS)

### Veritabanı bağlantı hatası alıyorum

1. SQL Server'ın çalıştığından emin olun
2. Connection string'i kontrol edin
3. Kullanıcı yetkilerini kontrol edin

### Form resmi yüklenmiyor

1. Resim formatını kontrol edin (PNG, JPG desteklenir)
2. Dosya boyutunu kontrol edin (max 10MB önerilir)
3. Dosya yolunu kontrol edin

### Export işlemi başarısız oluyor

1. Output klasörünün yazılabilir olduğundan emin olun
2. Yeterli disk alanı olduğunu kontrol edin
3. Font'ların yüklü olduğunu kontrol edin

## Changelog

### v1.0.0 (2025-01-XX)
- İlk sürüm
- Temel form doldurma özellikleri
- MSSQL veritabanı entegrasyonu
- Material Design arayüz
- Export özellikleri
- Kullanıcı yönetimi

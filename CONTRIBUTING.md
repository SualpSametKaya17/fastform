# Katkıda Bulunma Rehberi

FastForm projesine katkıda bulunmak istediğiniz için teşekkür ederiz! Bu belge, projeye nasıl katkıda bulunabileceğinizi açıklar.

## İçindekiler

1. [Başlarken](#başlarken)
2. [Geliştirme Ortamı](#geliştirme-ortamı)
3. [Kod Standartları](#kod-standartları)
4. [Commit Mesajları](#commit-mesajları)
5. [Pull Request Süreci](#pull-request-süreci)
6. [Test](#test)
7. [Dokümantasyon](#dokümantasyon)

## Başlarken

### Önkoşullar

- .NET 6.0 SDK veya üzeri
- Visual Studio 2022 veya VS Code
- Git
- SQL Server veya SQL Server LocalDB

### Fork ve Clone

1. Projeyi fork edin
2. Fork'u clone edin:
   ```bash
   git clone https://github.com/YOUR_USERNAME/fastform.git
   cd fastform
   ```
3. Upstream remote ekleyin:
   ```bash
   git remote add upstream https://github.com/ORIGINAL_OWNER/fastform.git
   ```

## Geliştirme Ortamı

### Kurulum

1. Bağımlılıkları yükleyin:
   ```bash
   dotnet restore
   ```

2. Veritabanını oluşturun:
   ```bash
   # SQL Server Management Studio ile schema_extended.sql çalıştırın
   ```

3. Uygulamayı çalıştırın:
   ```bash
   dotnet run --project FastForm
   ```

### Proje Yapısı

```
FastForm/
├── Data/           # DbContext ve migrations
├── Models/         # Entity modelleri
├── Services/       # Business logic
├── ViewModels/     # MVVM ViewModels
├── Views/          # XAML görünümler
└── Helpers/        # Yardımcı sınıflar
```

## Kod Standartları

### C# Coding Guidelines

Microsoft C# Coding Conventions'ı takip edin:

- PascalCase: Class, method, property isimleri
- camelCase: Local variable, parameter isimleri
- _camelCase: Private field isimleri

**Örnek:**

```csharp
public class FormTemplateService : IFormTemplateService
{
    private readonly FastFormDbContext _context;

    public async Task<FormTemplate> CreateTemplateAsync(FormTemplate template)
    {
        var templateName = template.Name;
        // ...
    }
}
```

### XAML Guidelines

- 4 space indentation
- Attributes her satırda bir tane (uzun durumda)
- Anlamlı x:Name kullanın

**Örnek:**

```xaml
<Button x:Name="SaveButton"
        Content="Kaydet"
        Command="{Binding SaveCommand}"
        Style="{StaticResource PrimaryButton}"/>
```

### Naming Conventions

- **Services:** `IFormTemplateService`, `FormTemplateService`
- **ViewModels:** `MainViewModel`, `FormEditorViewModel`
- **Views:** `MainWindow`, `FormEditorView`
- **Models:** `FormTemplate`, `FieldDefinition`

## Commit Mesajları

Conventional Commits formatını kullanın:

### Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- **feat:** Yeni özellik
- **fix:** Bug fix
- **docs:** Dokümantasyon
- **style:** Code style değişiklikleri
- **refactor:** Refactoring
- **test:** Test ekleme/düzenleme
- **chore:** Build, CI/CD vb.

### Örnekler

```
feat(export): PDF export özelliği eklendi

- QuestPDF kütüphanesi entegre edildi
- FormInstance'dan PDF oluşturma
- Batch export desteği

Closes #123
```

```
fix(validation): Email validasyonu düzeltildi

Regex pattern güncellendi ve test edildi.

Fixes #456
```

```
docs(readme): Kurulum adımları güncellendi

SQL Server kurulum detayları eklendi.
```

## Pull Request Süreci

### 1. Branch Oluştur

Feature veya fix için yeni branch:

```bash
git checkout -b feature/pdf-export
# veya
git checkout -b fix/email-validation
```

### 2. Değişiklikler Yap

- Kod yazın
- Test edin
- Dokümante edin

### 3. Commit

```bash
git add .
git commit -m "feat(export): PDF export özelliği eklendi"
```

### 4. Push

```bash
git push origin feature/pdf-export
```

### 5. Pull Request Aç

1. GitHub'da repository'nize gidin
2. "New Pull Request" tıklayın
3. Base: `main`, Compare: `feature/pdf-export`
4. Başlık ve açıklama yazın
5. "Create Pull Request" tıklayın

### PR Template

```markdown
## Açıklama

Bu PR şu değişiklikleri içerir:
- PDF export özelliği
- Batch export desteği

## Değişiklik Tipi

- [x] Yeni özellik (breaking change değil)
- [ ] Bug fix (breaking change değil)
- [ ] Breaking change

## Test

- [x] Unit testler eklendi
- [x] Integration testler eklendi
- [x] Manuel test yapıldı

## Checklist

- [x] Kod standartlarına uygun
- [x] Dokümantasyon güncellendi
- [x] Test coverage %80 üzeri
- [x] Breaking change yok
```

## Test

### Unit Tests

XUnit kullanın:

```csharp
public class FormTemplateServiceTests
{
    [Fact]
    public async Task CreateTemplate_ShouldCreateSuccessfully()
    {
        // Arrange
        var template = new FormTemplate { Name = "Test" };

        // Act
        var result = await _service.CreateTemplateAsync(template);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
    }
}
```

### Integration Tests

```csharp
public class FormWorkflowTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task FullWorkflow_ShouldCompleteSuccessfully()
    {
        // Test full form creation to export workflow
    }
}
```

### Test Coverage

Minimum %80 code coverage hedeflenir:

```bash
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=cobertura
```

## Dokümantasyon

### Code Comments

XML documentation comments kullanın:

```csharp
/// <summary>
/// Creates a new form template.
/// </summary>
/// <param name="template">The template to create</param>
/// <returns>The created template with ID</returns>
/// <exception cref="ArgumentNullException">Thrown when template is null</exception>
public async Task<FormTemplate> CreateTemplateAsync(FormTemplate template)
{
    // Implementation
}
```

### README Updates

Yeni özellik eklerken README'yi güncelleyin:

- Özellik açıklaması
- Kullanım örneği
- Yapılandırma (varsa)

### CHANGELOG

CHANGELOG.md dosyasını güncelleyin:

```markdown
## [Unreleased]

### Added
- PDF export özelliği (#123)

### Fixed
- Email validation bug (#456)
```

## Code Review

### Review Süreci

1. PR oluşturulur
2. Maintainer'lar review yapar
3. Gerekli değişiklikler yapılır
4. Approve edilir
5. Merge edilir

### Review Checklist

Reviewers şunları kontrol eder:

- [ ] Kod standartlarına uygunluk
- [ ] Test coverage
- [ ] Dokümantasyon
- [ ] Performance
- [ ] Security
- [ ] Breaking changes

## İletişim

- **Issues:** GitHub Issues kullanın
- **Discussions:** GitHub Discussions kullanın
- **Email:** maintainer@fastform.com

## Davranış Kuralları

### Pozitif Ortam

- Saygılı olun
- Yapıcı eleştiri yapın
- Farklı görüşlere açık olun
- Yardımcı olun

### Kabul Edilemez Davranışlar

- Hakaret
- Taciz
- Spam
- Troll

## Lisans

Katkılarınız MIT License altında lisanslanır.

## Teşekkürler

FastForm'a katkıda bulunduğunuz için teşekkür ederiz! 🎉

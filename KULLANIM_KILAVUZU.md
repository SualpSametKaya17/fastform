# FastForm Kullanım Kılavuzu

## İçindekiler
1. [Giriş](#giriş)
2. [Form Şablonu Oluşturma](#form-şablonu-oluşturma)
3. [Alan Tanımlama](#alan-tanımlama)
4. [Form Doldurma](#form-doldurma)
5. [Export İşlemleri](#export-işlemleri)
6. [İleri Seviye Özellikler](#ileri-seviye-özellikler)

## Giriş

FastForm, fiziksel formları dijitalleştirip kolayca doldurmanıza olanak sağlayan bir masaüstü uygulamasıdır.

### İlk Adımlar

1. Uygulamayı başlatın
2. Admin kullanıcısı ile giriş yapın (ilk kurulumda)
3. Dashboard'dan başlayın

## Form Şablonu Oluşturma

### Adım 1: Yeni Şablon Oluştur

1. Sol menüden **"Yeni Şablon"** butonuna tıklayın
2. Şablon bilgilerini girin:
   - **Ad:** Şablon adı (örn: "Başvuru Formu")
   - **Açıklama:** Şablon hakkında açıklama
   - **Kategori:** Form kategorisi (örn: "İnsan Kaynakları")

### Adım 2: Form Resmini Yükle

1. **"Resim Seç"** butonuna tıklayın
2. Form resmini seçin (PNG veya JPG)
3. Resim önizleme alanında görüntülenecektir

**İpuçları:**
- Yüksek çözünürlüklü resim kullanın (minimum 1200px genişlik)
- Net ve okunabilir bir form resmi kullanın
- Resmi düz ve düzgün tarayın

### Adım 3: Kaydet

1. **"Şablonu Kaydet"** butonuna tıklayın
2. Şablon kaydedildi mesajını görün

## Alan Tanımlama

### Yeni Alan Ekle

1. Şablon düzenleme ekranında **"Alan Ekle"** butonuna tıklayın
2. Yeni alan form resmi üzerinde görünecektir
3. Alanı sürükleyerek konumlandırın

### Alan Özellikleri

#### Temel Özellikler

- **Alan Adı:** Alanın teknik adı (örn: "adi_soyadi")
- **Alan Etiketi:** Kullanıcıya gösterilen ad (örn: "Adı Soyadı")
- **Alan Tipi:** Veri tipi seçin
  - **Metin:** Serbest metin
  - **Sayı:** Sadece rakamlar
  - **Tarih:** Tarih seçici
  - **Dropdown:** Açılır liste
  - **Checkbox:** Onay kutusu
  - **İmza:** İmza alanı

#### Pozisyon ve Boyut

- **X Koordinatı:** Yatay konum (piksel)
- **Y Koordinatı:** Dikey konum (piksel)
- **Genişlik:** Alan genişliği (piksel)
- **Yükseklik:** Alan yüksekliği (piksel)

**İpucu:** Alanı fare ile sürükleyerek de konumlandırabilirsiniz.

#### Stil Özellikleri

- **Font Ailesi:** Arial, Times New Roman, vb.
- **Font Boyutu:** 8-72 pt arası
- **Font Rengi:** Hex renk kodu (#000000)
- **Kalın:** Kalın yazı
- **İtalik:** Eğik yazı
- **Alt Çizgi:** Alt çizgili yazı
- **Hizalama:** Sol, Orta, Sağ

#### Validasyon Kuralları

- **Zorunlu:** Alan doldurulmalı mı?
- **Minimum Uzunluk:** En az karakter sayısı
- **Maximum Uzunluk:** En fazla karakter sayısı
- **Regex Pattern:** Düzenli ifade ile doğrulama
- **Hata Mesajı:** Hata durumunda gösterilecek mesaj

**Örnekler:**

Email validasyonu:
```
Regex: ^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$
Mesaj: Geçerli bir e-posta adresi giriniz
```

Telefon validasyonu:
```
Regex: ^0[0-9]{10}$
Mesaj: 11 haneli telefon numarası giriniz (0 ile başlayan)
```

TC Kimlik No:
```
Regex: ^[1-9][0-9]{10}$
Mesaj: 11 haneli TC Kimlik No giriniz
```

### Dropdown Seçenekleri

Dropdown alan tipi için seçenekler tanımlayın:

```json
[
  {"value": "erkek", "text": "Erkek"},
  {"value": "kadin", "text": "Kadın"},
  {"value": "diger", "text": "Belirtmek İstemiyorum"}
]
```

### Alanları Kaydet

1. Her alan eklemeden sonra **"Alan Kaydet"** tıklayın
2. Tüm alanlar eklendiğinde **"Şablonu Kaydet"** tıklayın

## Form Doldurma

### Yeni Form Oluştur

1. Ana menüden **"Yeni Form"** seçin
2. Şablon listesinden bir şablon seçin
3. Form otomatik olarak oluşturulacaktır

### Alanları Doldur

1. Her alan için ilgili veriyi girin
2. Zorunlu alanlar kırmızı (*) ile işaretlidir
3. Hatalı girişler için uyarı mesajı görürsünüz

### Önizleme

1. **"Önizleme"** butonuna tıklayın
2. Doldurduğunuz formun son halini görün
3. Gerekirse düzeltme yapın

### Kaydet ve Tamamla

#### Taslak Olarak Kaydet

- **"Kaydet"** butonu ile formu taslak olarak kaydedin
- Daha sonra devam edebilirsiniz

#### Tamamla

- **"Tamamla"** butonu ile formu tamamlayın
- Tamamlanan formlar düzenlenemez

## Export İşlemleri

### Tek Form Export

1. Formu açın
2. **"Dışa Aktar"** menüsünden format seçin:
   - PDF (önerilen)
   - PNG
   - JPEG

### Toplu Export

1. **"Formlar"** sayfasından export edilecek formları seçin
2. **"Toplu Dışa Aktar"** butonuna tıklayın
3. Format ve hedef klasör seçin
4. **"Başlat"** tıklayın

### Export Ayarları

**Kalite Ayarı:**
- Düşük (70): Küçük dosya boyutu
- Orta (85): Dengeli
- Yüksek (95): En iyi kalite (önerilen)

## İleri Seviye Özellikler

### Konfigürasyon Import/Export

#### Export

1. Şablon düzenleme ekranında **"Konfigürasyonu Dışa Aktar"** tıklayın
2. JSON dosyasını kaydedin
3. Başka sistemlerde kullanabilirsiniz

#### Import

1. **"Konfigürasyonu İçe Aktar"** tıklayın
2. JSON dosyasını seçin
3. Tüm alanlar otomatik olarak oluşturulur

### Toplu Form Doldurma (Batch Processing)

Excel veya CSV dosyasından toplu form doldurma:

1. **"Toplu İşlemler"** → **"Yeni İş"**
2. Şablon seçin
3. Excel/CSV dosyasını yükleyin
4. Sütun eşleştirmesi yapın
5. İşlemi başlatın

**Excel Örneği:**

| adi_soyadi | email | telefon |
|------------|-------|---------|
| Ahmet Yılmaz | ahmet@example.com | 05551234567 |
| Ayşe Demir | ayse@example.com | 05559876543 |

### Version Control

Her şablon değişikliği otomatik olarak kaydedilir:

1. **"Geçmiş"** sekmesinden eski versiyonları görün
2. İstediğiniz versiyona geri dönün
3. Değişiklikleri karşılaştırın

### Kullanıcı İzinleri

Şablon bazlı izin tanımlama:

1. Şablon ayarlarından **"İzinler"**
2. Kullanıcı veya rol seçin
3. İzinleri tanımlayın:
   - Görüntüleme
   - Düzenleme
   - Silme
   - Onaylama

### Audit Log (İzleme)

Tüm işlemler kaydedilir:

1. **"Raporlar"** → **"İşlem Geçmişi"**
2. Filtreler:
   - Kullanıcı
   - Tarih aralığı
   - İşlem tipi
   - Entity

### Ayarlar

#### Genel Ayarlar

- **Dil:** Türkçe / English
- **Tema:** Açık / Koyu
- **Otomatik Kaydetme:** Açık/Kapalı
- **Kaydetme Aralığı:** Saniye cinsinden

#### Export Ayarları

- **Varsayılan Format:** PDF / PNG / JPEG
- **Kalite:** 1-100 arası
- **Çıktı Klasörü:** Varsayılan kayıt konumu

## Kısayol Tuşları

- **Ctrl + N:** Yeni form
- **Ctrl + S:** Kaydet
- **Ctrl + P:** Önizleme
- **Ctrl + E:** Export
- **F5:** Yenile
- **ESC:** İptal

## Sorun Giderme

### Form resmi bulanık görünüyor

**Çözüm:** Daha yüksek çözünürlüklü resim kullanın (min 1200px genişlik)

### Alanlar yanlış konumda

**Çözüm:**
1. Zoom seviyesini kontrol edin
2. Koordinatları manuel olarak ayarlayın
3. Resmi yeniden yükleyin

### Export başarısız oluyor

**Çözüm:**
1. Output klasörünün yazılabilir olduğundan emin olun
2. Yeterli disk alanı kontrol edin
3. Antivirüs yazılımını kontrol edin

### Validasyon çalışmıyor

**Çözüm:**
1. Regex pattern'i test edin
2. Alan tipini kontrol edin
3. Zorunlu alan işaretini kontrol edin

## En İyi Uygulamalar

### Form Tasarımı

1. Net ve okunabilir form resimleri kullanın
2. Alanları mantıksal sırada yerleştirin
3. Tutarlı font ve stil kullanın
4. Yeterli boşluk bırakın

### Alan Tanımlama

1. Anlamlı alan adları kullanın (adi_soyadi, email)
2. Uygun validasyon kuralları ekleyin
3. Yardım metinleri yazın
4. Varsayılan değerler tanımlayın

### Veri Girişi

1. Formları düzenli olarak kaydedin
2. Önizlemeyi kullanın
3. Hataları hemen düzeltin
4. Notlar ekleyin

### Yedekleme

1. Konfigürasyonları düzenli olarak export edin
2. Veritabanı yedeği alın
3. Form resimlerini saklayın

## Destek

Sorun yaşıyorsanız:

1. Dokümantasyonu kontrol edin
2. SSS bölümüne bakın
3. Log dosyalarını inceleyin (`logs/` klasörü)
4. Destek ekibine başvurun

## Güncellemeler

Yeni versiyonlar için:

1. GitHub repository'yi takip edin
2. Release notes'u okuyun
3. Yedek alın
4. Güncellemeyi yapın

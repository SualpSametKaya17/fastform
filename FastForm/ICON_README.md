# FastForm Icon

Bu proje için bir `.ico` dosyası oluşturmak isterseniz:

## Online Icon Oluşturucu

1. [favicon.io](https://favicon.io/) - Ücretsiz
2. [icoconvert.com](https://icoconvert.com/) - PNG'den ICO'ya

## Veya Kendi İkonunuzu Kullanın

1. `FastForm/app.ico` dosyası oluşturun
2. `FastForm.csproj` dosyasına ekleyin:

```xml
<PropertyGroup>
  <ApplicationIcon>app.ico</ApplicationIcon>
</PropertyGroup>
```

## Şimdilik

Icon devre dışı bırakıldı, uygulama varsayılan Windows ikonu kullanacak.

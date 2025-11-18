-- FastForm Extended Database Schema
-- Microsoft SQL Server

-- Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Email NVARCHAR(200),
    FullName NVARCHAR(200),
    Role NVARCHAR(50) NOT NULL DEFAULT 'User', -- Admin, Manager, User, Viewer
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginDate DATETIME2,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Form Templates Table (Extended)
CREATE TABLE FormTemplates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Category NVARCHAR(100),
    ImagePath NVARCHAR(500) NOT NULL,
    ImageData VARBINARY(MAX),
    ThumbnailData VARBINARY(MAX),
    ConfigurationPath NVARCHAR(500),
    Version INT NOT NULL DEFAULT 1,
    IsActive BIT NOT NULL DEFAULT 1,
    IsPublic BIT NOT NULL DEFAULT 0,
    CreatedById INT,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedById INT,
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Tags NVARCHAR(500),
    CONSTRAINT FK_FormTemplate_CreatedBy FOREIGN KEY (CreatedById) REFERENCES Users(Id),
    CONSTRAINT FK_FormTemplate_ModifiedBy FOREIGN KEY (ModifiedById) REFERENCES Users(Id),
    CONSTRAINT UQ_FormTemplate_Name_Version UNIQUE(Name, Version)
);

-- Field Definitions Table (Extended)
CREATE TABLE FieldDefinitions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    FieldName NVARCHAR(100) NOT NULL,
    FieldLabel NVARCHAR(200),
    FieldType NVARCHAR(50) NOT NULL, -- Text, Number, Date, Dropdown, Checkbox, Signature, Barcode
    X FLOAT NOT NULL,
    Y FLOAT NOT NULL,
    Width FLOAT NOT NULL,
    Height FLOAT NOT NULL,

    -- Styling
    FontFamily NVARCHAR(100) DEFAULT 'Arial',
    FontSize FLOAT DEFAULT 12,
    FontColor NVARCHAR(20) DEFAULT '#000000',
    BackgroundColor NVARCHAR(20),
    BorderColor NVARCHAR(20),
    BorderThickness FLOAT DEFAULT 0,
    IsBold BIT DEFAULT 0,
    IsItalic BIT DEFAULT 0,
    IsUnderline BIT DEFAULT 0,
    TextAlignment NVARCHAR(20) DEFAULT 'Left', -- Left, Center, Right
    VerticalAlignment NVARCHAR(20) DEFAULT 'Top', -- Top, Middle, Bottom

    -- Validation
    IsRequired BIT DEFAULT 0,
    DefaultValue NVARCHAR(MAX),
    PlaceholderText NVARCHAR(200),
    ValidationRegex NVARCHAR(500),
    ValidationMessage NVARCHAR(500),
    MinLength INT,
    MaxLength INT,
    MinValue DECIMAL(18,2),
    MaxValue DECIMAL(18,2),

    -- Dropdown/Checkbox Options (JSON array)
    FieldOptions NVARCHAR(MAX),

    -- Metadata
    TabIndex INT DEFAULT 0,
    IsReadOnly BIT DEFAULT 0,
    IsVisible BIT DEFAULT 1,
    HelpText NVARCHAR(500),

    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Field_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Field_Name_Template UNIQUE(FormTemplateId, FieldName)
);

-- Form Instances Table (Extended)
CREATE TABLE FormInstances (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    InstanceName NVARCHAR(200),
    InstanceNumber NVARCHAR(50) UNIQUE,
    Status NVARCHAR(50) DEFAULT 'Draft', -- Draft, InProgress, Completed, Approved, Rejected, Archived
    Priority NVARCHAR(20) DEFAULT 'Normal', -- Low, Normal, High, Urgent

    -- Ownership
    CreatedById INT,
    AssignedToId INT,
    ApprovedById INT,

    -- Dates
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CompletedDate DATETIME2,
    ApprovedDate DATETIME2,
    DueDate DATETIME2,

    -- Output
    OutputPath NVARCHAR(500),
    OutputFormat NVARCHAR(50), -- PDF, PNG, DOCX

    -- Metadata
    Notes NVARCHAR(MAX),
    Tags NVARCHAR(500),

    CONSTRAINT FK_Instance_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Instance_CreatedBy FOREIGN KEY (CreatedById)
        REFERENCES Users(Id),
    CONSTRAINT FK_Instance_AssignedTo FOREIGN KEY (AssignedToId)
        REFERENCES Users(Id),
    CONSTRAINT FK_Instance_ApprovedBy FOREIGN KEY (ApprovedById)
        REFERENCES Users(Id)
);

-- Field Values Table
CREATE TABLE FieldValues (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormInstanceId INT NOT NULL,
    FieldDefinitionId INT NOT NULL,
    FieldValue NVARCHAR(MAX),

    -- For file/signature fields
    BinaryData VARBINARY(MAX),

    -- Metadata
    CreatedById INT,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedById INT,
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_FieldValue_Instance FOREIGN KEY (FormInstanceId)
        REFERENCES FormInstances(Id) ON DELETE CASCADE,
    CONSTRAINT FK_FieldValue_Definition FOREIGN KEY (FieldDefinitionId)
        REFERENCES FieldDefinitions(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_FieldValue_CreatedBy FOREIGN KEY (CreatedById)
        REFERENCES Users(Id),
    CONSTRAINT FK_FieldValue_ModifiedBy FOREIGN KEY (ModifiedById)
        REFERENCES Users(Id),
    CONSTRAINT UQ_FieldValue_Instance_Definition UNIQUE(FormInstanceId, FieldDefinitionId)
);

-- Configuration History Table
CREATE TABLE ConfigurationHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    ConfigurationJson NVARCHAR(MAX) NOT NULL,
    ChangeDescription NVARCHAR(500),
    ChangedById INT,
    ChangedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ConfigHistory_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ConfigHistory_ChangedBy FOREIGN KEY (ChangedById)
        REFERENCES Users(Id)
);

-- Audit Log Table
CREATE TABLE AuditLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    Action NVARCHAR(100) NOT NULL, -- Created, Updated, Deleted, Exported, Approved, etc.
    EntityType NVARCHAR(100) NOT NULL, -- FormTemplate, FormInstance, FieldDefinition, etc.
    EntityId INT,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    IpAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AuditLog_User FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);

-- Settings Table
CREATE TABLE Settings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX),
    Category NVARCHAR(100),
    Description NVARCHAR(500),
    DataType NVARCHAR(50), -- String, Int, Bool, Json
    IsSystem BIT DEFAULT 0,
    ModifiedById INT,
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Settings_ModifiedBy FOREIGN KEY (ModifiedById)
        REFERENCES Users(Id)
);

-- Form Template Permissions
CREATE TABLE FormTemplatePermissions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    UserId INT,
    RoleName NVARCHAR(50),
    CanView BIT DEFAULT 1,
    CanEdit BIT DEFAULT 0,
    CanDelete BIT DEFAULT 0,
    CanApprove BIT DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Permission_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Permission_User FOREIGN KEY (UserId)
        REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT CK_Permission_UserOrRole CHECK (UserId IS NOT NULL OR RoleName IS NOT NULL)
);

-- Batch Processing Jobs
CREATE TABLE BatchJobs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    JobName NVARCHAR(200) NOT NULL,
    FormTemplateId INT NOT NULL,
    DataSource NVARCHAR(500), -- CSV, Excel file path
    TotalRecords INT,
    ProcessedRecords INT DEFAULT 0,
    FailedRecords INT DEFAULT 0,
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending, Running, Completed, Failed, Cancelled
    StartDate DATETIME2,
    EndDate DATETIME2,
    CreatedById INT,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ErrorLog NVARCHAR(MAX),
    CONSTRAINT FK_BatchJob_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id),
    CONSTRAINT FK_BatchJob_CreatedBy FOREIGN KEY (CreatedById)
        REFERENCES Users(Id)
);

-- Localization/Translations
CREATE TABLE Translations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    LanguageCode NVARCHAR(10) NOT NULL, -- en, tr, de, etc.
    ResourceKey NVARCHAR(200) NOT NULL,
    ResourceValue NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(100),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Translation_Lang_Key UNIQUE(LanguageCode, ResourceKey)
);

-- Indexes for better performance
CREATE INDEX IX_FormTemplates_IsActive ON FormTemplates(IsActive);
CREATE INDEX IX_FormTemplates_CreatedDate ON FormTemplates(CreatedDate DESC);
CREATE INDEX IX_FormTemplates_Category ON FormTemplates(Category);
CREATE INDEX IX_FormTemplates_Tags ON FormTemplates(Tags);
CREATE INDEX IX_FieldDefinitions_FormTemplateId ON FieldDefinitions(FormTemplateId);
CREATE INDEX IX_FieldDefinitions_FieldType ON FieldDefinitions(FieldType);
CREATE INDEX IX_FormInstances_FormTemplateId ON FormInstances(FormTemplateId);
CREATE INDEX IX_FormInstances_Status ON FormInstances(Status);
CREATE INDEX IX_FormInstances_CreatedDate ON FormInstances(CreatedDate DESC);
CREATE INDEX IX_FormInstances_DueDate ON FormInstances(DueDate);
CREATE INDEX IX_FormInstances_AssignedToId ON FormInstances(AssignedToId);
CREATE INDEX IX_FieldValues_FormInstanceId ON FieldValues(FormInstanceId);
CREATE INDEX IX_FieldValues_FieldDefinitionId ON FieldValues(FieldDefinitionId);
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
CREATE INDEX IX_AuditLogs_EntityType_EntityId ON AuditLogs(EntityType, EntityId);
CREATE INDEX IX_AuditLogs_CreatedDate ON AuditLogs(CreatedDate DESC);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);

GO

-- Insert default admin user (password: Admin123!)
-- Note: In production, use proper password hashing
INSERT INTO Users (Username, PasswordHash, Email, FullName, Role, IsActive)
VALUES ('admin', 'AQAAAAEAACcQAAAAEKfV8qY6w8YnG9Y8qY6w8YnG9Y8qY6w8YnG9Y8qY6w8YnG9Y8qY6w8YnG9==',
        'admin@fastform.com', 'System Administrator', 'Admin', 1);

-- Insert default settings
INSERT INTO Settings (SettingKey, SettingValue, Category, Description, DataType, IsSystem)
VALUES
    ('App.Language', 'tr', 'General', 'Default application language', 'String', 1),
    ('App.Theme', 'Light', 'General', 'Application theme', 'String', 1),
    ('Export.DefaultFormat', 'PDF', 'Export', 'Default export format', 'String', 0),
    ('Export.Quality', '95', 'Export', 'Export image quality (1-100)', 'Int', 0),
    ('Form.AutoSave', 'true', 'Forms', 'Enable auto-save', 'Bool', 0),
    ('Form.AutoSaveInterval', '60', 'Forms', 'Auto-save interval in seconds', 'Int', 0),
    ('Database.BackupEnabled', 'true', 'Database', 'Enable automatic backups', 'Bool', 1),
    ('Database.BackupInterval', '24', 'Database', 'Backup interval in hours', 'Int', 1);

-- Insert sample translations (Turkish)
INSERT INTO Translations (LanguageCode, ResourceKey, ResourceValue, Category)
VALUES
    ('tr', 'App.Title', 'FastForm - Form Doldurma Sistemi', 'General'),
    ('tr', 'Menu.Dashboard', 'Kontrol Paneli', 'Menu'),
    ('tr', 'Menu.Templates', 'Form Şablonları', 'Menu'),
    ('tr', 'Menu.Forms', 'Formlar', 'Menu'),
    ('tr', 'Menu.Users', 'Kullanıcılar', 'Menu'),
    ('tr', 'Menu.Settings', 'Ayarlar', 'Menu'),
    ('tr', 'Button.Save', 'Kaydet', 'Common'),
    ('tr', 'Button.Cancel', 'İptal', 'Common'),
    ('tr', 'Button.Delete', 'Sil', 'Common'),
    ('tr', 'Button.Edit', 'Düzenle', 'Common'),
    ('tr', 'Button.New', 'Yeni', 'Common'),
    ('tr', 'Button.Export', 'Dışa Aktar', 'Common'),
    ('tr', 'Status.Draft', 'Taslak', 'Status'),
    ('tr', 'Status.Completed', 'Tamamlandı', 'Status'),
    ('tr', 'Status.Approved', 'Onaylandı', 'Status');

-- Insert sample translations (English)
INSERT INTO Translations (LanguageCode, ResourceKey, ResourceValue, Category)
VALUES
    ('en', 'App.Title', 'FastForm - Form Filling System', 'General'),
    ('en', 'Menu.Dashboard', 'Dashboard', 'Menu'),
    ('en', 'Menu.Templates', 'Form Templates', 'Menu'),
    ('en', 'Menu.Forms', 'Forms', 'Menu'),
    ('en', 'Menu.Users', 'Users', 'Menu'),
    ('en', 'Menu.Settings', 'Settings', 'Menu'),
    ('en', 'Button.Save', 'Save', 'Common'),
    ('en', 'Button.Cancel', 'Cancel', 'Common'),
    ('en', 'Button.Delete', 'Delete', 'Common'),
    ('en', 'Button.Edit', 'Edit', 'Common'),
    ('en', 'Button.New', 'New', 'Common'),
    ('en', 'Button.Export', 'Export', 'Common'),
    ('en', 'Status.Draft', 'Draft', 'Status'),
    ('en', 'Status.Completed', 'Completed', 'Status'),
    ('en', 'Status.Approved', 'Approved', 'Status');

GO

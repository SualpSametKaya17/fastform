-- FastForm Database Schema
-- Microsoft SQL Server

-- Form Templates Table
CREATE TABLE FormTemplates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    ImagePath NVARCHAR(500) NOT NULL,
    ImageData VARBINARY(MAX),
    ConfigurationPath NVARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_FormTemplate_Name UNIQUE(Name)
);

-- Field Definitions Table
CREATE TABLE FieldDefinitions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    FieldName NVARCHAR(100) NOT NULL,
    FieldType NVARCHAR(50) NOT NULL, -- Text, Number, Date, etc.
    X FLOAT NOT NULL,
    Y FLOAT NOT NULL,
    Width FLOAT NOT NULL,
    Height FLOAT NOT NULL,
    FontFamily NVARCHAR(100) DEFAULT 'Arial',
    FontSize FLOAT DEFAULT 12,
    FontColor NVARCHAR(20) DEFAULT '#000000',
    IsBold BIT DEFAULT 0,
    IsItalic BIT DEFAULT 0,
    TextAlignment NVARCHAR(20) DEFAULT 'Left', -- Left, Center, Right
    IsRequired BIT DEFAULT 0,
    DefaultValue NVARCHAR(MAX),
    ValidationRegex NVARCHAR(500),
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Field_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Field_Name_Template UNIQUE(FormTemplateId, FieldName)
);

-- Form Instances Table (Filled Forms)
CREATE TABLE FormInstances (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    InstanceName NVARCHAR(200),
    Status NVARCHAR(50) DEFAULT 'Draft', -- Draft, Completed, Archived
    CreatedBy NVARCHAR(100),
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CompletedDate DATETIME2,
    OutputPath NVARCHAR(500),
    CONSTRAINT FK_Instance_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE NO ACTION
);

-- Field Values Table
CREATE TABLE FieldValues (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormInstanceId INT NOT NULL,
    FieldDefinitionId INT NOT NULL,
    FieldValue NVARCHAR(MAX),
    CreatedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_FieldValue_Instance FOREIGN KEY (FormInstanceId)
        REFERENCES FormInstances(Id) ON DELETE CASCADE,
    CONSTRAINT FK_FieldValue_Definition FOREIGN KEY (FieldDefinitionId)
        REFERENCES FieldDefinitions(Id) ON DELETE NO ACTION,
    CONSTRAINT UQ_FieldValue_Instance_Definition UNIQUE(FormInstanceId, FieldDefinitionId)
);

-- Configuration History Table (For tracking changes)
CREATE TABLE ConfigurationHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormTemplateId INT NOT NULL,
    ConfigurationJson NVARCHAR(MAX) NOT NULL,
    ChangeDescription NVARCHAR(500),
    ChangedBy NVARCHAR(100),
    ChangedDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ConfigHistory_FormTemplate FOREIGN KEY (FormTemplateId)
        REFERENCES FormTemplates(Id) ON DELETE CASCADE
);

-- Indexes for better performance
CREATE INDEX IX_FormTemplates_IsActive ON FormTemplates(IsActive);
CREATE INDEX IX_FormTemplates_CreatedDate ON FormTemplates(CreatedDate DESC);
CREATE INDEX IX_FieldDefinitions_FormTemplateId ON FieldDefinitions(FormTemplateId);
CREATE INDEX IX_FormInstances_FormTemplateId ON FormInstances(FormTemplateId);
CREATE INDEX IX_FormInstances_Status ON FormInstances(Status);
CREATE INDEX IX_FormInstances_CreatedDate ON FormInstances(CreatedDate DESC);
CREATE INDEX IX_FieldValues_FormInstanceId ON FieldValues(FormInstanceId);
CREATE INDEX IX_FieldValues_FieldDefinitionId ON FieldValues(FieldDefinitionId);

GO

-- Sample Data
INSERT INTO FormTemplates (Name, Description, ImagePath, IsActive)
VALUES
    ('Form 1', 'İlk form şablonu', 'forms/form1.png', 1),
    ('Form 2', 'İkinci form şablonu', 'forms/form2.png', 1);

GO

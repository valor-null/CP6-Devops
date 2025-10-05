IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Cliente] (
    [IdCliente] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [CPF] nvarchar(11) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [DataCadastro] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_Cliente] PRIMARY KEY ([IdCliente])
);
GO

CREATE TABLE [ContaCorrente] (
    [IdConta] int NOT NULL IDENTITY,
    [NumeroConta] nvarchar(20) NOT NULL,
    [Saldo] decimal(18,2) NOT NULL DEFAULT 0.0,
    [TipoConta] nvarchar(50) NOT NULL DEFAULT N'Corrente',
    [IdCliente] int NOT NULL,
    CONSTRAINT [PK_ContaCorrente] PRIMARY KEY ([IdConta]),
    CONSTRAINT [FK_ContaCorrente_Cliente_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente]) ON DELETE CASCADE
);
GO

CREATE TABLE [Transacao] (
    [IdTransacao] int NOT NULL IDENTITY,
    [IdConta] int NOT NULL,
    [Tipo] nvarchar(20) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [DataHora] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_Transacao] PRIMARY KEY ([IdTransacao]),
    CONSTRAINT [FK_Transacao_ContaCorrente_IdConta] FOREIGN KEY ([IdConta]) REFERENCES [ContaCorrente] ([IdConta]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_ContaCorrente_IdCliente] ON [ContaCorrente] ([IdCliente]);
GO

CREATE INDEX [IX_Transacao_IdConta] ON [Transacao] ([IdConta]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251005021118_InitialCreate', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [ContaCorrente] DROP CONSTRAINT [FK_ContaCorrente_Cliente_IdCliente];
GO

ALTER TABLE [Transacao] DROP CONSTRAINT [FK_Transacao_ContaCorrente_IdConta];
GO

DROP INDEX [IX_Transacao_IdConta] ON [Transacao];
GO

EXEC sp_rename N'[ContaCorrente].[IX_ContaCorrente_IdCliente]', N'IX_Conta_IdCliente', N'INDEX';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Transacao]') AND [c].[name] = N'Tipo');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Transacao] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Transacao] ALTER COLUMN [Tipo] nvarchar(15) NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Transacao]') AND [c].[name] = N'DataHora');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Transacao] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Transacao] ADD DEFAULT (SYSUTCDATETIME()) FOR [DataHora];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ContaCorrente]') AND [c].[name] = N'TipoConta');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ContaCorrente] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [ContaCorrente] ALTER COLUMN [TipoConta] nvarchar(20) NOT NULL;
GO

ALTER TABLE [ContaCorrente] ADD [RowVersion] rowversion NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cliente]') AND [c].[name] = N'Nome');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Cliente] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Cliente] ALTER COLUMN [Nome] nvarchar(150) NOT NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cliente]') AND [c].[name] = N'Email');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Cliente] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Cliente] ALTER COLUMN [Email] nvarchar(255) NOT NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cliente]') AND [c].[name] = N'DataCadastro');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Cliente] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Cliente] ADD DEFAULT (SYSUTCDATETIME()) FOR [DataCadastro];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cliente]') AND [c].[name] = N'CPF');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Cliente] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Cliente] ALTER COLUMN [CPF] char(11) NOT NULL;
GO

CREATE INDEX [IX_Transacao_IdConta_DataHora] ON [Transacao] ([IdConta], [DataHora] DESC);
GO

ALTER TABLE [Transacao] ADD CONSTRAINT [CK_Transacao_Tipo] CHECK (Tipo IN ('CREDITO','DEBITO','TRANSFERENCIA'));
GO

ALTER TABLE [Transacao] ADD CONSTRAINT [CK_Transacao_Valor] CHECK (Valor > 0);
GO

CREATE UNIQUE INDEX [UQ_ContaCorrente_NumeroConta] ON [ContaCorrente] ([NumeroConta]);
GO

ALTER TABLE [ContaCorrente] ADD CONSTRAINT [CK_ContaCorrente_TipoConta] CHECK (TipoConta IN ('Corrente','Poupanca'));
GO

CREATE UNIQUE INDEX [UQ_Cliente_CPF] ON [Cliente] ([CPF]);
GO

CREATE UNIQUE INDEX [UQ_Cliente_Email] ON [Cliente] ([Email]);
GO

ALTER TABLE [ContaCorrente] ADD CONSTRAINT [FK_ContaCorrente_Cliente_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente]) ON DELETE NO ACTION;
GO

ALTER TABLE [Transacao] ADD CONSTRAINT [FK_Transacao_ContaCorrente_IdConta] FOREIGN KEY ([IdConta]) REFERENCES [ContaCorrente] ([IdConta]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251005185021_Initial', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251005185108_HardeningSchema', N'8.0.7');
GO

COMMIT;
GO


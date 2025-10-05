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
CREATE TABLE [Cliente] (
    [IdCliente] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [CPF] nvarchar(11) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [DataCadastro] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_Cliente] PRIMARY KEY ([IdCliente])
);

CREATE TABLE [ContaCorrente] (
    [IdConta] int NOT NULL IDENTITY,
    [NumeroConta] nvarchar(20) NOT NULL,
    [Saldo] decimal(18,2) NOT NULL DEFAULT 0.0,
    [TipoConta] nvarchar(50) NOT NULL DEFAULT N'Corrente',
    [IdCliente] int NOT NULL,
    CONSTRAINT [PK_ContaCorrente] PRIMARY KEY ([IdConta]),
    CONSTRAINT [FK_ContaCorrente_Cliente_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [Cliente] ([IdCliente]) ON DELETE CASCADE
);

CREATE TABLE [Transacao] (
    [IdTransacao] int NOT NULL IDENTITY,
    [IdConta] int NOT NULL,
    [Tipo] nvarchar(20) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [DataHora] datetime2 NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [PK_Transacao] PRIMARY KEY ([IdTransacao]),
    CONSTRAINT [FK_Transacao_ContaCorrente_IdConta] FOREIGN KEY ([IdConta]) REFERENCES [ContaCorrente] ([IdConta]) ON DELETE CASCADE
);

CREATE INDEX [IX_ContaCorrente_IdCliente] ON [ContaCorrente] ([IdCliente]);

CREATE INDEX [IX_Transacao_IdConta] ON [Transacao] ([IdConta]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251005021118_InitialCreate', N'9.0.9');

COMMIT;
GO


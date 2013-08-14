
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/31/2013 14:37:32
-- Generated from EDMX file: D:\Project\Hootan\FRN\Databases\FnRDbContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DB_58454_fnr2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Breed_PetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Breeds] DROP CONSTRAINT [FK_Breed_PetType];
GO
IF OBJECT_ID(N'[dbo].[FK_Pet_Breed]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pets] DROP CONSTRAINT [FK_Pet_Breed];
GO
IF OBJECT_ID(N'[dbo].[FK_Pet_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pets] DROP CONSTRAINT [FK_Pet_User];
GO
IF OBJECT_ID(N'[dbo].[FK_User_Vet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_User_Vet];
GO
IF OBJECT_ID(N'[dbo].[FK_Subscription_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscriptions] DROP CONSTRAINT [FK_Subscription_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Subscription_Pet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscriptions] DROP CONSTRAINT [FK_Subscription_Pet];
GO
IF OBJECT_ID(N'[dbo].[FK_Subscription_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscriptions] DROP CONSTRAINT [FK_Subscription_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_Subscription_Vet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscriptions] DROP CONSTRAINT [FK_Subscription_Vet];
GO
IF OBJECT_ID(N'[dbo].[FK_Product_PetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Product_PetType];
GO
IF OBJECT_ID(N'[dbo].[FK_Condition_Vet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conditions] DROP CONSTRAINT [FK_Condition_Vet];
GO
IF OBJECT_ID(N'[dbo].[FK_Condition_Schedule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conditions] DROP CONSTRAINT [FK_Condition_Schedule];
GO
IF OBJECT_ID(N'[dbo].[FK_Schedule_Vet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Schedules] DROP CONSTRAINT [FK_Schedule_Vet];
GO
IF OBJECT_ID(N'[dbo].[FK_Event_Schedule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_Event_Schedule];
GO
IF OBJECT_ID(N'[dbo].[FK_Vet_AvailableProducts_Source]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VetProducts] DROP CONSTRAINT [FK_Vet_AvailableProducts_Source];
GO
IF OBJECT_ID(N'[dbo].[FK_Vet_AvailableProducts_Target]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VetProducts] DROP CONSTRAINT [FK_Vet_AvailableProducts_Target];
GO
IF OBJECT_ID(N'[dbo].[FK_Condition_Pets_Source]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConditionPets] DROP CONSTRAINT [FK_Condition_Pets_Source];
GO
IF OBJECT_ID(N'[dbo].[FK_Condition_Pets_Target]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConditionPets] DROP CONSTRAINT [FK_Condition_Pets_Target];
GO
IF OBJECT_ID(N'[dbo].[FK_Schedule_Users_Source]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ScheduleUsers] DROP CONSTRAINT [FK_Schedule_Users_Source];
GO
IF OBJECT_ID(N'[dbo].[FK_Schedule_Users_Target]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ScheduleUsers] DROP CONSTRAINT [FK_Schedule_Users_Target];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Breeds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Breeds];
GO
IF OBJECT_ID(N'[dbo].[PetTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PetTypes];
GO
IF OBJECT_ID(N'[dbo].[Pets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pets];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Subscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subscriptions];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Vets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vets];
GO
IF OBJECT_ID(N'[dbo].[Conditions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conditions];
GO
IF OBJECT_ID(N'[dbo].[Schedules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Schedules];
GO
IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[VetProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VetProducts];
GO
IF OBJECT_ID(N'[dbo].[ConditionPets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConditionPets];
GO
IF OBJECT_ID(N'[dbo].[ScheduleUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ScheduleUsers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Breeds'
CREATE TABLE [dbo].[Breeds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IdealWeight] float  NOT NULL,
    [PetTypeId] int  NOT NULL
);
GO

-- Creating table 'PetTypes'
CREATE TABLE [dbo].[PetTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pets'
CREATE TABLE [dbo].[Pets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [BreedId] int  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [UserId] int  NOT NULL,
    [Weight] float  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Role] int  NOT NULL,
    [Status] int  NOT NULL,
    [VetId] int  NULL,
    [TokenCustomerID] nvarchar(max)  NULL,
    [RebillCustomerID] nvarchar(max)  NULL
);
GO

-- Creating table 'Subscriptions'
CREATE TABLE [dbo].[Subscriptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [PetId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [VetId] int  NOT NULL,
    [DateSubscribed] datetime  NOT NULL,
    [NextDeliveryDate] datetime  NOT NULL,
    [Sent] bit  NOT NULL,
    [RebillID] nvarchar(max)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Cost] float  NOT NULL,
    [Price] float  NOT NULL,
    [Amount] int  NOT NULL,
    [PetTypeId] int  NOT NULL,
    [LowerWeightLimit] float  NOT NULL,
    [HeigherWeightLimit] float  NOT NULL
);
GO

-- Creating table 'Vets'
CREATE TABLE [dbo].[Vets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL
);
GO

-- Creating table 'Conditions'
CREATE TABLE [dbo].[Conditions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [VetId] int  NOT NULL,
    [ScheduleId] int  NULL
);
GO

-- Creating table 'Schedules'
CREATE TABLE [dbo].[Schedules] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [VetId] int  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [StartDateTime] datetime  NOT NULL,
    [EndDateTime] datetime  NOT NULL,
    [ScheduleId] int  NOT NULL
);
GO

-- Creating table 'Vet_AvailableProducts'
CREATE TABLE [dbo].[Vet_AvailableProducts] (
    [Vets_Id] int  NOT NULL,
    [AvailableProducts_Id] int  NOT NULL
);
GO

-- Creating table 'Condition_Pets'
CREATE TABLE [dbo].[Condition_Pets] (
    [Conditions_Id] int  NOT NULL,
    [Pets_Id] int  NOT NULL
);
GO

-- Creating table 'Schedule_Users'
CREATE TABLE [dbo].[Schedule_Users] (
    [Schedules_Id] int  NOT NULL,
    [Users_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Breeds'
ALTER TABLE [dbo].[Breeds]
ADD CONSTRAINT [PK_Breeds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PetTypes'
ALTER TABLE [dbo].[PetTypes]
ADD CONSTRAINT [PK_PetTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pets'
ALTER TABLE [dbo].[Pets]
ADD CONSTRAINT [PK_Pets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [PK_Subscriptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Vets'
ALTER TABLE [dbo].[Vets]
ADD CONSTRAINT [PK_Vets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Conditions'
ALTER TABLE [dbo].[Conditions]
ADD CONSTRAINT [PK_Conditions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Schedules'
ALTER TABLE [dbo].[Schedules]
ADD CONSTRAINT [PK_Schedules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Vets_Id], [AvailableProducts_Id] in table 'Vet_AvailableProducts'
ALTER TABLE [dbo].[Vet_AvailableProducts]
ADD CONSTRAINT [PK_Vet_AvailableProducts]
    PRIMARY KEY NONCLUSTERED ([Vets_Id], [AvailableProducts_Id] ASC);
GO

-- Creating primary key on [Conditions_Id], [Pets_Id] in table 'Condition_Pets'
ALTER TABLE [dbo].[Condition_Pets]
ADD CONSTRAINT [PK_Condition_Pets]
    PRIMARY KEY NONCLUSTERED ([Conditions_Id], [Pets_Id] ASC);
GO

-- Creating primary key on [Schedules_Id], [Users_Id] in table 'Schedule_Users'
ALTER TABLE [dbo].[Schedule_Users]
ADD CONSTRAINT [PK_Schedule_Users]
    PRIMARY KEY NONCLUSTERED ([Schedules_Id], [Users_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PetTypeId] in table 'Breeds'
ALTER TABLE [dbo].[Breeds]
ADD CONSTRAINT [FK_Breed_PetType]
    FOREIGN KEY ([PetTypeId])
    REFERENCES [dbo].[PetTypes]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Breed_PetType'
CREATE INDEX [IX_FK_Breed_PetType]
ON [dbo].[Breeds]
    ([PetTypeId]);
GO

-- Creating foreign key on [BreedId] in table 'Pets'
ALTER TABLE [dbo].[Pets]
ADD CONSTRAINT [FK_Pet_Breed]
    FOREIGN KEY ([BreedId])
    REFERENCES [dbo].[Breeds]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Pet_Breed'
CREATE INDEX [IX_FK_Pet_Breed]
ON [dbo].[Pets]
    ([BreedId]);
GO

-- Creating foreign key on [UserId] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [FK_Subscription_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Subscription_User'
CREATE INDEX [IX_FK_Subscription_User]
ON [dbo].[Subscriptions]
    ([UserId]);
GO

-- Creating foreign key on [PetId] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [FK_Subscription_Pet]
    FOREIGN KEY ([PetId])
    REFERENCES [dbo].[Pets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Subscription_Pet'
CREATE INDEX [IX_FK_Subscription_Pet]
ON [dbo].[Subscriptions]
    ([PetId]);
GO

-- Creating foreign key on [PetTypeId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Product_PetType]
    FOREIGN KEY ([PetTypeId])
    REFERENCES [dbo].[PetTypes]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Product_PetType'
CREATE INDEX [IX_FK_Product_PetType]
ON [dbo].[Products]
    ([PetTypeId]);
GO

-- Creating foreign key on [Vets_Id] in table 'Vet_AvailableProducts'
ALTER TABLE [dbo].[Vet_AvailableProducts]
ADD CONSTRAINT [FK_Vet_AvailableProducts_Vet_AvailableProducts_Source]
    FOREIGN KEY ([Vets_Id])
    REFERENCES [dbo].[Vets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AvailableProducts_Id] in table 'Vet_AvailableProducts'
ALTER TABLE [dbo].[Vet_AvailableProducts]
ADD CONSTRAINT [FK_Vet_AvailableProducts_Vet_AvailableProducts_Target]
    FOREIGN KEY ([AvailableProducts_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Vet_AvailableProducts_Vet_AvailableProducts_Target'
CREATE INDEX [IX_FK_Vet_AvailableProducts_Vet_AvailableProducts_Target]
ON [dbo].[Vet_AvailableProducts]
    ([AvailableProducts_Id]);
GO

-- Creating foreign key on [VetId] in table 'Conditions'
ALTER TABLE [dbo].[Conditions]
ADD CONSTRAINT [FK_Condition_Vet]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[Vets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Condition_Vet'
CREATE INDEX [IX_FK_Condition_Vet]
ON [dbo].[Conditions]
    ([VetId]);
GO

-- Creating foreign key on [Conditions_Id] in table 'Condition_Pets'
ALTER TABLE [dbo].[Condition_Pets]
ADD CONSTRAINT [FK_Condition_Pets_Condition_Pets_Source]
    FOREIGN KEY ([Conditions_Id])
    REFERENCES [dbo].[Conditions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Pets_Id] in table 'Condition_Pets'
ALTER TABLE [dbo].[Condition_Pets]
ADD CONSTRAINT [FK_Condition_Pets_Condition_Pets_Target]
    FOREIGN KEY ([Pets_Id])
    REFERENCES [dbo].[Pets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Condition_Pets_Condition_Pets_Target'
CREATE INDEX [IX_FK_Condition_Pets_Condition_Pets_Target]
ON [dbo].[Condition_Pets]
    ([Pets_Id]);
GO

-- Creating foreign key on [Schedules_Id] in table 'Schedule_Users'
ALTER TABLE [dbo].[Schedule_Users]
ADD CONSTRAINT [FK_Schedule_Users_Schedule_Users_Source]
    FOREIGN KEY ([Schedules_Id])
    REFERENCES [dbo].[Schedules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_Id] in table 'Schedule_Users'
ALTER TABLE [dbo].[Schedule_Users]
ADD CONSTRAINT [FK_Schedule_Users_Schedule_Users_Target]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Schedule_Users_Schedule_Users_Target'
CREATE INDEX [IX_FK_Schedule_Users_Schedule_Users_Target]
ON [dbo].[Schedule_Users]
    ([Users_Id]);
GO

-- Creating foreign key on [ScheduleId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK_Event_Schedule]
    FOREIGN KEY ([ScheduleId])
    REFERENCES [dbo].[Schedules]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Event_Schedule'
CREATE INDEX [IX_FK_Event_Schedule]
ON [dbo].[Events]
    ([ScheduleId]);
GO

-- Creating foreign key on [VetId] in table 'Schedules'
ALTER TABLE [dbo].[Schedules]
ADD CONSTRAINT [FK_Schedule_Vet]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[Vets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Schedule_Vet'
CREATE INDEX [IX_FK_Schedule_Vet]
ON [dbo].[Schedules]
    ([VetId]);
GO

-- Creating foreign key on [ScheduleId] in table 'Conditions'
ALTER TABLE [dbo].[Conditions]
ADD CONSTRAINT [FK_Condition_Schedule]
    FOREIGN KEY ([ScheduleId])
    REFERENCES [dbo].[Schedules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Condition_Schedule'
CREATE INDEX [IX_FK_Condition_Schedule]
ON [dbo].[Conditions]
    ([ScheduleId]);
GO

-- Creating foreign key on [ProductId] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [FK_Subscription_Product]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Subscription_Product'
CREATE INDEX [IX_FK_Subscription_Product]
ON [dbo].[Subscriptions]
    ([ProductId]);
GO

-- Creating foreign key on [VetId] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [FK_Subscription_Vet]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[Vets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Subscription_Vet'
CREATE INDEX [IX_FK_Subscription_Vet]
ON [dbo].[Subscriptions]
    ([VetId]);
GO

-- Creating foreign key on [VetId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_User_Vet]
    FOREIGN KEY ([VetId])
    REFERENCES [dbo].[Vets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_User_Vet'
CREATE INDEX [IX_FK_User_Vet]
ON [dbo].[Users]
    ([VetId]);
GO

-- Creating foreign key on [UserId] in table 'Pets'
ALTER TABLE [dbo].[Pets]
ADD CONSTRAINT [FK_Pet_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Pet_User'
CREATE INDEX [IX_FK_Pet_User]
ON [dbo].[Pets]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
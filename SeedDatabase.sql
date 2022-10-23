USE CascadeBooks
GO

DECLARE @BeforeTheyAreHangedId      UNIQUEIDENTIFIER = '9BC6B1F8-5A82-4925-BB38-4C0890B25C67',
        @CarrieId                   UNIQUEIDENTIFIER = '2B333A66-9D78-4967-AF79-2D64CE6F4479',
        @ChristineId                UNIQUEIDENTIFIER = 'C36B2014-3A5E-46F7-B83F-0E23453E1363',
        @LastArgumentOfKingsId      UNIQUEIDENTIFIER = '21F82CB7-4328-4A9D-9420-6EEBE08E5E37',
        @TheBladeItselfId           UNIQUEIDENTIFIER = 'E1158088-C85A-4217-BC04-4CD71F9F275B',
        @TheShiningId               UNIQUEIDENTIFIER = '0E679291-2961-4415-B9B9-115EC15874EF',
        @StephenKingId              UNIQUEIDENTIFIER = 'F40B159A-53FE-406C-8102-90100F4236F4',
        @JoeAbercrombieId           UNIQUEIDENTIFIER = '242480CF-3CC8-4A0D-90FE-F3A99EA994ED',
        @JohnGrishamId              UNIQUEIDENTIFIER = '1f066c4d-81e2-447e-b87d-e6cbcdb9ae66',
        @NeilGaimanId               UNIQUEIDENTIFIER = '1781813c-435d-4908-b0a4-84004231ffdf',
        @DoubledayId                UNIQUEIDENTIFIER = '084030A4-F0CA-4D9C-B567-1A5B259E7F87',
        @HarperCollinsId            UNIQUEIDENTIFIER = '268AFD6D-9D14-42D4-88F0-5C632760D941',
        @PengiunId                  UNIQUEIDENTIFIER = '131d69b3-8ecd-4340-a1d0-f5b82fd49262',
        @SimonSchusterId            UNIQUEIDENTIFIER = 'e0c38dee-8312-4183-b2dd-049c5b08cb75',
        @SycamoreRowId              UNIQUEIDENTIFIER = '5b11f420-9654-4b45-8ed5-090c2675ecfe',
        @ATimeForMercyId            UNIQUEIDENTIFIER = '672f60cd-7eeb-44cc-bd86-dd426cb33ae2',
        @ATimeToKillId              UNIQUEIDENTIFIER = 'c78c384e-1a67-4b67-b62c-6391a5e106bc',
        @GoodOmensId                UNIQUEIDENTIFIER = '1573e77b-cb0f-43c3-b2c8-c1476ae5bcc7',
        @NeverwhereId               UNIQUEIDENTIFIER = '63f87d41-1cc6-4f6e-8476-bdcd5f2407eb',
        @StardustId                 UNIQUEIDENTIFIER = '1c980293-3c6b-41e1-891c-6bdd46b50962';

INSERT INTO [dbo].[Author]
    VALUES
    (@StephenKingId, 'Stephen', 'King'),
    (@JohnGrishamId, 'John', 'Grisham'),
    (@JoeAbercrombieId, 'Joe', 'Abercrombie'),
    (@NeilGaimanId, 'Neil', 'Gaiman');


INSERT INTO [dbo].[Publisher]
    VALUES
    (@DoubledayId, 'Doubleday'),
    (@SimonSchusterId, 'Simon & Schuster'),
    (@HarperCollinsId, 'HarperCollins'),
    (@PengiunId, 'Penguin');
    
INSERT INTO [dbo].[Book]
    VALUES
    (@BeforeTheyAreHangedId, @JoeAbercrombieId, @HarperCollinsId, 'Before They Are Hanged'),
    (@TheBladeItselfId, @JoeAbercrombieId, @HarperCollinsId, 'The Blade Itself'),
    (@LastArgumentOfKingsId, @JoeAbercrombieId, @HarperCollinsId, 'Last Argument of Kings'),
    (@GoodOmensId, @NeilGaimanId, @SimonSchusterId, 'Good Omens'),
    (@NeverwhereId, @NeilGaimanId, @SimonSchusterId, 'Neverwhere'),
    (@StardustId, @NeilGaimanId, @SimonSchusterId, 'Stardust'),
    (@CarrieId, @StephenKingId, @DoubledayId, 'Carrie'),
    (@ChristineId, @StephenKingId, @DoubledayId, 'Christine'),
    (@TheShiningId, @StephenKingId, @DoubledayId, 'The Shining'),
    (@ATimeForMercyId, @JohnGrishamId, @PengiunId, 'A Time for Mercy'),
    (@SycamoreRowId, @JohnGrishamId, @PengiunId, 'Sycamore Row'),
    (@ATimeToKillId, @JohnGrishamId, @PengiunId, 'A Time to Kill');

INSERT INTO [dbo].[Price]
    (BookId, Currency, [Value])
    VALUES
    (@BeforeTheyAreHangedId, 'USD', 12.99),
    (@TheBladeItselfId, 'USD', 13.99),
    (@LastArgumentOfKingsId, 'USD', 14.99),
    (@GoodOmensId, 'USD', 15.99),
    (@NeverwhereId, 'USD', 16.99),
    (@StardustId, 'USD', 17.99),
    (@CarrieId, 'USD', 18.99),
    (@ChristineId, 'USD', 19.99),
    (@TheShiningId, 'USD', 20.99),
    (@ATimeForMercyId, 'USD', 21.99),
    (@SycamoreRowId, 'USD', 22.99),
    (@ATimeToKillId, 'USD', 23.99);
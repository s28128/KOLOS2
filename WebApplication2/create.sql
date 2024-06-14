-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2024-06-08 12:06:39.879

-- tables
-- Table: Client
CREATE TABLE Client (
                        IdClient int  NOT NULL,
                        FirstName nvarchar(100)  NOT NULL,
                        LastName nvarchar(100)  NOT NULL,
                        Email nvarchar(100)  NOT NULL,
                        Phone nvarchar(100)  NULL,
                        CONSTRAINT Client_pk PRIMARY KEY  (IdClient)
);

-- Table: Discount
CREATE TABLE Discount (
                          IdDiscount int  NOT NULL,
                          Value int  NOT NULL,
                          IdSubscription int  NOT NULL,
                          DateFrom date  NOT NULL,
                          DateTo date  NOT NULL,
                          CONSTRAINT Discount_pk PRIMARY KEY  (IdDiscount)
);

-- Table: Payment
CREATE TABLE Payment (
                         IdPayment int  NOT NULL,
                         Date date  NOT NULL,
                         IdClient int  NOT NULL,
                         IdSubscription int  NOT NULL,
                         CONSTRAINT Payment_pk PRIMARY KEY  (IdPayment)
);

-- Table: Sale
CREATE TABLE Sale (
                      IdSale int  NOT NULL,
                      IdClient int  NOT NULL,
                      IdSubscription int  NOT NULL,
                      CreatedAt date  NOT NULL,
                      CONSTRAINT Sale_pk PRIMARY KEY  (IdSale)
);

-- Table: Subscription
CREATE TABLE Subscription (
                              IdSubscription int  NOT NULL,
                              Name nvarchar(100)  NOT NULL,
                              RenewalPeriod int  NOT NULL,
                              EndTime date  NOT NULL,
                              Price money  NOT NULL,
                              CONSTRAINT Subscription_pk PRIMARY KEY  (IdSubscription)
);

-- foreign keys
-- Reference: Discount_Subscription (table: Discount)
ALTER TABLE Discount ADD CONSTRAINT Discount_Subscription
    FOREIGN KEY (IdSubscription)
        REFERENCES Subscription (IdSubscription);

-- Reference: Payment_Client (table: Payment)
ALTER TABLE Payment ADD CONSTRAINT Payment_Client
    FOREIGN KEY (IdClient)
        REFERENCES Client (IdClient);

-- Reference: Payment_Subscription (table: Payment)
ALTER TABLE Payment ADD CONSTRAINT Payment_Subscription
    FOREIGN KEY (IdSubscription)
        REFERENCES Subscription (IdSubscription);

-- Reference: Table_5_Client (table: Sale)
ALTER TABLE Sale ADD CONSTRAINT Table_5_Client
    FOREIGN KEY (IdClient)
        REFERENCES Client (IdClient);

-- Reference: Table_5_Subscription (table: Sale)
ALTER TABLE Sale ADD CONSTRAINT Table_5_Subscription
    FOREIGN KEY (IdSubscription)
        REFERENCES Subscription (IdSubscription);

-- End of file.


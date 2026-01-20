-- IARA Database Setup Script
-- Run this script in SQL Server Management Studio against your 'iara' database

-- Drop existing tables if they exist (in correct order due to foreign keys)
IF OBJECT_ID('TICKETS', 'U') IS NOT NULL DROP TABLE TICKETS;
IF OBJECT_ID('INSPECTIONS', 'U') IS NOT NULL DROP TABLE INSPECTIONS;
IF OBJECT_ID('LOGBOOK', 'U') IS NOT NULL DROP TABLE LOGBOOK;
IF OBJECT_ID('CATCH_QUOTAS', 'U') IS NOT NULL DROP TABLE CATCH_QUOTAS;
IF OBJECT_ID('PERMITS', 'U') IS NOT NULL DROP TABLE PERMITS;
IF OBJECT_ID('VESSELS', 'U') IS NOT NULL DROP TABLE VESSELS;
IF OBJECT_ID('SPECIES', 'U') IS NOT NULL DROP TABLE SPECIES;
IF OBJECT_ID('PERSONNEL', 'U') IS NOT NULL DROP TABLE PERSONNEL;

-- Create PERSONNEL table
CREATE TABLE PERSONNEL (
    person_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    role NVARCHAR(50) NOT NULL,
    contact_email NVARCHAR(100) NULL,
    password_hash NVARCHAR(256) NULL,
    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL
);

-- Create SPECIES table
CREATE TABLE SPECIES (
    species_id INT IDENTITY(1,1) PRIMARY KEY,
    species_name NVARCHAR(100) NOT NULL UNIQUE,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL
);

-- Create VESSELS table
CREATE TABLE VESSELS (
    vessel_id INT IDENTITY(1,1) PRIMARY KEY,
    reg_number NVARCHAR(50) NOT NULL UNIQUE,
    vessel_name NVARCHAR(100) NOT NULL,
    captain_id INT NULL,
    owner_details NVARCHAR(500) NULL,
    tonnage DECIMAL(10,2) NULL,
    length_m DECIMAL(8,2) NULL,
    width_m DECIMAL(8,2) NULL,
    engine_power_kw DECIMAL(10,2) NULL,
    fuel_type NVARCHAR(50) NULL,
    displacement_tons DECIMAL(10,2) NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_VESSELS_Captain FOREIGN KEY (captain_id) REFERENCES PERSONNEL(person_id)
);

-- Create PERMITS table
CREATE TABLE PERMITS (
    permit_id INT IDENTITY(1,1) PRIMARY KEY,
    vessel_id INT NOT NULL,
    issue_date DATE NOT NULL,
    expiry_date DATE NOT NULL,
    is_active BIT DEFAULT 1,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_PERMITS_Vessel FOREIGN KEY (vessel_id) REFERENCES VESSELS(vessel_id)
);

-- Create CATCH_QUOTAS table
CREATE TABLE CATCH_QUOTAS (
    quota_id INT IDENTITY(1,1) PRIMARY KEY,
    permit_id INT NOT NULL,
    species_id INT NOT NULL,
    year SMALLINT NOT NULL,
    min_catch_kg DECIMAL(12,2) NULL,
    avg_catch_kg DECIMAL(12,2) NULL,
    max_catch_kg DECIMAL(12,2) NOT NULL,
    fuel_hours_limit INT NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_CATCH_QUOTAS_Permit FOREIGN KEY (permit_id) REFERENCES PERMITS(permit_id),
    CONSTRAINT FK_CATCH_QUOTAS_Species FOREIGN KEY (species_id) REFERENCES SPECIES(species_id),
    CONSTRAINT UQ_CATCH_QUOTAS_PermitSpeciesYear UNIQUE (permit_id, species_id, year)
);

-- Create LOGBOOK table
CREATE TABLE LOGBOOK (
    log_entry_id INT IDENTITY(1,1) PRIMARY KEY,
    vessel_id INT NOT NULL,
    captain_id INT NOT NULL,
    start_time DATETIME2 NOT NULL,
    duration_hours INT NULL,
    latitude DECIMAL(9,6) NULL,
    longitude DECIMAL(9,6) NULL,
    species_id INT NOT NULL,
    catch_kg DECIMAL(10,2) NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_LOGBOOK_Vessel FOREIGN KEY (vessel_id) REFERENCES VESSELS(vessel_id),
    CONSTRAINT FK_LOGBOOK_Captain FOREIGN KEY (captain_id) REFERENCES PERSONNEL(person_id),
    CONSTRAINT FK_LOGBOOK_Species FOREIGN KEY (species_id) REFERENCES SPECIES(species_id)
);

-- Create INSPECTIONS table
CREATE TABLE INSPECTIONS (
    inspection_id INT IDENTITY(1,1) PRIMARY KEY,
    vessel_id INT NOT NULL,
    inspector_id INT NOT NULL,
    inspection_date DATETIME2 NOT NULL,
    is_legal BIT DEFAULT 1,
    notes NVARCHAR(MAX) NULL,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_INSPECTIONS_Vessel FOREIGN KEY (vessel_id) REFERENCES VESSELS(vessel_id),
    CONSTRAINT FK_INSPECTIONS_Inspector FOREIGN KEY (inspector_id) REFERENCES PERSONNEL(person_id)
);

-- Create TICKETS table
CREATE TABLE TICKETS (
    ticket_id INT IDENTITY(1,1) PRIMARY KEY,
    ticket_number NVARCHAR(50) NOT NULL UNIQUE,
    inspection_id INT NOT NULL,
    penalty_amount DECIMAL(10,2) NULL,
    is_validated BIT DEFAULT 0,
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 NULL,
    CONSTRAINT FK_TICKETS_Inspection FOREIGN KEY (inspection_id) REFERENCES INSPECTIONS(inspection_id)
);

-- Insert sample data

-- Insert Personnel (Captains and Inspectors)
-- Default password for all users is "password123" (hashed with BCrypt)
INSERT INTO PERSONNEL (name, role, contact_email, password_hash, is_active) VALUES 
('Ivan Petrov', 'Captain', 'ivan.petrov@example.com', '$2a$12$I5SdzcrgrWrxMAgb4t1aj.4S.IP5oMQqq52pekQMly2JrFqok6/8G', 1),
('Maria Dimitrova', 'Captain', 'maria.d@example.com', '$2a$12$I5SdzcrgrWrxMAgb4t1aj.4S.IP5oMQqq52pekQMly2JrFqok6/8G', 1),
('Georgi Ivanov', 'Inspector', 'g.ivanov@iara.bg', '$2a$12$I5SdzcrgrWrxMAgb4t1aj.4S.IP5oMQqq52pekQMly2JrFqok6/8G', 1),
('Elena Stoyanova', 'Inspector', 'e.stoyanova@iara.bg', '$2a$12$I5SdzcrgrWrxMAgb4t1aj.4S.IP5oMQqq52pekQMly2JrFqok6/8G', 1),
('Admin User', 'Admin', 'admin@iara.bg', '$2a$12$I5SdzcrgrWrxMAgb4t1aj.4S.IP5oMQqq52pekQMly2JrFqok6/8G', 1);

-- Insert Species (expanded list of Black Sea fish species)
INSERT INTO SPECIES (species_name) VALUES 
('European Sprat'),
('Turbot'),
('Red Mullet'),
('Rapana Venosa'),
('Black Sea Anchovy'),
('Horse Mackerel'),
('Whiting'),
('Black Sea Shad'),
('Garfish'),
('Bluefish'),
('Grey Mullet'),
('Gobies');

-- Insert Vessels (expanded fleet)
INSERT INTO VESSELS (reg_number, vessel_name, captain_id, owner_details, tonnage, length_m, width_m, engine_power_kw, fuel_type, displacement_tons) VALUES 
('BG-VAR-001', 'Морска Звезда', 1, 'Петров ЕООД, Варна, ул. Морска 15', 50.5, 18.5, 5.2, 220.0, 'Diesel', 45.0),
('BG-VAR-002', 'Черно Море', 2, 'Рибарска Компания АД, Варна', 75.0, 22.0, 6.5, 350.0, 'Diesel', 68.0),
('BG-BUR-001', 'Златна Рибка', 1, 'Димитрова ООД, Бургас', 35.0, 15.0, 4.5, 180.0, 'Diesel', 30.0),
('BG-VAR-003', 'Нептун', 2, 'Нептун Фишинг ЕООД, Варна', 120.0, 28.0, 7.8, 500.0, 'Diesel', 105.0),
('BG-BUR-002', 'Делфин', 1, 'Делфин Груп АД, Бургас', 45.0, 16.5, 4.8, 200.0, 'Diesel', 40.0),
('BG-VAR-004', 'Посейдон', 2, 'Посейдон Марин ООД, Варна', 85.0, 24.0, 7.0, 420.0, 'Diesel', 78.0),
('BG-SOZ-001', 'Созопол', 1, 'Община Созопол - Рибарско Сдружение', 28.0, 12.0, 3.8, 150.0, 'Diesel', 24.0),
('BG-NES-001', 'Несебър Стар', 2, 'Старият Несебър ЕООД', 55.0, 19.0, 5.5, 280.0, 'Diesel', 48.0);

-- Insert Permits (all vessels have permits)
INSERT INTO PERMITS (vessel_id, issue_date, expiry_date, is_active) VALUES 
(1, '2025-01-01', '2025-12-31', 1),
(2, '2025-01-01', '2025-12-31', 1),
(3, '2024-06-01', '2025-05-31', 1),
(4, '2025-01-01', '2025-12-31', 1),
(5, '2025-03-01', '2026-02-28', 1),
(6, '2024-12-01', '2025-11-30', 1),
(7, '2025-01-15', '2026-01-14', 1),
(8, '2024-09-01', '2025-08-31', 1);

-- Insert Catch Quotas (comprehensive quotas for 2025 and 2026)
INSERT INTO CATCH_QUOTAS (permit_id, species_id, year, min_catch_kg, avg_catch_kg, max_catch_kg, fuel_hours_limit) VALUES 
-- Permit 1 (Морска Звезда)
(1, 1, 2025, 5000.00, 7500.00, 10000.00, 200),
(1, 2, 2025, 1000.00, 1500.00, 2000.00, 150),
(1, 5, 2025, 3000.00, 4500.00, 6000.00, 180),
(1, 1, 2026, 5500.00, 8000.00, 11000.00, 220),
-- Permit 2 (Черно Море)
(2, 1, 2025, 8000.00, 12000.00, 15000.00, 300),
(2, 4, 2025, 15000.00, 20000.00, 25000.00, 400),
(2, 6, 2025, 4000.00, 6000.00, 8000.00, 200),
(2, 1, 2026, 9000.00, 13000.00, 17000.00, 320),
-- Permit 3 (Златна Рибка)
(3, 3, 2025, 2000.00, 3000.00, 4000.00, 150),
(3, 5, 2025, 2500.00, 3500.00, 5000.00, 160),
-- Permit 4 (Нептун)
(4, 1, 2025, 12000.00, 18000.00, 25000.00, 450),
(4, 4, 2025, 20000.00, 30000.00, 40000.00, 500),
(4, 2, 2025, 3000.00, 4500.00, 6000.00, 250),
(4, 1, 2026, 15000.00, 22000.00, 30000.00, 500),
-- Permit 5 (Делфин)
(5, 5, 2025, 4000.00, 6000.00, 8000.00, 200),
(5, 6, 2025, 3000.00, 4500.00, 6000.00, 180),
-- Permit 6 (Посейдон)
(6, 1, 2025, 10000.00, 15000.00, 20000.00, 380),
(6, 4, 2025, 18000.00, 25000.00, 32000.00, 420),
-- Permit 7 (Созопол)
(7, 3, 2025, 1500.00, 2000.00, 2500.00, 120),
(7, 7, 2025, 1000.00, 1500.00, 2000.00, 100),
-- Permit 8 (Несебър Стар)
(8, 1, 2025, 6000.00, 9000.00, 12000.00, 250),
(8, 5, 2025, 5000.00, 7500.00, 10000.00, 220);

-- Insert sample Logbook entries (comprehensive fishing activity data)
INSERT INTO LOGBOOK (vessel_id, captain_id, start_time, duration_hours, latitude, longitude, species_id, catch_kg) VALUES 
-- January 2025 fishing trips
(1, 1, '2025-01-05 05:00:00', 10, 43.1833, 27.9167, 1, 320.5),
(1, 1, '2025-01-08 06:00:00', 8, 43.2100, 27.9500, 1, 185.0),
(1, 1, '2025-01-12 05:30:00', 9, 43.1950, 27.8800, 2, 92.5),
(1, 1, '2025-01-15 06:00:00', 8, 43.1833, 27.9167, 1, 250.5),
(1, 1, '2025-01-16 05:30:00', 10, 43.2000, 28.0000, 2, 85.0),
(1, 1, '2025-01-18 04:30:00', 11, 43.2200, 27.9800, 5, 410.0),
(2, 2, '2025-01-06 04:00:00', 14, 42.4833, 27.4667, 4, 1850.0),
(2, 2, '2025-01-10 03:30:00', 12, 42.5100, 27.5200, 4, 1420.0),
(2, 2, '2025-01-15 04:00:00', 12, 42.4833, 27.4667, 4, 1200.0),
(2, 2, '2025-01-19 03:00:00', 15, 42.4500, 27.4200, 1, 890.0),
(3, 1, '2025-01-07 06:30:00', 7, 42.4200, 27.6800, 3, 145.0),
(3, 1, '2025-01-14 07:00:00', 6, 42.4350, 27.7100, 5, 220.0),
(4, 2, '2025-01-04 02:30:00', 16, 43.0500, 28.5000, 1, 1650.0),
(4, 2, '2025-01-09 03:00:00', 14, 43.0800, 28.4500, 4, 2100.0),
(4, 2, '2025-01-13 02:00:00', 18, 43.1200, 28.6000, 1, 1890.0),
(4, 2, '2025-01-17 03:30:00', 15, 43.0300, 28.3800, 2, 420.0),
(5, 1, '2025-01-11 05:00:00', 9, 42.3800, 27.7500, 5, 380.0),
(5, 1, '2025-01-16 04:30:00', 10, 42.4000, 27.7200, 6, 290.0),
(6, 2, '2025-01-05 03:00:00', 13, 43.1500, 28.2000, 1, 1120.0),
(6, 2, '2025-01-12 02:30:00', 14, 43.1800, 28.2500, 4, 1580.0),
(7, 1, '2025-01-08 07:00:00', 5, 42.4167, 27.6833, 3, 95.0),
(7, 1, '2025-01-15 06:30:00', 6, 42.4100, 27.6600, 7, 110.0),
(8, 2, '2025-01-06 04:30:00', 11, 42.6500, 27.7300, 1, 680.0),
(8, 2, '2025-01-14 05:00:00', 10, 42.6800, 27.7000, 5, 520.0);

-- Insert sample Inspections (variety of inspection results)
INSERT INTO INSPECTIONS (vessel_id, inspector_id, inspection_date, is_legal, notes) VALUES 
(1, 3, '2025-01-10 10:00:00', 1, 'All documents in order. Vessel compliant. Safety equipment checked and valid.'),
(2, 4, '2025-01-12 14:30:00', 1, 'Routine inspection. No issues found. Crew properly licensed.'),
(3, 3, '2025-01-08 09:00:00', 0, 'Expired safety equipment. Warning issued. Must renew within 30 days.'),
(4, 4, '2025-01-05 11:00:00', 1, 'Comprehensive inspection. All permits valid. Logbook properly maintained.'),
(4, 3, '2025-01-15 08:30:00', 1, 'Follow-up inspection. Previous issues resolved.'),
(5, 4, '2025-01-09 13:00:00', 1, 'First inspection of the year. All equipment functional.'),
(6, 3, '2025-01-11 10:30:00', 0, 'Minor logbook discrepancies found. Verbal warning issued.'),
(6, 4, '2025-01-18 09:00:00', 1, 'Re-inspection passed. Logbook issues corrected.'),
(7, 3, '2025-01-07 14:00:00', 1, 'Small vessel inspection. Compliant with regulations.'),
(8, 4, '2025-01-13 11:30:00', 0, 'GPS tracking device malfunction. Repair required within 14 days.'),
(1, 4, '2025-01-17 10:00:00', 1, 'Random spot check. All in order.'),
(2, 3, '2025-01-19 09:30:00', 1, 'Quota utilization check. Within allowed limits.');

-- Insert Tickets for violations
INSERT INTO TICKETS (ticket_number, inspection_id, penalty_amount, is_validated) VALUES 
('TKT-2025-001', 3, 500.00, 1),
('TKT-2025-002', 7, 250.00, 0),
('TKT-2025-003', 10, 750.00, 0);

PRINT 'Database setup completed successfully!';
PRINT 'Tables created: PERSONNEL, SPECIES, VESSELS, PERMITS, CATCH_QUOTAS, LOGBOOK, INSPECTIONS, TICKETS';
PRINT 'Sample data inserted.';

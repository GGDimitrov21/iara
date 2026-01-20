-- IARA Mock Data Insert Script
-- Run this script to add mock data WITHOUT dropping existing tables
-- Use this after running fix_personnel.sql to update password hashes

-- Clear existing data (in correct order due to foreign keys)
DELETE FROM TICKETS;
DELETE FROM INSPECTIONS;
DELETE FROM LOGBOOK;
DELETE FROM CATCH_QUOTAS;
DELETE FROM PERMITS;
DELETE FROM VESSELS;
DELETE FROM SPECIES;
-- Keep PERSONNEL data (users with valid passwords)

-- Insert Species (Black Sea fish species)
SET IDENTITY_INSERT SPECIES ON;
INSERT INTO SPECIES (species_id, species_name) VALUES 
(1, 'European Sprat'),
(2, 'Turbot'),
(3, 'Red Mullet'),
(4, 'Rapana Venosa'),
(5, 'Black Sea Anchovy'),
(6, 'Horse Mackerel'),
(7, 'Whiting'),
(8, 'Black Sea Shad'),
(9, 'Garfish'),
(10, 'Bluefish'),
(11, 'Grey Mullet'),
(12, 'Gobies');
SET IDENTITY_INSERT SPECIES OFF;

-- Insert Vessels (expanded fleet)
SET IDENTITY_INSERT VESSELS ON;
INSERT INTO VESSELS (vessel_id, reg_number, vessel_name, captain_id, owner_details, tonnage, length_m, width_m, engine_power_kw, fuel_type, displacement_tons) VALUES 
(1, 'BG-VAR-001', N'Морска Звезда', 1, N'Петров ЕООД, Варна, ул. Морска 15', 50.5, 18.5, 5.2, 220.0, 'Diesel', 45.0),
(2, 'BG-VAR-002', N'Черно Море', 2, N'Рибарска Компания АД, Варна', 75.0, 22.0, 6.5, 350.0, 'Diesel', 68.0),
(3, 'BG-BUR-001', N'Златна Рибка', 1, N'Димитрова ООД, Бургас', 35.0, 15.0, 4.5, 180.0, 'Diesel', 30.0),
(4, 'BG-VAR-003', N'Нептун', 2, N'Нептун Фишинг ЕООД, Варна', 120.0, 28.0, 7.8, 500.0, 'Diesel', 105.0),
(5, 'BG-BUR-002', N'Делфин', 1, N'Делфин Груп АД, Бургас', 45.0, 16.5, 4.8, 200.0, 'Diesel', 40.0),
(6, 'BG-VAR-004', N'Посейдон', 2, N'Посейдон Марин ООД, Варна', 85.0, 24.0, 7.0, 420.0, 'Diesel', 78.0),
(7, 'BG-SOZ-001', N'Созопол', 1, N'Община Созопол - Рибарско Сдружение', 28.0, 12.0, 3.8, 150.0, 'Diesel', 24.0),
(8, 'BG-NES-001', N'Несебър Стар', 2, N'Старият Несебър ЕООД', 55.0, 19.0, 5.5, 280.0, 'Diesel', 48.0);
SET IDENTITY_INSERT VESSELS OFF;

-- Insert Permits (all vessels have permits)
SET IDENTITY_INSERT PERMITS ON;
INSERT INTO PERMITS (permit_id, vessel_id, issue_date, expiry_date, is_active) VALUES 
(1, 1, '2025-01-01', '2025-12-31', 1),
(2, 2, '2025-01-01', '2025-12-31', 1),
(3, 3, '2024-06-01', '2025-05-31', 1),
(4, 4, '2025-01-01', '2025-12-31', 1),
(5, 5, '2025-03-01', '2026-02-28', 1),
(6, 6, '2024-12-01', '2025-11-30', 1),
(7, 7, '2025-01-15', '2026-01-14', 1),
(8, 8, '2024-09-01', '2025-08-31', 1);
SET IDENTITY_INSERT PERMITS OFF;

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
SET IDENTITY_INSERT INSPECTIONS ON;
INSERT INTO INSPECTIONS (inspection_id, vessel_id, inspector_id, inspection_date, is_legal, notes) VALUES 
(1, 1, 3, '2025-01-10 10:00:00', 1, N'All documents in order. Vessel compliant. Safety equipment checked and valid.'),
(2, 2, 4, '2025-01-12 14:30:00', 1, N'Routine inspection. No issues found. Crew properly licensed.'),
(3, 3, 3, '2025-01-08 09:00:00', 0, N'Expired safety equipment. Warning issued. Must renew within 30 days.'),
(4, 4, 4, '2025-01-05 11:00:00', 1, N'Comprehensive inspection. All permits valid. Logbook properly maintained.'),
(5, 4, 3, '2025-01-15 08:30:00', 1, N'Follow-up inspection. Previous issues resolved.'),
(6, 5, 4, '2025-01-09 13:00:00', 1, N'First inspection of the year. All equipment functional.'),
(7, 6, 3, '2025-01-11 10:30:00', 0, N'Minor logbook discrepancies found. Verbal warning issued.'),
(8, 6, 4, '2025-01-18 09:00:00', 1, N'Re-inspection passed. Logbook issues corrected.'),
(9, 7, 3, '2025-01-07 14:00:00', 1, N'Small vessel inspection. Compliant with regulations.'),
(10, 8, 4, '2025-01-13 11:30:00', 0, N'GPS tracking device malfunction. Repair required within 14 days.'),
(11, 1, 4, '2025-01-17 10:00:00', 1, N'Random spot check. All in order.'),
(12, 2, 3, '2025-01-19 09:30:00', 1, N'Quota utilization check. Within allowed limits.');
SET IDENTITY_INSERT INSPECTIONS OFF;

-- Insert Tickets for violations
INSERT INTO TICKETS (ticket_number, inspection_id, penalty_amount, is_validated) VALUES 
('TKT-2025-001', 3, 500.00, 1),
('TKT-2025-002', 7, 250.00, 0),
('TKT-2025-003', 10, 750.00, 0);

PRINT 'Mock data inserted successfully!';
PRINT '';
PRINT 'Summary:';
PRINT '  - 12 Species';
PRINT '  - 8 Vessels';
PRINT '  - 8 Permits';
PRINT '  - 22 Catch Quotas';
PRINT '  - 24 Logbook Entries';
PRINT '  - 12 Inspections';
PRINT '  - 3 Tickets';
PRINT '';
PRINT 'Login with: admin@iara.bg / password123';

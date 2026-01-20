-- Fix PERSONNEL table - Add missing columns
-- Run this in SSMS against your 'iara' database

-- First, check what columns exist
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'PERSONNEL';

-- Add password_hash column
ALTER TABLE PERSONNEL ADD password_hash NVARCHAR(256) NULL;
GO

-- Add is_active column  
ALTER TABLE PERSONNEL ADD is_active BIT DEFAULT 1;
GO

-- Update existing personnel with default password (password123 hashed with BCrypt)
UPDATE PERSONNEL 
SET password_hash = '$2a$12$I5SdzcrgrWrxMAgb4t1aj.4S.IP5oMQqq52pekQMly2JrFqok6/8G',
    is_active = 1;
GO

PRINT 'PERSONNEL table fixed!';
PRINT 'All users now have password: password123';

-- Show the updated table structure
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'PERSONNEL';

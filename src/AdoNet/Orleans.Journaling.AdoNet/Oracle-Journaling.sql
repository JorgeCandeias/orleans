-- The design criteria for this table are:
--
-- 1. It stores append-only log segments for Orleans journaling state machines as binary data.
--
-- 2. The table design should scale with the idea of tens or hundreds (or even more) types
-- of grains that may operate with even hundreds of thousands of grain IDs within each type.
--
-- 3. The table and its associated operations should remain stable. There should not be
-- structural reason for unexpected delays in operations. It should be possible to insert
-- data reasonably fast without resource contention.
--
-- 4. The index is designed to support fast lookups by GrainId and ordered retrieval by Version.
--
-- 5. The OperationType field tracks whether a segment is an append or a replacement (compaction).
--
-- 6. For compaction (Replace operations), all previous segments for a grain are deleted
-- and replaced with a single new segment at Version 1.

-- Create sequence for Id
BEGIN
    EXECUTE IMMEDIATE 'CREATE SEQUENCE OrleansJournalingStorage_SEQ START WITH 1 INCREMENT BY 1';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -955 THEN
            RAISE;
        END IF;
END;
/

-- Create table
DECLARE
    table_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO table_count 
    FROM user_tables 
    WHERE table_name = 'ORLEANSJOURNALINGSTORAGE';
    
    IF table_count = 0 THEN
        EXECUTE IMMEDIATE '
        CREATE TABLE OrleansJournalingStorage
        (
            -- Unique identifier for each log segment entry
            Id NUMBER(19) NOT NULL PRIMARY KEY,
            
            -- The grain identifier as a string
            GrainId NVARCHAR2(512) NOT NULL,
            
            -- The version/sequence number of this log segment
            Version NUMBER(19) NOT NULL,
            
            -- The binary log segment data
            SegmentData BLOB NOT NULL,
            
            -- Type of operation: ''Append'' or ''Replace''
            OperationType NVARCHAR2(50) NOT NULL,
            
            -- When this segment was created
            CreatedUtc TIMESTAMP NOT NULL
        )';
    END IF;
END;
/

-- Create trigger for auto-increment
CREATE OR REPLACE TRIGGER OrleansJournalingStorage_TR
BEFORE INSERT ON OrleansJournalingStorage
FOR EACH ROW
BEGIN
    IF :NEW.Id IS NULL THEN
        SELECT OrleansJournalingStorage_SEQ.NEXTVAL INTO :NEW.Id FROM DUAL;
    END IF;
END;
/

-- Create composite index on GrainId and Version for fast ordered lookups
DECLARE
    index_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO index_count 
    FROM user_indexes 
    WHERE index_name = 'IX_OJS_GRAINID_VERSION';
    
    IF index_count = 0 THEN
        EXECUTE IMMEDIATE '
        CREATE INDEX IX_OJS_GrainId_Version 
        ON OrleansJournalingStorage (GrainId, Version)';
    END IF;
END;
/

-- Create unique constraint to prevent duplicate versions for the same grain
DECLARE
    constraint_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO constraint_count 
    FROM user_constraints 
    WHERE constraint_name = 'UQ_OJS_GRAINID_VERSION';
    
    IF constraint_count = 0 THEN
        EXECUTE IMMEDIATE '
        ALTER TABLE OrleansJournalingStorage
        ADD CONSTRAINT UQ_OJS_GrainId_Version UNIQUE (GrainId, Version)';
    END IF;
END;
/

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

CREATE TABLE IF NOT EXISTS OrleansJournalingStorage
(
    -- Unique identifier for each log segment entry
    Id BIGSERIAL NOT NULL PRIMARY KEY,
    
    -- The grain identifier as a string
    GrainId VARCHAR(512) NOT NULL,
    
    -- The version/sequence number of this log segment
    Version BIGINT NOT NULL,
    
    -- The binary log segment data
    SegmentData BYTEA NOT NULL,
    
    -- Type of operation: 'Append' or 'Replace'
    OperationType VARCHAR(50) NOT NULL,
    
    -- When this segment was created
    CreatedUtc TIMESTAMP NOT NULL
);

-- Composite index on GrainId and Version for fast ordered lookups
CREATE INDEX IF NOT EXISTS IX_OrleansJournalingStorage_GrainId_Version 
    ON OrleansJournalingStorage (GrainId, Version);

-- Unique constraint to prevent duplicate versions for the same grain
-- This helps maintain log integrity
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'uq_orleansjournalingstorage_grainid_version'
    ) THEN
        ALTER TABLE OrleansJournalingStorage
        ADD CONSTRAINT UQ_OrleansJournalingStorage_GrainId_Version UNIQUE (GrainId, Version);
    END IF;
END
$$;

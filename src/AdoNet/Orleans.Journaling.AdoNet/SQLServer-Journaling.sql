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

IF OBJECT_ID(N'[OrleansJournalingStorage]', 'U') IS NULL
CREATE TABLE OrleansJournalingStorage
(
    -- Unique identifier for each log segment entry
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    
    -- The grain identifier as a string
    GrainId NVARCHAR(512) NOT NULL,
    
    -- The version/sequence number of this log segment
    Version BIGINT NOT NULL,
    
    -- The binary log segment data
    SegmentData VARBINARY(MAX) NOT NULL,
    
    -- Type of operation: 'Append' or 'Replace'
    OperationType NVARCHAR(50) NOT NULL,
    
    -- When this segment was created
    CreatedUtc DATETIME2 NOT NULL,
    
    -- Composite index on GrainId and Version for fast ordered lookups
    INDEX IX_OrleansJournalingStorage_GrainId_Version NONCLUSTERED (GrainId, Version)
);
GO

-- Optional: Add a unique constraint to prevent duplicate versions for the same grain
-- This helps maintain log integrity
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_OrleansJournalingStorage_GrainId_Version' AND object_id = OBJECT_ID(N'[OrleansJournalingStorage]'))
BEGIN
    ALTER TABLE OrleansJournalingStorage
    ADD CONSTRAINT UQ_OrleansJournalingStorage_GrainId_Version UNIQUE (GrainId, Version);
END
GO

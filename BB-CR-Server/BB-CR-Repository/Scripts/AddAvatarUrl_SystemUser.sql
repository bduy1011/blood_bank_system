-- Thêm cột AvatarUrl vào bảng SystemUser (nếu chưa có)
-- Chạy script này khi deploy để hỗ trợ tính năng avatar.
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.SystemUser') AND name = 'AvatarUrl'
)
BEGIN
    ALTER TABLE dbo.SystemUser ADD AvatarUrl NVARCHAR(500) NULL;
END
GO

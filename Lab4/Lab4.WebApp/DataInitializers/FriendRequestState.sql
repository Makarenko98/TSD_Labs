MERGE INTO FriendRequestState t1 USING (SELECT 1 id) t2 ON (t1.Id = 0)
WHEN MATCHED THEN UPDATE  SET IsPositive = CONVERT(bit, 'False'), Name = N'New'
WHEN NOT MATCHED THEN INSERT (Id, IsPositive, Name) VALUES (0, CONVERT(bit, 'False'), N'New');
MERGE INTO FriendRequestState t1 USING (SELECT 1 id) t2 ON (t1.Id = 1)
WHEN MATCHED THEN UPDATE  SET IsPositive = CONVERT(bit, 'True'), Name = N'Accepted'
WHEN NOT MATCHED THEN INSERT (Id, IsPositive, Name) VALUES (1, CONVERT(bit, 'True'), N'Accepted');
MERGE INTO FriendRequestState t1 USING (SELECT 1 id) t2 ON (t1.Id = 2)
WHEN MATCHED THEN UPDATE  SET IsPositive = CONVERT(bit, 'False'), Name = N'Postponed'
WHEN NOT MATCHED THEN INSERT (Id, IsPositive, Name) VALUES (2, CONVERT(bit, 'False'), N'Postponed');
MERGE INTO FriendRequestState t1 USING (SELECT 1 id) t2 ON (t1.Id = 3)
WHEN MATCHED THEN UPDATE  SET IsPositive = CONVERT(bit, 'False'), Name = N'Rejected'
WHEN NOT MATCHED THEN INSERT (Id, IsPositive, Name) VALUES (3, CONVERT(bit, 'False'), N'Rejected');
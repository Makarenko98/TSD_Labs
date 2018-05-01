select *
from Users

select *
from Chats c

insert into Chats (Name)
values (N'TestChat2');

select *
from Messages m

select *
from ChatUsers cu


insert into Messages (ChatId, Text, Time, UserId)
values (1, N'SomeText' + cast(getutcdate() as nvarchar(max)), default, 15);

declare @messageID int = 14;
declare @UserId int = 15;
select top 10 m.*
from Messages m
	join Messages m1 on m1.Id = @messageID
		and m.ChatId = m1.ChatId
		and m.Time < m1.Time
	join ChatUsers cu on m1.ChatId = cu.ChatId
		and @UserID = cu.UserId
order by m.time desc


select * from Users
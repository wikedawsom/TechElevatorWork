USE [npcampground];


DELETE FROM UserReservation;
DELETE FROM [User];
DELETE FROM RoleItem;
DELETE FROM reservation;
DELETE FROM [site];
DELETE FROM campground;
DELETE FROM park;


INSERT INTO [park] ([name], [location], [establish_date], [area], [visitors], [description]) 
VALUES (N'Acadia', N'Maine', CAST(N'1919-02-26' AS Date), 47389, 2563129, N'Good Description.');


INSERT INTO [dbo].[campground] ([park_id], [name], [open_from_mm], [open_to_mm], [daily_fee]) 
VALUES ((SELECT TOP 1 park.park_id FROM park), N'Blackwoods', 1, 12, 35.0000);


INSERT INTO [site] ([campground_id], [site_number], [max_occupancy], [accessible], [max_rv_length], [utilities]) 
VALUES ((SELECT TOP 1 campground.campground_id FROM campground), 1, 6, 0, 0, 0);


--

INSERT INTO [reservation] ([site_id], [name], [from_date], [to_date], [create_date]) 
VALUES ((SELECT TOP 1 [site].site_id FROM [site]), N'Christopher', CAST(N'2019-10-21' AS Date), CAST(N'2019-10-25' AS Date), CAST(N'2019-10-23T11:02:43.357' AS DateTime));


INSERT INTO [RoleItem] ([Id], [Name]) VALUES (1, N'Administrator');

INSERT INTO [RoleItem] ([Id], [Name]) VALUES (2, N'StandardUser');

--

INSERT INTO [User] ([FirstName], [LastName], [Username], [Email], [Hash], [RoleId], [Salt]) 
VALUES (N'Christopher', N'Rupp', N'cjr', N'chris@techelevator.com', N'/mn0B+0TG+lKEu/6b/ZKvoJlVFk=', 2, N'h2vIMfAwmgJpgu9q6IqCJw==');


--

INSERT INTO [UserReservation] ([UserId], [ReservationId]) 
VALUES ((SELECT TOP 1 [User].Id FROM [User]), (SELECT TOP 1 reservation.reservation_id FROM reservation));

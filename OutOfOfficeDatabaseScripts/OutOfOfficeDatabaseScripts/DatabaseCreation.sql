CREATE DATABASE OutOfOffice;
GO
USE OutOfOffice;
CREATE TABLE Employees (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FullName VARCHAR NOT NULL,
	Subdivision VARCHAR NOT NULL CHECK(Subdivision IN('HR', 'Development', 'Management', 'Administration')),
	Position VARCHAR NOT NULL CHECK(Position IN('HR Manager', 'Developer', 'Project Manager', 'Administrator')),
	Status BIT NOT NULL,
	PeoplePartner INT NOT NULL FOREIGN KEY REFERENCES Employees(ID),
	OutOfOfficeBalance INT NOT NULL,
	Photo IMAGE NULL
	);
GO
CREATE FUNCTION PeoplePartnerPosition
(
    @PartnerID	INT
)
RETURNS BIT
AS
    BEGIN
    RETURN CASE
        WHEN EXISTS 
            (
                SELECT ID, Position FROM Employees 
                WHERE ID = @PartnerID 
                AND Position = 'HR Manager'
            )
            THEN 1
        ELSE 0
    END
    END;
GO
ALTER TABLE Employees
ADD CONSTRAINT IsHRManager CHECK(dbo.PeoplePartnerPosition(PeoplePartner)=1);
GO

CREATE TABLE LeaveRequests (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Employee INT NOT NULL FOREIGN KEY REFERENCES Employees(ID),
	AbscenceReason VARCHAR NOT NULL CHECK(AbscenceReason IN('Sickness', 'Vacation', 'Family Emergency', 'Other')),
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	Comment TEXT NULL,
	Status VARCHAR NOT NULL CHECK(Status IN('New', 'Approved')) DEFAULT 'New',
	);

CREATE TABLE ApprovalRequests (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Approver INT NOT NULL FOREIGN KEY REFERENCES Employees(ID),
	LeaveRequest INT NOT NULL FOREIGN KEY REFERENCES LeaveRequests(ID),
	Status VARCHAR NOT NULL CHECK(Status IN('New', 'Approved')) DEFAULT 'New',
	Comment TEXT NULL,
	);

CREATE TABLE Projects (
	ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ProjectType VARCHAR NOT NULL CHECK(ProjectType IN('Backend', 'Frontend', 'Database')),
	StartDate DATE NOT NULL,
	EndDate DATE NULL,
	ProjectManager INT NOT NULL FOREIGN KEY REFERENCES Employees(ID),
	Comment TEXT NULL,
	Status VARCHAR NOT NULL CHECK(Status IN('Active', 'Inactive')),
	);
GO
CREATE FUNCTION ProjectManagerPosition
(
    @ProjectManagerID	INT
)
RETURNS BIT
AS
    BEGIN
    RETURN CASE
        WHEN EXISTS 
            (
                SELECT ID, Position FROM Employees 
                WHERE ID = @ProjectManagerID 
                AND Position = 'Project Manager'
            )
            THEN 1
        ELSE 0
    END
    END;
GO
ALTER TABLE Projects
ADD CONSTRAINT IsProjectManager CHECK(dbo.ProjectManagerPosition(ProjectManager)=1);
GO
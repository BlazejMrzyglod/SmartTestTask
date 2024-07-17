USE OutOfOffice;
INSERT INTO Employees (FullName, Subdivision, Position, Status, PeoplePartner, OutOfOfficeBalance)
VALUES ('John Doe', 'HR', 'HR Manager', 'Active', 1, 30);

INSERT INTO Employees (FullName, Subdivision, Position, Status, PeoplePartner, OutOfOfficeBalance)
VALUES ('Diana Lynch', 'HR', 'HR Manager', 'Active', 1, 30);

UPDATE Employees
SET PeoplePartner=2
WHERE ID=1;

INSERT INTO Employees (FullName, Subdivision, Position, Status, PeoplePartner, OutOfOfficeBalance)
VALUES ('Harrison Taylor', 'Development', 'Developer', 'Active', 2, 30);

INSERT INTO Employees (FullName, Subdivision, Position, Status, PeoplePartner, OutOfOfficeBalance)
VALUES ('Deborah Morris', 'Management', 'Project Manager', 'Active', 2, 30);

INSERT INTO Employees (FullName, Subdivision, Position, Status, PeoplePartner, OutOfOfficeBalance)
VALUES ('Ila Salazar', 'Administration', 'Administrator', 'Active', 1, 30);	

INSERT INTO LeaveRequests (Employee, AbscenceReason, StartDate, EndDate)
VALUES (3, 'Sickness', GETDATE(), GETDATE()+3);	

INSERT INTO LeaveRequests (Employee, AbscenceReason, StartDate, EndDate)
VALUES (4, 'Vacation', GETDATE(), GETDATE()+7);	

INSERT INTO ApprovalRequests (Approver, LeaveRequest)
VALUES (1, 1);

INSERT INTO ApprovalRequests (Approver, LeaveRequest)
VALUES (2, 2);

INSERT INTO Projects (ProjectType, StartDate, ProjectManager, Status)
VALUES ('Backend', GETDATE(), 4, 'Active');

INSERT INTO Projects (ProjectType, StartDate, ProjectManager, Status)
VALUES ('Frontend', GETDATE(), 4, 'Active');

INSERT INTO ProjectsAndEmployees(EmployeeId, ProjectId)
VALUES (4, 1)

INSERT INTO ProjectsAndEmployees(EmployeeId, ProjectId)
VALUES (4, 2)
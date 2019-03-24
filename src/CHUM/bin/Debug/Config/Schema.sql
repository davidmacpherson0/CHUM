/*-----------Clear the Database first----------*/

DROP TABLE IF EXISTS 'Classes_Users_Bridge';
DROP TABLE IF EXISTS 'Users';
DROP TABLE IF EXISTS 'User_Type';
DROP TABLE IF EXISTS 'Subjects';
DROP TABLE IF EXISTS 'Classes';
DROP TABLE IF EXISTS 'Exports';
DROP TABLE IF EXISTS 'Filter_Classes_Bridge';
DROP TABLE IF EXISTS 'Filter_Users_Bridge';

/*-----------Create Refrence Tables------------*/

CREATE TABLE IF NOT EXISTS 'User_Type' (
	'ID'	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	'Label'	varchar(256)
);

CREATE TABLE IF NOT EXISTS 'Exports' (
	'ID' INTEGER PRIMARY KEY AUTOINCREMENT, 
	'Name' TEXT
);

/*---------------------------------------------*/

/*----------Creating Main Data Tables----------*/

CREATE TABLE IF NOT EXISTS 'Users' (
	'ID'	varchar(20) NOT NULL PRIMARY KEY,
	'Barcode'	varchar(256),
    'RFID'	varchar(256),
	'First_Name'	varchar(256),
	'Last_Name'	varchar(256),
	'Form_Class'	varchar(256),
	'Preferred_First_Name'	varchar(256),
	'Preferred_Last_Name'	varchar(256),
	'DOB'	datetime,
	'Year_Level'	varchar(5),
	'UserName'	varchar(10),
	'Sex'	varchar(1),
	'Enrolment_Status'	varchar(2),
	'Date_Enrolled'	datetime,
	'Exit_Date'	datetime,
	'House'	varchar(50),
	'Indigenous_Status'	integer,
	'Independent_Status'	varchar(1),
	'User_Type_ID'	integer,
	FOREIGN KEY('User_Type_ID') REFERENCES 'User_Type'('ID')
);

CREATE TABLE IF NOT EXISTS 'Classes' (
	'Class_Code'	varchar(100) NOT NULL PRIMARY KEY,
    'Name'	varchar(512) NOT NULL,
	'Subject_Prefix'	varchar(10),
	'Year_Level'	varchar(5),
	'Semeseter_ID'	integer,
	'Class_Level'	varchar(1),
	FOREIGN KEY('Subject_Prefix') REFERENCES 'Subjects'('Prefix')
);

CREATE TABLE IF NOT EXISTS 'Subjects' (
	'Name'	varchar(512),
	'Prefix'	varchar(10) NOT NULL PRIMARY KEY
);

/*---------------------------------------------*/

/*----------Creating Bridgeing Tables----------*/

CREATE TABLE IF NOT EXISTS 'Classes_Users_Bridge' (
	'ID'	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
	'Classes_Class_Code'	varchar(100),
	'Users_ID'	varchar(20),
	FOREIGN KEY('Classes_Class_Code') REFERENCES 'Classes'('Class_Code'),
	FOREIGN KEY('Users_ID') REFERENCES 'Users'('ID')
);            

CREATE TABLE IF NOT EXISTS 'Filter_Users_Bridge'( 
	'ID' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
	'Users_ID' varchar(20), 
	'Exports_ID' INTEGER, 
	FOREIGN KEY('Users_ID') REFERENCES 'Users'('ID'), 
	FOREIGN KEY('Exports_ID') REFERENCES 'Exports'('ID')
);

CREATE TABLE IF NOT EXISTS 'Filter_Classes_Bridge'( 
	'ID' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
	'Classes_Class_Code' varchar(100), 
	'Exports_ID' INTEGER, 
	FOREIGN KEY('Classes_Class_Code') REFERENCES 'Classes'('Class_Code'), 
	FOREIGN KEY('Exports_ID') REFERENCES 'Exports'('ID')
);

/*---------------------------------------------*/


/*----Inserting Data into Reference Tables-----*/

INSERT INTO 'User_Type' (ID, Label) VALUES 
	(1,'Student'), 
	(2, 'Teacher');

INSERT INTO 'Exports' (ID,Name) VALUES 
	(1,'Click View'), 
	(2,'Infitini'), 
	(3,'Readcloud');

/*---------------------------------------------*/
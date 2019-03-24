# CHUM
What’s CHUM?
Image this
you are a technician working in a school environment 
you are in the middle of a rollover of devices 
100 brand new devices 
200 old devices 
the work bench is full
teachers and students coming at you left and right 
then all of a sudden, your manager comes to you and says 
	"The Vice Principal went to a conference and saw this great product
		that will make all our lives easier all you need to do is use the 
		exports from our Student Management System and upload it into theirs"

you think great I now have to take the exports and somehow generate the files to upload into this system

This is where CHUM comes in.
CHUM is design to create exports for external vendors 

You map your csv's to the correct fields using a JSON file and hey presto you have an export for a vendor

this is still early days for the project 
the more output file examples i can get the more files it will generate
so please send me a template file and i will update the exports

How to use it 
This is the process 
Import files -> Database -> Export Files

Importing Data
You map your import files to the database via a JSON File
the JSON file lives in Config\Maps\Import
What you need in the JSON File this is the Bare bones

{
  "TableType": "DAL.Database.Model.<Table in the Database>, DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
  "FileName": "<Your csv file>",
  "FieldMap": [
	{
      "FileFieldname": "<The Column name in the csv file>",
	  "TableFieldName": "<The Colum in the table>",
	  "IsIndex": <true = This is the primary key of the Table, if you don't know make don't include it or mark it as false>
    },
  ]
}

Table Type = The name of the Table you are Importing into
File Name = The Name of the csv data file that is in the .\Import Folder 
Field Map = is an array of what columns in the csv file that are being mapped to the Database table


if you have one export file that needs to map to more then one table 
just create another JSON file

for example 
You have an csv data file that has teacher and class data you would create 2 JSON Files 
one to map the teachers to the users table 
one to map the classes to the class table 

Once you have the mapping sorted out you can hit run 
it will generate the Exports 

The exports configuration is controlled by the JSON files in the .\Config\Export Folder
at the moment there is only 3 
	Click View
	Concord Infiniti
	ReadCloud
the goal is to get as many different types of exports as possible
to cater to all the different systems that are out there

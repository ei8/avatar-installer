BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "RegionPermit" (
	"SequenceId"	integer NOT NULL,
	"UserNeuronId"	varchar(36),
	"RegionNeuronId"	varchar(36),
	"WriteLevel"	integer,
	"ReadLevel"	integer,
	PRIMARY KEY("SequenceId" AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS "User" (
	"UserId"	TEXT NOT NULL,
	"NeuronId"	varchar(36) NOT NULL,
	"Active"	integer,
	PRIMARY KEY("UserId")
);

CREATE TABLE IF NOT EXISTS "NeuronPermit" (
	"UserNeuronId"	TEXT NOT NULL,
	"NeuronId"	TEXT NOT NULL,
	"ExpirationDate"	TEXT,
	PRIMARY KEY("UserNeuronId","NeuronId")
);


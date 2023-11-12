BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "RegionPermit" (
	"SequenceId"	integer NOT NULL,
	"UserNeuronId"	varchar(36),
	"RegionNeuronId"	varchar(36),
	"WriteLevel"	integer,
	"ReadLevel"	integer,
	PRIMARY KEY("SequenceId" AUTOINCREMENT)
);

INSERT OR IGNORE INTO "RegionPermit" VALUES(1, "672537e1-d813-479b-a2ca-931c6e831f19", "3114f835-7857-43d0-8e69-54abcbeef7a7", 1, 1);
INSERT OR IGNORE INTO "RegionPermit" VALUES(2, "672537e1-d813-479b-a2ca-931c6e831f19", NULL, 1, 1);
INSERT OR IGNORE INTO "RegionPermit" VALUES(3, "672537e1-d813-479b-a2ca-931c6e831f19", "a6dde287-7896-439e-83da-edac57394b9b", 1, 1);

CREATE TABLE IF NOT EXISTS "User" (
	"UserId"	TEXT NOT NULL,
	"NeuronId"	varchar(36) NOT NULL,
	"Active"	integer,
	PRIMARY KEY("UserId")
);

INSERT OR IGNORE INTO "User" VALUES("Author", "672537e1-d813-479b-a2ca-931c6e831f19", NULL);

CREATE TABLE IF NOT EXISTS "NeuronPermit" (
	"UserNeuronId"	TEXT NOT NULL,
	"NeuronId"	TEXT NOT NULL,
	"ExpirationDate"	TEXT,
	PRIMARY KEY("UserNeuronId","NeuronId")
);

INSERT OR IGNORE INTO "NeuronPermit" VALUES("672537e1-d813-479b-a2ca-931c6e831f19", "a1a3933a-d6e9-429c-bbf3-a09c6fdedddc", NULL);

COMMIT;

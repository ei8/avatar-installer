BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Notification" (
	"SequenceId"	integer NOT NULL,
	"Timestamp"	varchar,
	"TypeName"	varchar,
	"Id"	varchar,
	"Version"	integer,
	"AuthorId"	varchar,
	"Data"	varchar,
	PRIMARY KEY("SequenceId" AUTOINCREMENT)
);
CREATE UNIQUE INDEX IF NOT EXISTS "IdVersion" ON "Notification" (
	"Id",
	"Version"
);
COMMIT;

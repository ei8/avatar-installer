BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "View" (
	"Url"	varchar NOT NULL,
	"ParentUrl"	varchar,
	"Name"	varchar,
	"IsDefault"	integer,
	"Sequence"	integer,
	"Icon"	varchar,
	PRIMARY KEY("Url")
);
COMMIT;

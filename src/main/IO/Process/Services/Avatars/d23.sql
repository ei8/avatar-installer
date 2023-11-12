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

INSERT OR IGNORE INTO "View" VALUES("Messages", NULL, "Messages", 0, 3, "oi-envelope-closed");

COMMIT;

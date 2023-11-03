BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Resource" (
	"PathPattern"	varchar NOT NULL,
	"InUri"	varchar,
	"OutUri"	varchar,
	"Methods"	varchar,
	PRIMARY KEY("PathPattern")
);
COMMIT;

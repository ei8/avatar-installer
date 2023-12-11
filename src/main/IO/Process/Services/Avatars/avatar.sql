BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Resource" (
	"PathPattern"	varchar NOT NULL,
	"InUri"	varchar,
	"OutUri"	varchar,
	"Methods"	varchar,
	PRIMARY KEY("PathPattern")
);

INSERT OR IGNORE INTO "Resource" VALUES("^(nuclei\/d23.*)$", "http://cortex.diary.nucleus.in.api:80", "http://cortex.diary.nucleus.out.api:80", "GET,POST,PATCH,DELETE");
INSERT OR IGNORE INTO "Resource" VALUES("^(cortex\/(?:neurons|terminals){1}.*)$", NULL, "http://cortex.library.out.api:80", "GET");
INSERT OR IGNORE INTO "Resource" VALUES("^(nuclei\/chat.*)$", "http://cortex.chat.nucleus.in.api:80", "http://cortex.chat.nucleus.out.api:80", "GET,POST,PATCH,DELETE");

COMMIT;

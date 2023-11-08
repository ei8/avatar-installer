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

INSERT INTO "Notification" VALUES(1, "2020-06-04T14:22:03.0858674+00:00", 
	"neurUL.Cortex. Domain.Model.Neurons. NeuronCreated, neurUL. Cortex. Domain.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
	"672537e1-d813-479b-a2ca-931c6e831f19", 1, "672537e1-d813-479b-a2ca-931c6e831f19", 
	"{"Id":"672537e1-d813-479b-a2ca-931c6e831f19","Version":1,"Timestamp":"2020-06-04T14:22:03.0364363+00:00"}");

INSERT INTO "Notification" VALUES(2, "2020-06-04T14:22:04.7606448+00:00", 
	"ei8.Data.Tag.Domain.Model.TagChanged, ei8.Data.Tag.Domain.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
	"672537e1-d813-479b-a2ca-931c6e831f19", 2, "672537e1-d813-479b-a2ca-931c6e831f19", 
	"{"Tag":"Author Neuron","Id":"672537e1-d813-479b-a2ca-931c6e831f19","Version":2,"Timestamp":"2020-06-04T14:22:04.7024466+00:00"}");


COMMIT;

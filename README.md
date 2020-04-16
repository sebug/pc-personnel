# PC Personnel
From the Excel file that is filled out after the roll call and some other
information, provide a queryable API.


## Database stuff
We have a pgadmin container that we start as well. We don't check in the environment files containig the password.

For migrations, you currently can:

	docker exec -i pc-personnel_postgresql_1 psql -U postgres < migration-scripts/1.0.0/01.\ Vehicle.sql

etc.

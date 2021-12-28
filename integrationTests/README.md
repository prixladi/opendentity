# API tests for Opendentity

Source code for tests is located in the /tests folder.

## Run tests

To start the opendentity service and database run `docker-compose up -d`.

After that, you can run tests using `yarn dev` command which builds tests and runs them.

Containers have no mapped volumes so you can have a clean start just stopping containers using `docker-compose down` and starting them again `docker-compose up -d`. They actually need to be recreated before running tests again because tests need a fresh DB.

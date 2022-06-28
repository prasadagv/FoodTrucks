# Pending Items or Known Issues

1. Syncfoodtrucksdata and Authentication services are NOT for review as these are setup and supporting APIs to run actual foodtrucks API.
1. Azure key-vault is not used in the solution.  All the secrets are defined as plain text in configutaions section.
1. Didn't use any digital certificates to validate the client.
1. API Gateway or Azure AD is not integrated into the solution
1. CI/CD is not implemented
1. Need to mock the Cosmos Container and Cosmos Client in repository of foodtrucks api.
1. Non-functional testing (Performance, Load, UI..) is not done
1. NLog is implemented but configuations are not done to write the logs to blob storage or external source
1. Proper/enrich of error messages are not used and only a standard messages are sent as response.

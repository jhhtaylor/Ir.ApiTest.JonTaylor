# Intelligent Reach API Project

## Overview
This project consists of an API for managing products with features such as creation, retrieval, updating, and optional pagination and filtering. The implementation uses an in-memory database for simplicity.

## Key Features
- **Create Product**: Add new products with auto-generated timestamps.
- **Get Product(s)**: Retrieve single or multiple products with optional filtering based on the `LastUpdated` property and pagination support.
- **Update Product**: Apply partial updates with JSON Patch, ensuring some fields like `Id` and timestamps are not modifiable directly.

## Postman Collection
A Postman collection is included to facilitate testing of the API endpoints. Comments within the collection will guide you on how to use each request effectively.

## Future Enhancements
- **Unit Testing**: Future implementations would include unit tests using frameworks like xUnit to ensure each component functions correctly independently and together.
- **Auto-Increment ID**: Though not implemented in this submission, configuring the product ID to auto-increment could be achieved using annotations in the Entity Framework model to simplify object creation.

## Submission Note
Navigate to the Ir.ApiTest project and run `dotnet run`. Refer to the embedded Postman collection for endpoint testing.

Sincerely,
Jon Taylor

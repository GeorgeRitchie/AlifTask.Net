# AlifTask

## Objective

Develop a REST API for a financial institution that offers electronic wallet services to its partners. This institution handles two types of e-wallet accounts: identified and unidentified. The API must support multiple clients, exclusively using HTTP `POST` methods with JSON as the data format.

## Authentication

Clients are required to authenticate using HTTP headers `X-UserId` for user identification and `X-Digest` for request integrity. The `X-Digest` header represents an HMAC-SHA1 hash of the request body.

## Account Limits

There should be predefined e-wallets with variable balances. The maximum balance is 10,000 somoni for unidentified accounts and 100,000 somoni for identified accounts.

## Data Storage

You may choose any appropriate data storage solution for this task.

## API Service Methods

1. **Account Existence**: Check if an e-wallet account exists.
2. **Account Top-Up**: Add funds to an e-wallet.
3. **Monthly Transactions**: Retrieve the total number and sum of top-up transactions for the current month.
4. **Balance Inquiry**: Check the balance of an e-wallet.

## Development Guidelines

- Use Git and GitHub for version control, making sure to commit meaningful changes.
- Provide the result of your task as a link to your GitHub repository. Direct file submissions (e.g., .zip, .rar) will not be accepted.

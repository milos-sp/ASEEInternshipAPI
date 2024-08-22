# ASEE Internship Project - Personal Finance Management

# PFM Backend

Backend written in .NET Core, PostgreSQL used as a database

# Features

## Import transactions from csv file

- Enable import of bank transactions based on the format of transactions.csv file.
- Create relational DB schema for transactions with `id` as primary key.
- Validate input according to OAS3 spec.
- Persist transaction into database.

## List transactions with filters and pagination

- Enable paginated listing of transactions based on supplied filter conditions.
- Implement period filter (start-date and end-date).
- Implement transaction kinds filter as a list of acceptable transaction kinds.

## Import categories from csv file

- Enable import of spending categories based on the format of categories.csv file.
- Create relational DB schema for categories with `code` as primary key and foreign key from transactions to categories on `catcode` matching `code` field.
- Persist categories into database.

## Categorize single transaction

- Enable categorization of a single transaction.
- Validate that both category and transaction exists in database.

## Analytical view of spending by categories and subcategories

- Enable analytical views of spendings by categories and subcategories.
- Implement optional filters:
    - Category filter
    - Period filter (start-date and end-date)
    - Direction filter (debits or credits)

## Split transaction

- Enable split of transaction into multiple spending categories or subcategories.
- If transaction is already split, deleta existing splits and replace them with new ones.
- Validate that the transaction and categories exist.
- Create relational DB schema that can persist splits for a transaction.
- Extend transaction list endpoint to return splits for each transaction.
- Persist splits into database with `amount` and `catcode`.

## Automatically assign categories based on predefined rules

- Enable automatic assignment of categories and subcategories based on predefined rules in config file.
- If transaction already has a category assigned do not reasign it to automaticaly determined category.
- Each rule has a code of categery and SQL compliant predicate expression (filter condition) that defines which transactions should fall into the category.

**Examples of rules:**

```json
[
    {
        "field": "mcc",
        "description": "Food",
        "catcode": "39",
        "predicate": "\"Mcc\" = 5811"
    },
    {
        "field": "beneficiary-name",
        "description": "Gasoline",
        "catcode": "4",
        "predicate": "LOWER(\"BeneficiaryName\") LIKE '%chevron%' OR LOWER(\"BeneficiaryName\") LIKE '%shell%'"
    }
]
```


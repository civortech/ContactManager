# Contact Manager Demo

1. Domain & Constraints
	- A contact is either a customer or a supplier
	- A customer’s birthday is not required
	- An email address must be a valid email address
	- A supplier’s telephone number must be at least 7 numeric characters, maximum 12
	- A name can be 50 characters for both first and last name

2. Functional Requirements
	- Retrieve and show a paged list of contacts (both customers and suppliers in one view)
	- Create, edit and remove contacts from the list
	
3. Technical requirements
	1. The contact manager solution implements a classic 3 tier app, which containing:
		- A Web App
		- A Web API REST service
		- A Relational Database Management System
		
	2. High Level Architecture diagram:
		![GitHub Logo](https://embed.creately.com/5CU4N9dfbDW?type=svg)
	3. The contact manager application uses:
		- ASP.NET Core with [React](https://reactjs.org/)
		- ASP.NET Core Web API
		- SQL Server database
		- [DevExtreme](https://js.devexpress.com/) UI Component lib

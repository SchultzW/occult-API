# Code Review Form

|                                                      | --------------------------------------- |
| ---------------------------------------------------- | --------------------------------------- |
| Course  number, lab number and lab group             |  CS  296N , Lab 2                               |
| Developer                                            |  Will Schultz                                    |
| URL  for the project repository and branch on GitHub |  https://github.com/LCC-CS296N/labs-SchultzW/tree/Identity                                   |
| URL  on a server (if it has been published)          |                                         |
| Reviewer  and Date                                   |  Nicolette Molitor 1/20/20                                       |

###  Instructions

The reviewer will complete this form for the beta version of a lab assignment done by one of their lab partners. After filling out the “Beta” column and adding comments, the reviewer will submit this document to the Code Review assignment on the LMS.

The developer will revise the beta version of their lab work and fill out the “Production” column to reflect any changes they have made. The developer will submit this completed form along with the production version of their lab assignment.

### Review

| **Criteria**                                                 | **Beta** | **Release** |
| ------------------------------------------------------------ | -------- | ----------- |
| Does  it compile and run without errors?                     |   No       | Yes |
|                                                              |          |             |
| Do  all the pages load correctly?                            |  No        | Yes |
|                                                              |          |             |
| Does  the style conform to MVC conventions and our class standards? | yes         | Yes |
|                                                              |          |             |
| Do  all the links, buttons or other UI elements work correctly? |   no       | Yes |
|                                                              |          |             |
|                                                              |          |             |
| Do  the design and implementation conform to OOP best practices? |   yes       | Yes |
|                                                              |          |             |
|                                                              |          |             |
| Does  the style conform to C# coding conventions?            |    yes      | Yes |
|                                                              |          |             |
|                                                              |          |             |
| Does  the solution meet all the requirements? (list any issues below) |    not yet      | Yes |
|                                                              |          |             |
|                                                              |          |             |
|                                                              |          |             |
|                                                              |          |             |
|                                                              |          |             |

#### Comments:


I tried some things that got to the point of creating a user I made a list below.
1. In the AdminUser method in the AdminController, pass userManager.Users to the view().
2. Uncomment the appUsers line in the dbAppContext model.
3. Create a new migration and update the database.
4. The CreateUser view in the Admin view folder is misspelled. I added an 'e' to create and it worked.

After creating a user and hitting submit the user did not save to the database yet.





------

 

## Appendix

### Aspects of coding style to check

- Is proper indentation used?
- Are the HTML elements and variables named descriptively?
- Have any unnecessary lines of code or files been removed?
- Are there explanatory comments in the code?
- Do variable names use camelCase? 
- Are properties, methods and classes named using PascalCase (aka TitleCase)?
- Are constant names written using ALL_CAPS?

### Best practices in Object Oriented Programming

- Is the code DRY (no duplicated blocks of code)?
- Are named constants used instead of repeated literal constants?
- Is code that does computation or logical operations separated into its own class instead of being added to the code-behind?
- Are all instance variables private?
- Are local variables used instead of instance variables wherever possible?
- Does each method do just one thing (no “Swiss Armey” methods)?
- Are classes “loosely coupled” and “highly coherent”?

 

------

Written by Brian Bird, Lane Community College, winter 2017, updated winter 2020

------

